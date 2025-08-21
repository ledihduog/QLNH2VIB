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
    //[Authorize(Roles = ADMINISTRATOR)] // comment de test. do phai get token. chi admin moi co per control product
    public class ProductController : ControllerBase
    {
        ProductRepository productRepo = new ProductRepository();

        private readonly IResponseCacheRepository _cach;
        public ProductController(IResponseCacheRepository cach)
        {
            _cach = cach;
        }

        [HttpGet(ApiEndpoints.Product.GetAll)]
        [Cache(100)]
        public async Task<IActionResult> GetAll([FromQuery] List<string>? paramsList, [FromQuery] string? search, [FromQuery] bool import = false, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 100)
        {
            var request = new GetAllResquest
            {
                Params = paramsList?.Select(p => new RequestItem { Value = p }).ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize,
                Search = search
            };
            var data = await productRepo.GetAll(request, import);

            return Ok(new ApiResponse<PagedResponse<Product>>(1, "", new PagedResponse<Product>
            {
                Data = data.lstProduct,
                TotalCount = data.TotalCount
            }));
        }


        [HttpPost(ApiEndpoints.Product.Create)]
        public IActionResult Create([FromBody] Product request)
        {
            try
            {
                _cach.RemoveCacheResponseAsync("/api/Product");
                return Ok(new ApiResponse<string>(productRepo.Create(request), "Thêm thành công", null));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut(ApiEndpoints.Product.Update)]
        public IActionResult Update([FromRoute] int id, [FromBody] Product request)
        {
            try
            {
                request.Id = id;
                Product model = productRepo.GetByID(request.Id);
                MapperHelper.Map(request, model);
                _cach.RemoveCacheResponseAsync("/api/Product");
                return Ok(new ApiResponse<string>(productRepo.Update(model), "Cập nhật thành công", null));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete(ApiEndpoints.Product.Delete)]
        public IActionResult Delete(int id)
        {
            try
            {
                Product model = productRepo.GetByID(id);

                if (model != null)
                {
                    _cach.RemoveCacheResponseAsync("/api/Product");
                    return Ok(new ApiResponse<string>(productRepo.Delete(id), "Xóa thành công", null));
                }
                return BadRequest(new ApiResponse<string>(0, "Xóa thất bại", null));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(ApiEndpoints.Product.DeleteBatch)]
        public IActionResult Delete(List<Product> lstRequest)
        {
            try
            {
                _cach.RemoveCacheResponseAsync("/api/Product");
                return Ok(new ApiResponse<string>(productRepo.Delete(lstRequest), "Xóa danh sách thành công", null));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
