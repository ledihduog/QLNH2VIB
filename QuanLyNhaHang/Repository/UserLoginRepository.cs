using Microsoft.AspNetCore.Identity.Data;
using QuanLyNhaHang.Common;
using QuanLyNhaHang.Models.QLNHModels;
using QuanLyNhaHang.ViewModels;
using System.Linq;
using System.Text;

using static QuanLyNhaHang.Common.Constants;
using LoginRequest = QuanLyNhaHang.ViewModels.LoginRequest;

namespace QuanLyNhaHang.Repository
{
    public class UserLoginRepository : GenericRepository<UserLogin>
    {
        private const string TokenSecret = "ExportingSmilesToTheWorldFromThangLongAssyDiv2";
        private readonly RoleRepository roleRepository = new RoleRepository();
        private readonly UserRoleRepository userRoleRepository = new UserRoleRepository();
        private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(8);

        public async Task<(IEnumerable<UserLogin> lstUser, int TotalCount)> GetAll(GetAllResquest request, bool import)
        {
            var queryBuilder = new StringBuilder("SELECT * FROM UserLogin");
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
                queryBuilder.Append($" Code LIKE N'%{request.Search}%' OR [Group] LIKE N'%{request.Search}%' OR Email LIKE N'%{request.Search}%' OR Dept LIKE N'%{request.Search}%'" +
                    $" OR Grade LIKE N'%{request.Search}%' OR Lastname LIKE N'%{request.Search}%'  OR Firstname LIKE N'%{request.Search}%'");
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
            var data = await SQLHelper<UserLogin>.SqlToList(queryBuilder.ToString(), SQL_QLNH);

            return (data, totalCount);
        }


        public async Task<bool> SignUp(UserLogin model, string code)
        {
            try
            {
                var user = GetAll().FirstOrDefault(x => x.Code == model.Code);
                if (user != null)
                {
                    throw new DataNotFoundException("Mã code người dùng đã tồn tại");
                }
                Create(model);
                var roleId = roleRepository.GetAll().FirstOrDefault(u => u.RoleName == "USER").Id;
                userRoleRepository.Create(new UserRole
                {
                    RoleId = roleId,
                    UserId = model.Id.ToString(),
                    CreateDate = DateTime.Now,
                    CreateBy = code,
                });
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> UpdateUser(UserLogin model)
        {
            try
            {
                var user = GetAll().FirstOrDefault(x => x.Code == model.Code & x.Id != model.Id);
                if (user is not null)
                {
                    return false; // Nếu tài khoản đã tồn tại, trả về false
                }
                var existingUser = GetByID(model.Id);
                MapperHelper.Map(model, existingUser);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> DeleteList(List<UserLogin> lst)
        {
            try
            {
                var lstId = lst.Select(x => x.Id).ToList();
                var lstUserdb = db.UserLogin.Where(u => lstId.Contains(u.Id)).ToList();
                db.UserLogin.RemoveRange(lstUserdb);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
