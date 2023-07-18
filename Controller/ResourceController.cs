using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SignalRWebpack.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResourceController : ControllerBase
    {
        [HttpGet("protected-resource")]
        [Authorize] // Áp dụng xác thực Access Token
        public IActionResult GetProtectedResource()
        {
            // Xác thực thành công, trả về tài nguyên bảo vệ
            return Ok("Protected resource");
        }
    }
}
