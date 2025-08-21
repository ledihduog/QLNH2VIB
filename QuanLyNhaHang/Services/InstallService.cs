using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using QuanLyNhaHang.Common;
using QuanLyNhaHang.Configuration;
using QuanLyNhaHang.IRepository;
using QuanLyNhaHang.Models.Context;
using QuanLyNhaHang.Repository;
using StackExchange.Redis;
using System.Text;


namespace QuanLyNhaHang.Services
{
    public static class InstallService
    {
        public static void AddCustomServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllers();
            services.AddDbContext<QLNHContext>(option => option.UseSqlServer(Constants.SQL_QLNH));

            services.AddEndpointsApiExplorer();
            //services.AddSwaggerGen();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Nhập 'Bearer {token}' vào đây."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            services.AddHttpClient();



            var redisCongifuration = new RedisConfiguration();
            config.GetSection(nameof(RedisConfiguration)).Bind(redisCongifuration);
            services.AddSingleton(redisCongifuration);
            if (!redisCongifuration.Enabled)
            {
                return;
            }
            services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisCongifuration.ConnectionString));
            services.AddStackExchangeRedisCache(option =>
            {
                option.Configuration = redisCongifuration.ConnectionString;
            });
            services.AddSingleton<IResponseCacheRepository, ResponseCacheRepository>();

            //var redisCongifuration = new RedisConfiguration();
            //config.GetSection(nameof(RedisConfiguration)).Bind(redisCongifuration);
            //services.AddSingleton(redisCongifuration);
            //if (redisCongifuration.Enabled)
            //{
            //    services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisCongifuration.ConnectionString));
            //    services.AddStackExchangeRedisCache(option =>
            //    {
            //        option.Configuration = redisCongifuration.ConnectionString;
            //    });
            //    services.AddSingleton<IResponseCacheRepository, ResponseCacheRepository>();
            //}
            //else
            //{
            //    services.AddSingleton<IResponseCacheRepository, DummyCacheRepository>();
            //}

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins",
                    builder =>
                    {
                        builder.AllowAnyHeader()
                               .AllowAnyMethod()
                               .WithOrigins("http://localhost:4200", "http://localhost:44321")
                               .AllowCredentials();
                    });
            });

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = config["Jwt:Audience"],
                    ValidIssuer = config["Jwt:Issuer"],
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]))
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("Authentication Failed: " + context.Exception.Message);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine("Token validated successfully");
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization();




        }
    }
}
