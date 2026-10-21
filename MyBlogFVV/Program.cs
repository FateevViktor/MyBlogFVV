using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
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
            //Добавляем сваггер
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MyBlog API",
                    Version = "v1",
                    Description = "В данном API организованы основные функции на все контроллеры",
                    Contact = new OpenApiContact
                    {
                        Name = "Фатеев Виктор Вячеславович",
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Лицензии нет",
                    }
                });
                //настраиваем Swagger на чтение XML-комментариев
                //var xmlPath = Path.Combine(Environment.CurrentDirectory, "documentation.xml");
                var xmlFilename = "documentation.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                options.IncludeXmlComments(Path.Combine("../MyBlogFVV.API", xmlFilename));
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                /*
                app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;
                });
                */
                //app.UseDeveloperExceptionPage();
                
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();                
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
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
