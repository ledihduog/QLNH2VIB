namespace QuanLyNhaHang.ViewModels
{
    public class TokenGenerationRequest
    {
        public int UserId { get; set; }
        public string Code { get; set; }
        public Dictionary<string, object> CustomClaims { get; set; } = new();
    }
}
