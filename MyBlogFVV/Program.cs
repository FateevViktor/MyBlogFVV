using Microsoft.AspNetCore.Authentication.Cookies;
using MyBlogFVV.BLL.Interfaces;
using MyBlogFVV.BLL.Services;
using MyBlogFVV.DAL.Interfaces;
using MyBlogFVV.DAL.Repositories;
using System.Security.Claims;
namespace MyBlogFVV
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpContextAccessor();

            // аутентификация с помощью куки
            //подключаем аутентификацию со схемой Cookies
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/User/Login";
                    options.Cookie.Name = "authCookie";
                    options.AccessDeniedPath = "/AccessDanied"; //путь к странице с информацией о запрете доступа
                });
            //подключаем серсив авторизации
            builder.Services.AddAuthorization(opts => {

                opts.AddPolicy("OnlyForRoleAdmin", policy => {
                    policy.RequireClaim(ClaimTypes.Role, "Admin");
                });
                opts.AddPolicy("OnlyForRoleUser", policy => {
                    policy.RequireClaim(ClaimTypes.Role, "User");
                });
                opts.AddPolicy("OnlyForRoleModerator", policy => {
                    policy.RequireClaim(ClaimTypes.Role, "Moderator");
                });
            });

            //var connection = builder.Configuration.GetConnectionString("DefaultConnection");
            //Контекст БД
            //builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connection));
            //Внедряем зависимости
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<ICommentService, CommentService>();
            builder.Services.AddScoped<ITagService, TagService>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();
            app.UseAuthentication();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
