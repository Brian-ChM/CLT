using Core.Models;
using Infraestructura.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers;

public class AuthController: BaseApiController
{
    [HttpPost("generate-token")] 
    public IActionResult GenerateToken(AuthService service, [FromBody] User user)
    {
        return Ok(service.CreateToken(user));
    }

    [Authorize(Roles = "admin")]
    [HttpGet("Admin")]
    public IActionResult Admin()
    {
        var user = HttpContext.User;
        var roles = user.Claims.Where(c =>  c.Type == ClaimTypes.Role).Select(c => c.Value);
        return Ok($"Claims de role {string.Join(' ', roles)}");
    }

    [Authorize(Roles = "seguridad")]
    [HttpGet("Seguridad")]
    public IActionResult Seguridad()
    {
        var user = HttpContext.User;
        var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
        return Ok($"Claims de role {string.Join(' ', roles)}");
    }

    [Authorize(Roles = "admin,seguridad")]
    [HttpGet("Todos")]
    public IActionResult Todos()
    {
        var user = HttpContext.User;
        var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
        return Ok($"Claims de role {string.Join(' ', roles)}");
    }
}
