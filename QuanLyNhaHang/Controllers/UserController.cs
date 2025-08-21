using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyNhaHang.Attributes;
using QuanLyNhaHang.Common;
using QuanLyNhaHang.IRepository;
using QuanLyNhaHang.Models;
using QuanLyNhaHang.Models.QLNHModels;
using QuanLyNhaHang.Repository;
using QuanLyNhaHang.ViewModels;
using static QuanLyNhaHang.Common.Constants;

namespace QuanLyNhaHang.Controllers
{
    [ApiController]
    //[Authorize(Roles = ADMINISTRATOR)]
    public class UserController : ControllerBase
    {

        private readonly UserLoginRepository userRepo = new UserLoginRepository();
        private readonly IResponseCacheRepository cach;
        public UserController(IResponseCacheRepository cach)
        {
            this.cach = cach;
        }

        [HttpGet(ApiEndpoints.UserLogin.GetAll)]
        [Cache(100)]
        public async Task<IActionResult> GetAll( [FromQuery] List<string>? paramsList, [FromQuery] string? status, [FromQuery] string? search, [FromQuery] bool import, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 100)
        {
            var request = new GetAllResquest
            {
                Params = paramsList?.Select(p => new RequestItem { Value = p }).ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize,
                Search = search
            };

            var (data, totalCount) = await userRepo.GetAll(request, import);

            var usersResponse = data.MapToResponse().ListUser;

            var response = new ApiResponse<PagedResponse<UserResponse>>(1, "", new PagedResponse<UserResponse>
            {
                Data = usersResponse,
                TotalCount = totalCount
            }
            );
            return Ok(response);
        }


        [HttpPost(ApiEndpoints.UserLogin.Create)]
        public async Task<IActionResult> Create([FromBody] SignUpRequest request)
        {
            try
            {
                var model = request.MapToResponse();
                var code = HttpContext.GetCode();
                var userResigter = await userRepo.SignUp(model, code);
                await cach.RemoveCacheResponseAsync("/api/User");
                var response = new ApiResponse<IEnumerable<UserResponse>>(1, "Đăng ký thành công", null);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut(ApiEndpoints.UserLogin.Update)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserRequest request)
        {
            var model = request.MapToResponse().WithDeptId(id);
            bool usersResponse = await userRepo.UpdateUser(model);
            if (usersResponse)
            {
                await cach.RemoveCacheResponseAsync("/api/User");
                var response = new ApiResponse<IEnumerable<UserResponse>>(1, "Cập nhật thành công", null);
                return Ok(response);
            }
            else
            {
                return BadRequest("Cập nhật thất bại");
            }
        }


        [HttpDelete(ApiEndpoints.UserLogin.Delete)]

        public IActionResult Delete(int id)
        {
            try
            {
                UserLogin model = userRepo.GetByID(id);

                if (model != null)
                {
                    cach.RemoveCacheResponseAsync("/api/UserLogin");
                    return Ok(new ApiResponse<string>(userRepo.Delete(id), "Xóa thành công", null));
                }
                return BadRequest(new ApiResponse<string>(0, "Xóa thất bại", null));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPost(ApiEndpoints.UserLogin.DeleteBatch)]
        public IActionResult Delete(List<UserLogin> lstRequest)
        {
            try
            {
                cach.RemoveCacheResponseAsync("/api/UserLogin");
                return Ok(new ApiResponse<string>(userRepo.Delete(lstRequest), "Xóa danh sách thành công", null));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
