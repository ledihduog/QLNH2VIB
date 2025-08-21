namespace QuanLyNhaHang.Models.QLNHModels
{
    public class UserRole
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public int RoleId { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
