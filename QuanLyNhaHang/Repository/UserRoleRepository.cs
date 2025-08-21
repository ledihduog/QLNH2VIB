using QuanLyNhaHang.Common;
using QuanLyNhaHang.Models.QLNHModels;
using QuanLyNhaHang.ViewModels;
using System.Text;
using static QuanLyNhaHang.Common.Constants;

namespace QuanLyNhaHang.Repository
{
    public class UserRoleRepository : GenericRepository<UserRole>
    {
        public async Task<(IEnumerable<UserRoleVM> lstUseRole, int TotalCount)> GetAll(GetAllResquest request, bool import)
        {
            var queryBuilder = new StringBuilder("SELECT ur.id, us.Code as [UserId], r.Id as [RoleId], us.Lastname, us.Firstname, r.RoleName, ur.CreateBy, ur.CreateDate" +
                " FROM UserRole ur INNER JOIN Role r ON ur.RoleId = r.id INNER JOIN UserLogin us ON ur.UserId = us.Code");
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
                queryBuilder.Append($" us.Code LIKE N'%{request.Search}%' OR us.Firstname LIKE N%'{request.Search}%' OR us.Lastname LIKE N%'{request.Search}%'" +
                    $" OR r.RoleName LIKE N'%{request.Search}%'  OR ur.CreateBy LIKE N'%{request.Search}%'");
            }
            string countQuery = queryBuilder.ToString().Replace("SELECT ur.id, us.Code as [UserId], r.Id as [RoleId], us.Lastname, us.Firstname, r.RoleName, ur.CreateBy, ur.CreateDate FROM UserRole", "SELECT COUNT(*) FROM UserRole");
            int totalCount = await SQLHelper<UserRole>.SqlToScalar<int>(countQuery, SQL_QLNH);
            int offset = (request.PageIndex - 1) * request.PageSize;
            if (import)
            {
                queryBuilder.Append(" ORDER BY ur.id, us.Code ASC");
            }
            else
            {
                queryBuilder.Append($" ORDER BY  ur.id, us.Code DESC OFFSET {offset} ROWS FETCH NEXT {request.PageSize} ROWS ONLY");
            }
            var data = await SQLHelper<UserRoleVM>.SqlToList(queryBuilder.ToString(), SQL_QLNH);
            return (data, totalCount);
        }
    }
}
