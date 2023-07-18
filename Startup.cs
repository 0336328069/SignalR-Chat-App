//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.AspNetCore.SignalR;
//using Microsoft.AspNetCore.SpaServices;
//using SignalRWebpack.Hubs;

//namespace SignalRChatApp
//{
//    public class Startup
//    {
//        public Startup(IConfiguration configuration)
//        {
//            Configuration = configuration;
//        }

//        public IConfiguration Configuration { get; }

//        public void ConfigureServices(IServiceCollection services)
//        {
//            services.AddSpaStaticFiles(configuration =>
//            {
//                configuration.RootPath = "ClientApp/dist";
//            });
//            services.AddCors(options =>
//            {
//                options.AddPolicy("AllowVueApp", builder =>
//                {
//                    builder.WithOrigins("http://localhost:3000")
//                       .AllowAnyMethod()
//                       .AllowAnyHeader()
//                       .AllowCredentials();
//                });
//            });
//            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//             .AddJwtBearer(options =>
//             {
//                 options.TokenValidationParameters = new TokenValidationParameters
//                 {
//                     ValidateIssuer = true,
//                     ValidateAudience = true,
//                     ValidateIssuerSigningKey = true,
//                     ValidIssuer = "MGI",
//                     ValidAudience = "chat-api",
//                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key"))
//                 };
//             });
//            services.AddSignalR();
//            services.AddScoped<UserTracker>();
//            // Cấu hình và đăng ký các dịch vụ
//        }

//        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//        {
//            if (env.IsDevelopment())
//            {
//                app.UseDeveloperExceptionPage();
//            }

//            app.UseRouting();
//            app.UseStaticFiles();
//            app.UseSpaStaticFiles();
//            app.UseAuthorization();
//            app.UseAuthentication();
//            app.UseCors("AllowVueApp");
//            app.UseEndpoints(endpoints =>
//            {
//                endpoints.MapHub<ChatHub>("/chathub"); // Đăng ký Hub của bạn
//                                                       // Cấu hình các định tuyến và điều khiển truy cập vào Hub khác (nếu cần)
//            });
//            // Cấu hình pipeline xử lý HTTP request
//        }
//    }
//}