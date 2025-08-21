namespace QuanLyNhaHang.Models
{
    public class ApiResponse<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public ApiResponse(int _Code, string _Message, T _Data)
        {
            Code = _Code;
            Message = _Message;
            Data = _Data;
        }
    }
}
