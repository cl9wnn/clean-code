using System.ComponentModel.DataAnnotations;
using API.Models;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(AccountService accountService): ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        var validator = new UserValidator();
        var validationResult = await validator.ValidateAsync(request);
        
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors.Select(errors => errors.ErrorMessage));
        
        await accountService.RegisterAsync(request.Email!, request.Password!, request.FirstName!);

        return Ok();
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest? request)
    {
        if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            return Unauthorized();
        
        var token = await accountService.LoginAsync(request.Email, request.Password);
        
        if (token == null)
            return Unauthorized();
        
        return Ok(token);
    }
}