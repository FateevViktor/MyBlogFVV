using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using MyBlogFVV.BLL.Interfaces;
using MyBlogFVV.BLL.Services;
using MyBlogFVV.DAL.EF;
using MyBlogFVV.DAL.Interfaces;
using MyBlogFVV.DAL.Repositories;
using NLog.Web;
using System.Security.Claims;

namespace MyBlogFVV.WEB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //--------Подключаю NLog--------------------
            // Remove default Microsoft logging providers
            builder.Logging.ClearProviders();
            // Register NLog
            builder.Host.UseNLog();
            //------------------------------------------

            //Настройки подключения к БД
            var connection = builder.Configuration.GetConnectionString("DefaultConnection");
            //Контекст БД
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connection));

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
            builder.Services.AddScoped<IRoleService, RoleService>();


            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            /*
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                //app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            */
            //Задал культуру
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("ru-RU"), // Явная культура
            });
            //перенаправим если страницу не нашли
            app.UseStatusCodePagesWithRedirects("NotFoundContent");

            app.UseAuthentication();            

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
