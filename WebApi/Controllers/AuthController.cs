using Core.Interfaces.Services;
using Core.Models;
using Infraestructura.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers;

public class AuthController : BaseApiController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("validate-token")]
    public IActionResult ValidateToken([FromBody] string token)
    {
        if (!_authService.ValidateJwt(token))
            return BadRequest("Token no valido.");

        return Ok("Token valido.");
    }

    [HttpPost("generate-token")]
    public IActionResult GenerateToken([FromBody] User user)
    {
        return Ok(_authService.CreateToken(user));
    }

    [Authorize(Roles = "user,admin")]
    [HttpGet("User")]
    public IActionResult AuthUser()
    {
        var user = HttpContext.User;
        var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
        return Ok($"{string.Join(", ", roles)}");
    }

    [Authorize(Roles = "seguridad,admin")]
    [HttpGet("Seguridad")]
    public IActionResult AuthSeguridad()
    {
        var user = HttpContext.User;
        var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
        return Ok($"{string.Join(' ', roles)}");
    }
}
