
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

    public AuthController(IUserServices userServices)

    //UserManager<UsersEntity> userManager,
    {
        _userServices = userServices;
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
        if (model.Password != model.ConfirmPassword)
            return BadRequest("Passwords do not match");

        var result = await _userServices.CreateUser(model);

        if (result == true)
            return Ok("User registered successfully!");

        return BadRequest();
    }



    [HttpGet("GetUserById/{Id}")]
    public async Task<ActionResult<UserReturnData>> GetUserById(string Id)
    {
        if (Id == null) return BadRequest();
        var user = await _userServices.GetUserById(Id);
        return Ok(user);
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
   
