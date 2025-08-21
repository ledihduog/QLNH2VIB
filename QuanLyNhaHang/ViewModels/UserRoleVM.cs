namespace QuanLyNhaHang.ViewModels
{
    public class UserRoleVM
    {
        public int id { get; set; }
        public int RoleId { get; set; }
        public string UserId { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string RoleName { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public bool? disabled { get; set; }
        public bool? Checked { get; set; } = false;
    }
}
