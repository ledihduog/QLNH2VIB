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
    //[Authorize(Roles = ADMINISTRATOR)] comment for test
    public class RoleController : ControllerBase
    {
        RoleRepository roleRepo = new RoleRepository();
        UserRoleRepository userRoleRepo = new UserRoleRepository();

        private readonly IResponseCacheRepository _cach;
        public RoleController(IResponseCacheRepository cach)
        {
            _cach = cach;
        }

        [HttpGet(ApiEndpoints.Role.GetAll)]
        [Cache(100)]
        public async Task<IActionResult> GetAll([FromQuery] List<string>? paramsList, [FromQuery] string? dept, [FromQuery] string? search, [FromQuery] bool import = false, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 100)
        {
            var request = new GetAllResquest
            {
                Params = paramsList?.Select(p => new RequestItem { Value = p }).ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize,
                Search = search
            };
            var data = await roleRepo.GetAll(request, import);
            
            return Ok(new ApiResponse<PagedResponse<Role>>(1, "", new PagedResponse<Role>
            {
                Data = data.lstRole,
                TotalCount = data.TotalCount
            }));
        }


        [HttpPost(ApiEndpoints.Role.Create)]
        public IActionResult Create([FromBody] Role request)
        {
            try
            {
                _cach.RemoveCacheResponseAsync("/api/Role");
                return Ok(new ApiResponse<string>(roleRepo.Create(request), "Thêm thành công", null));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut(ApiEndpoints.Role.Update)]
        public IActionResult Update([FromRoute] int id, [FromBody] Role request)
        {
            try
            {
                request.Id = id;
                Role model = roleRepo.GetByID(request.Id);
                MapperHelper.Map(request, model);
                _cach.RemoveCacheResponseAsync("/api/Role");
                return Ok(new ApiResponse<string>(roleRepo.Update(model), "Cập nhật thành công", null));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete(ApiEndpoints.Role.Delete)]
        public IActionResult Delete(int id)
        {
            try
            {
                Role model = roleRepo.GetByID(id);

                if (model != null)
                {
                    _cach.RemoveCacheResponseAsync("/api/Role");
                    return Ok(new ApiResponse<string>(roleRepo.Delete(id), "Xóa thành công", null));
                }
                return BadRequest(new ApiResponse<string>(0, "Xóa thất bại", null));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(ApiEndpoints.Role.DeleteBatch)]
        public IActionResult Delete(List<Role> lstRequest)
        {
            try
            {
                _cach.RemoveCacheResponseAsync("/api/Role");
                return Ok(new ApiResponse<string>(roleRepo.Delete(lstRequest), "Xóa danh sách thành công", null));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
