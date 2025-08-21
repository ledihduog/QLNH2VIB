namespace QuanLyNhaHang.ViewModels
{
    public class GetAllResquest
    {
        public List<RequestItem>? Params { get; set; } = null;
        public string? Option { get; set; }

        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 100;
        public string? Search { get; set; }
    }
}
