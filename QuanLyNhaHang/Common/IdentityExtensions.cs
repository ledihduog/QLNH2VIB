using System.Security.Claims;

namespace QuanLyNhaHang.Common
{
    public static class IdentityExtensions
    {
        public static string? GetCode(this HttpContext context)
        {
            var userId = context.User.Claims.SingleOrDefault(x => x.Type == "code")?.Value;
            return userId;
        }

    }
}
