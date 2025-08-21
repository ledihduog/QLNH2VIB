using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyNhaHang.Attributes;
using QuanLyNhaHang.Common;
using QuanLyNhaHang.IRepository;
using QuanLyNhaHang.Models;
using QuanLyNhaHang.Models.QLNHModels;
using QuanLyNhaHang.Repository;
using QuanLyNhaHang.ViewModels;

namespace QuanLyNhaHang.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
    {
        OrderRepository orderRepo = new OrderRepository();

        private readonly IResponseCacheRepository _cach;
        public OrderController(IResponseCacheRepository cach)
        {
            _cach = cach;
        }

        [HttpGet(ApiEndpoints.Order.GetAll)]
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
            var code = HttpContext.GetCode();
            if (code == null) code = "";
            var data = await orderRepo.GetAll(request, import, code);

            return Ok(new ApiResponse<PagedResponse<Order>>(1, "", new PagedResponse<Order>
            {
                Data = data.lstOrder,
                TotalCount = data.TotalCount
            }));
        }


        [HttpPost(ApiEndpoints.Order.Create)]
        public async Task<IActionResult> Create([FromBody] Order request)
        {
            try
            {
                _cach.RemoveCacheResponseAsync("/api/Order");
                _cach.RemoveCacheResponseAsync("/api/Product");
                await orderRepo.CreateOrderAsync(request);
                return Ok(new ApiResponse<string>(1, "Thêm thành công", null));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut(ApiEndpoints.Order.Update)]
        public IActionResult Update([FromRoute] int id, [FromBody] Order request)
        {
            try
            {
                request.Id = id;
                Order model = orderRepo.GetByID(request.Id);
                MapperHelper.Map(request, model);
                _cach.RemoveCacheResponseAsync("/api/Order");
                return Ok(new ApiResponse<string>(orderRepo.Update(model), "Cập nhật thành công", null));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete(ApiEndpoints.Order.Delete)]
        public IActionResult Delete(int id)
        {
            try
            {
                Order model = orderRepo.GetByID(id);

                if (model != null)
                {
                    _cach.RemoveCacheResponseAsync("/api/Order");
                    return Ok(new ApiResponse<string>(orderRepo.Delete(id), "Xóa thành công", null));
                }
                return BadRequest(new ApiResponse<string>(0, "Xóa thất bại", null));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(ApiEndpoints.Order.DeleteBatch)]
        public IActionResult Delete(List<Order> lstRequest)
        {
            try
            {
                _cach.RemoveCacheResponseAsync("/api/Order");
                return Ok(new ApiResponse<string>(orderRepo.Delete(lstRequest), "Xóa danh sách thành công", null));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
