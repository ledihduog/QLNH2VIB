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
    public class SumaryController : ControllerBase
    {
        SumaryRepository sumaryRepo = new SumaryRepository();

        private readonly IResponseCacheRepository _cach;
        public SumaryController(IResponseCacheRepository cach)
        {
            _cach = cach;
        }

        [HttpGet(ApiEndpoints.Sumary.GetAll)]
        [Cache(100)]
        public async Task<IActionResult> GetAll([FromQuery] string fromdate, [FromQuery] string todate, [FromQuery] List<string>? paramsList, [FromQuery] string? search, [FromQuery] bool import = false, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 100)
        {
            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            }
            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            }
            var request = new GetAllResquest
            {
                Params = paramsList?.Select(p => new RequestItem { Value = p }).ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize,
                Search = search
            };
            var data = await sumaryRepo.GetAll(request, import, fromdate, todate);

            return Ok(new ApiResponse<PagedResponse<Order>>(1, "", new PagedResponse<Order>
            {
                Data = data.lstOrder,
                TotalCount = data.TotalCount
            }));
        }







    }
}
