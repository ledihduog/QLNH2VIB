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
    [Authorize(Roles = ADMINISTRATOR)]

    public class UserRoleController : ControllerBase
    {
        UserRoleRepository userRoleRepo = new UserRoleRepository();

        private readonly IResponseCacheRepository cach;
        public UserRoleController(IResponseCacheRepository cach)
        {
            this.cach = cach;
        }

        [HttpGet(ApiEndpoints.UserRole.GetAll)]
        [Cache(100)]
        public async Task<IActionResult> GetAll(
         [FromQuery] List<string>? paramsList,
         [FromQuery] string? dept, [FromQuery] string? search, [FromQuery] bool import = false, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 100)
        {
            var request = new GetAllResquest
            {
                Params = paramsList?.Select(p => new RequestItem { Value = p }).ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize,
                Search = search
            };
            var data = await userRoleRepo.GetAll(request, import);

            return Ok(new ApiResponse<PagedResponse<UserRoleVM>>(1, "", new PagedResponse<UserRoleVM>
            {
                Data = data.lstUseRole,
                TotalCount = data.TotalCount
            }));
        }


        [HttpPost(ApiEndpoints.UserRole.Create)]
        public IActionResult Create([FromBody] UserRole request)
        {
            try
            {
                cach.RemoveCacheResponseAsync("/api/UserRole");
                request.CreateDate = DateTime.Now;
                request.CreateBy = HttpContext.GetCode();
                return Ok(new ApiResponse<string>(userRoleRepo.Create(request), "Thêm thành công", null));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut(ApiEndpoints.UserRole.Update)]
        public IActionResult Update([FromRoute] int id, [FromBody] UserRole request)
        {
            try
            {
                var userRoleDelete = userRoleRepo.GetAll().Where(u => u.Id == id).ToList();
                userRoleRepo.Delete(userRoleDelete);
                request.Id = 0;
                request.CreateDate = DateTime.Now;
                request.CreateBy = HttpContext.GetCode();
                cach.RemoveCacheResponseAsync("/api/UserRole");
                return Ok(new ApiResponse<string>(userRoleRepo.Create(request), "Cập nhật thành công", null));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        [HttpDelete(ApiEndpoints.UserRole.Delete)]
        public IActionResult Delete(int id)
        {
            try
            {
                UserRole model = userRoleRepo.GetByID(id);

                if (model != null)
                {
                    cach.RemoveCacheResponseAsync("/api/UserRole");
                    return Ok(new ApiResponse<string>(userRoleRepo.Delete(id), "Xóa thành công", null));
                }
                return BadRequest(new ApiResponse<string>(0, "User role không tồn tại", null));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(ApiEndpoints.UserRole.DeleteBatch)]
        public IActionResult Delete(List<UserRole> lstRequest)
        {
            try
            {
                cach.RemoveCacheResponseAsync("/api/UserRole");
                var lstId = lstRequest.Select(u => u.Id);
                var lstDelete = userRoleRepo.GetAll().Where(u => lstId.Contains(u.Id));
                userRoleRepo.RemoveRange(lstDelete);
                return Ok(new ApiResponse<string>(1, "Xóa danh sách thành công", null));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
