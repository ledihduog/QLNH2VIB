using static QuanLyNhaHang.ApiEndpoints;

namespace QuanLyNhaHang.Models.QLNHModels
{
    public class Order
    {
        public int Id { get; set; }
        public int UserLoginCode { get; set; }
        public int ProductId { get; set; }
        public int Qty { get; set; }
        public DateTime Entry { get; set; } = DateTime.UtcNow;
    }
}
