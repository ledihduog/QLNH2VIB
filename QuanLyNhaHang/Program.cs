//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();

using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using QuanLyNhaHang.Services;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddCustomServices(builder.Configuration);

    // cau hinh chinh sach gioi han Rate Limiting toan cuc
    builder.Services.AddRateLimiter(option =>
    {
        option.AddSlidingWindowLimiter("global", opt =>
        {
            opt.PermitLimit = 50;
            opt.Window = TimeSpan.FromMinutes(1);
            opt.SegmentsPerWindow = 4;
            opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            opt.QueueLimit = 0;
        });
    });

    //builder.Logging.ClearProviders();

    var app = builder.Build();
    AppService.Service = app.Services;
    app.UseRateLimiter();
    
    app.UseCors("AllowSpecificOrigins"); // cau hinh CORS cho angular
    //app.UseHttpsRedirection();
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseAuthentication();
    app.UseAuthorization();
    //app.UseMiddleware<ValidationMappingMiddleware>();
    app.MapControllers();
    app.MapControllers().RequireRateLimiting("global");

    app.Run();

}
catch { }
