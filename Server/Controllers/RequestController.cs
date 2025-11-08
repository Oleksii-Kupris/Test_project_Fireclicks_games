using Microsoft.AspNetCore.Mvc;
using Server.Services;

[ApiController]
[Route("api/request")]
public class RequestController : ControllerBase
{
    private readonly TokenService _tokenService;

    public RequestController(TokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("send")]
    public IActionResult Send([FromBody] TokenDto dto)
    {
        if (dto == null || string.IsNullOrEmpty(dto.EncryptedToken))
            return BadRequest(new { error = "Missing token" });

        var userId = _tokenService.DecryptToken(dto.EncryptedToken);
        if (userId == null)
            return Unauthorized(new { error = "Invalid token" });

        int count = _tokenService.RegisterRequest(userId);
        return Ok(new { requestCount = count });
    }

    public class TokenDto
    {
        public string EncryptedToken { get; set; } = string.Empty;
    }
}
