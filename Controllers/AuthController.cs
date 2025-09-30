using AuthServices.Data.Dto;
using AuthServices.Data.Entities;
using AuthServices.Models;
using AuthServices.Services;
using AuthServices.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace AuthServices.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IUserService userService, SignInManager<UsersEntity> signInManager) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly SignInManager<UsersEntity> _signInManager = signInManager;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        var exists = await _userService.UserExistsByEmailAsync(model.Email);
        if (exists)
            return Conflict("Email is already in use.");

        if (model.Password != model.ConfirmPassword)
            return BadRequest();

        var created = await _userService.CreateUser(model);

        return created
             ? Ok("User registered successfully!")
             : BadRequest("Failed to register user");
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] SignInData model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }
        var user = await _userService.GetUserByEmail(model.Email);
        if (user == null)
            return Unauthorized("Invalid login attempt.");

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        return result.Succeeded
             ? Ok(user.Email)
             : BadRequest();
    }

    [HttpPost("signout")]
    public async Task<IActionResult> Signout()
    {
        await _signInManager.SignOutAsync();
        return Ok("User signed out successfully!");
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> RequestPasswordReset([FromBody] ForgotPasswordRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        await _userService.RequestPasswordReset(request.Email);

        //Always returns 200 Ok to prevent email enumeration
        return Ok();
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest("ModelState not valid.");

        var result = await _userService.ChangeForgottenPassword(request);

        return result
            ? Ok(new { success = true, message = "Password changed" })
            : BadRequest(result);
    }

    [HttpGet("GetUserById/{Id}")]
    public async Task<ActionResult<UserReturnData>> GetUserById(string Id)
    {
        if (Id == null) return BadRequest();
        var user = await _userService.GetUserById(Id);
        return Ok(user);
    }

    [HttpGet("GetUserByEmail/{email}")]
    public async Task<ActionResult<bool>> GetUserByEmail(string email)
    {
        if (string.IsNullOrEmpty(email)) return BadRequest();
        var result = await _userService.GetUserByEmail(email);
        return Ok(result);
    }



    [HttpGet("GetAllUsers")]
    public async Task<ActionResult<IEnumerable<UserReturnData>>> GetAllUsers()
    {
        var users = await _userService.GetAllUsers();
        return Ok(users);
    }



    [HttpPut("Update")]
    public async Task<ActionResult<bool>> UpdateUser(UsersEntity user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }
        var result = await _userService.UpdateUser(user);
        return Ok(result);
        //if (result == true) return result("User updated succesfully");
    }



    [HttpDelete("Delete")]
    public async Task<ActionResult<bool>> DeleteUser(string Id)
    {
        if (Id == null)
            return BadRequest();

        return await _userService.DeleteUser(Id);
    }


}