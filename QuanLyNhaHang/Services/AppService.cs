namespace QuanLyNhaHang.Services
{
    public static class AppService
    {
        public static IServiceProvider Service { get; set; }
        public static IConfiguration Configuration => Service.GetService<IConfiguration>();
    }
}
