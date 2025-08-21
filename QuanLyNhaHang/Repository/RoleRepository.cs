using QuanLyNhaHang.Common;
using QuanLyNhaHang.Models.QLNHModels;
using QuanLyNhaHang.ViewModels;
using System.Text;
using static QuanLyNhaHang.Common.Constants;

namespace QuanLyNhaHang.Repository
{
    public class RoleRepository : GenericRepository<Role>
    {
        public async Task<(IEnumerable<Role> lstRole, int TotalCount)> GetAll(GetAllResquest request, bool import)
        {
            var queryBuilder = new StringBuilder("SELECT * FROM Role");
            if (request.Params?.Any() == true)
            {
                var conditions = request.Params
                    .Where(u => !string.IsNullOrEmpty(u.Value))
                    .Select(u => $"{u.Value}")
                    .ToList();
                if (conditions.Any())
                {
                    queryBuilder.Append(" WHERE ");
                    queryBuilder.Append(string.Join(" AND ", conditions));
                }
            }
            if (request.Search is not null && request.Search.Length > 0)
            {
                if (queryBuilder.ToString().Contains("WHERE"))
                {
                    queryBuilder.Append(" AND ");
                }
                else
                {
                    queryBuilder.Append(" WHERE ");
                }
                queryBuilder.Append($" RoleName LIKE N'%{request.Search}%'");
            }
            string countQuery = queryBuilder.ToString().Replace("SELECT * FROM", "SELECT COUNT(*) FROM");
            int totalCount = await SQLHelper<Role>.SqlToScalar<int>(countQuery, SQL_QLNH);
            int offset = (request.PageIndex - 1) * request.PageSize;
            if (import)
            {
                queryBuilder.Append(" ORDER BY ID ASC");
            }
            else
            {
                queryBuilder.Append($" ORDER BY ID DESC OFFSET {offset} ROWS FETCH NEXT {request.PageSize} ROWS ONLY");
            }
            var data = await SQLHelper<Role>.SqlToList(queryBuilder.ToString(), SQL_QLNH);
            return (data, totalCount);
        }
    }
}
