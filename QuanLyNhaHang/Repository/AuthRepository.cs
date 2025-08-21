using Microsoft.IdentityModel.Tokens;
using QuanLyNhaHang.Common;
using QuanLyNhaHang.Models.QLNHModels;
using QuanLyNhaHang.Services;
using QuanLyNhaHang.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuanLyNhaHang.Repository
{
    public class AuthRepository : GenericRepository<UserLogin>
    {
        RoleRepository roleRepo = new RoleRepository();
        UserRoleRepository userRoleRepo = new UserRoleRepository();


        public static IConfiguration _config;

        public AuthRepository()
        {
            _config = AppService.Configuration;
        }

        public string GenerateAccessToken(UserLogin user)
        {
            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim("code", user.Code),
                new Claim("email", user.Email ?? ""),
                new Claim("lastName", user.LastName ?? ""),
                new Claim("firstName", user.FirstName ?? ""),
                new Claim("fullName", user.LastName +" " +user.FirstName),
                new Claim("grade", user.Grade),
                new Claim("shift", user.Shift ?? ""),
                new Claim("group", user.Group ?? ""),
                new Claim("image", user.Picture ?? ""),
                new Claim("dept", user.Dept ?? ""),
            };

            var lstUserRole = userRoleRepo.GetAll().Where(u => u.UserId == user.Code).Select(x => x.RoleId);
            var lstRole = roleRepo.GetAll().Where(u => lstUserRole.Contains(u.Id)).Select(u => u.RoleName).ToList();
            lstRole.ForEach(role => claims.Add(new Claim("roles", role)));
            var accessToken = GenerateWebToken(claims.ToList());

            return accessToken;
        }

        public string GenerateWebToken(List<Claim> claims)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            double expireHours = Convert.ToDouble(_config["Jwt:ExpireDay"]);
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                expires: DateTime.Now.AddDays(expireHours),
                signingCredentials: credentials,
                claims: claims.ToArray()
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public string SignIn(LoginRequest request)
        {
            var userLogin = GetAll().FirstOrDefault(u => u.Code == request.Code);

            if (userLogin == null)
            {
                throw new DataNotFoundException("Mã nhân viên không chính xác!");
            }
            if (!PasswordHelper.VeryfyPassword(request.Password, userLogin.Password))
            {
                throw new DataNotFoundException("Mật khẩu không chính xác!");
            }

            return GenerateAccessToken(userLogin);
        }


        public async Task<string> ChangePassword(string code, string currentPassword, string newPassword)
        {
            var user = GetAll().FirstOrDefault(u => u.Code == code);
            if (user == null) throw new DataNotFoundException("User not found");
            if (!PasswordHelper.VeryfyPassword(currentPassword, user.Password))
            {
                throw new Exception("Mật khẩu cũ không chính xác");
            }
            user.Password = PasswordHelper.HashPassword(newPassword);
            Update(user);
            var token = GenerateAccessToken(user);
            return token;
        }


    }
}
