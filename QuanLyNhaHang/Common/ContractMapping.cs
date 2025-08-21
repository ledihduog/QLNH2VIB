using QuanLyNhaHang.Models.QLNHModels;
using QuanLyNhaHang.ViewModels;

namespace QuanLyNhaHang.Common
{
    public static class ContractMapping
    {
        public static UserResponse MapToResponse(this UserLogin user)
        {
            return new UserResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                Grade = user.Grade,
                Group = user.Group,
                LastName = user.LastName,
                Shift = user.Shift,
                Code = user.Code,
                Email = user.Email,
                Dept = user.Dept,
                Password = user.Password
            };
        }
        public static UsersResponse MapToResponse(this IEnumerable<UserLogin> users)
        {
            return new UsersResponse
            {
                ListUser = users.Select(MapToResponse)
            };
        }

        public static UserLogin MapToResponse(this SignUpRequest user)
        {
            return new UserLogin
            {
                FirstName = user.Firstname,
                LastName = user.Lastname,
                Password = PasswordHelper.HashPassword(user.Password),
                Picture = null,
                Grade = user.Grade,
                Group = user.Group,
                Shift = user.Shift,
                Code = user.Code,
                Dept = user.Dept,
                Id = user.Id
            };
        }

        public static UserLogin MapToResponse(this UpdateUserRequest user)
        {
            return new UserLogin
            {
                Password = MaHoaMD5.EncryptPassword(user.Password),
                Code = user.Code,
                FirstName = user.Firstname,
                LastName = user.Lastname,
                Grade = user.Grade,
                Email = user.Email,
                Group = user.Group,
                Shift = user.Shift,
                Dept = user.Dept,
            };
        }

        public static UserLogin WithDeptId(this UserLogin options, int Id)
        {
            options.Id = Id;
            return options;
        }
    }
}
