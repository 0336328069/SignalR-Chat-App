using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.SignalR;
using SignalRWebpack.Data;
using SignalRWebpack.Hubs;
using SignalRWebpack.Modal;
using SignalRWebpack.ViewModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SignalRWebpack.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHubContext<ChatHub> _hubContext;

        public AuthController(ApplicationDbContext dbContext, IHubContext<ChatHub> hubContext)
        {
            _dbContext = dbContext;
            _hubContext = hubContext;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginViewModel model)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == model.Username);
            if (user == null)
            {
                return BadRequest("Invalid username or password");
            }

            if (model.Password != user.Password)
            {
                return BadRequest("Invalid username or password");
            }

            _hubContext.Clients.All.SendAsync("UserLoggedIn", user.Username);
            // Tạo mã thông báo truy cập (Access Token)
            var accessToken = GenerateAccessToken(user);

            // Trả về Access Token cho người dùng
            return Ok(new { AccessToken = accessToken });
        }

        private string GenerateAccessToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                // Các claim khác nếu cần thiết
            };

            // Khóa bí mật (Secret Key) để ký và giải mã Access Token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Kl23CW97H0u-LUPCjQgZMQ"));

            // Tạo Signing Credentials từ khóa bí mật
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tạo Access Token
            var token = new JwtSecurityToken(
                issuer: "MGI",
                audience: "chat-api",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), // Thời hạn của Access Token
                signingCredentials: creds
            );

            // Trả về chuỗi mã hóa của Access Token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
