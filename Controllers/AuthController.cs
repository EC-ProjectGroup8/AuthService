
using AuthServices.Data.Dto;
using AuthServices.Data.Entities;
using AuthServices.Models;
using AuthServices.Services;
using AuthServices.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    //private readonly UserManager<UsersEntity> _userManager;
    private readonly IUserServices _userServices;
    private readonly SignInManager<UsersEntity> _signInManager;

    public AuthController(IUserServices userServices, SignInManager<UsersEntity> signInManager)

    //UserManager<UsersEntity> userManager,
    {
        _userServices = userServices;
        _signInManager = signInManager;
        //_userManager = userManager;
    }


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
        var emailIsAvailable = await _userServices.CheckUserByEmail(model.Email);
        if (!emailIsAvailable)
            return Conflict("Email is already in use.");

        if (model.Password != model.ConfirmPassword)
            return BadRequest();

        var created = await _userServices.CreateUser(model);

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
        var user = await _userServices.GetUserByEmail(model.Email);
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



    [HttpGet("GetUserById/{Id}")]
    public async Task<ActionResult<UserReturnData>> GetUserById(string Id)
    {
        if (Id == null) return BadRequest();
        var user = await _userServices.GetUserById(Id);
        return Ok(user);
    }

    [HttpGet("GetUserByEmail/{email}")]
    public async Task<ActionResult<bool>> GetUserByEmail(string email)
    {
        if (string.IsNullOrEmpty(email)) return BadRequest();
        var result = await _userServices.GetUserByEmail(email);
        return Ok(result);
    }



    [HttpGet("GetAllUsers")]
    public async Task<ActionResult<IEnumerable<UserReturnData>>> GetAllUsers()
    {
        var users = await _userServices.GetAllUsers();
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
       var result  = await _userServices.UpdateUser(user);
        return Ok(result);
        //if (result == true) return result("User updated succesfully");
    }



    [HttpDelete("Delete")]
    public async Task<ActionResult<bool>> DeleteUser(string Id)
    {
        if (Id == null) 
            return BadRequest();

        return await _userServices.DeleteUser(Id);
    }


}
   
