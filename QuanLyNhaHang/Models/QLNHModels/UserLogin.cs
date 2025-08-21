namespace QuanLyNhaHang.Models.QLNHModels
{
    public class UserLogin
    {
        public int Id { get; set; }
        public string? IDCard { get; set; }
        public string Code { get; set; }
        public string Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Grade { get; set; }
        public string? Shift { get; set; }
        public string? Group { get; set; }
        public string? Picture { get; set; }
        public string? Dept { get; set; }
        public string? Email { get; set; }
    }
}
