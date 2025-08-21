using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyNhaHang.Common;
using QuanLyNhaHang.IRepository;
using QuanLyNhaHang.Models;
using QuanLyNhaHang.Repository;
using QuanLyNhaHang.ViewModels;

namespace QuanLyNhaHang.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly UserLoginRepository userRepo = new UserLoginRepository();

        private readonly AuthRepository authRepo = new AuthRepository();
        private readonly IResponseCacheRepository cach;
        public AuthController(IResponseCacheRepository cach)
        {
            this.cach = cach;
        }

        [HttpPost(ApiEndpoints.Auth.SignIn)]
        public async Task<IActionResult> SignIn([FromBody] LoginRequest request)
        {
            try
            {
                var pass = PasswordHelper.HashPassword(request.Password);
                return Ok(new ApiResponse<string>(1, "", authRepo.SignIn(request)));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut(ApiEndpoints.Auth.ChangPass)]
        public async Task<IActionResult> ChangPass([FromBody] ChangePassRequest request)
        {
            try
            {
                var code = HttpContext.GetCode();
                var user = userRepo.GetAll().FirstOrDefault(u => u.Code == code);
                var token = await authRepo.ChangePassword(code, request.CurrentPassword, request.Password);

                return Ok(new ApiResponse<string>(1, "", token));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
