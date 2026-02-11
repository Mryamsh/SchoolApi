using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginDto model)
    {
        var user = _context.Students
            .FirstOrDefault(u =>
                u.Email == model.Email &&
                u.Password == model.Password
            );

        if (user == null)
        {
            return BadRequest(new { message = "Invalid email or password" });
        }

        return Ok(new
        {
            message = "Login successful",
            userId = user.Id,
            email = user.Email
        });
    }
}
