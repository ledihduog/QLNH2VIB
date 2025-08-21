namespace QuanLyNhaHang.ViewModels
{
    public class UpdateUserRequest
    {
        public string Code { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Grade { get; set; }
        public string? Shift { get; set; }
        public string? Group { get; set; }
        public string? Picture { get; set; }
        public string? Dept { get; set; }
    }
}
