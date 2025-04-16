using Meds.Server.Models.DbModels;
using Meds.Server.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Meds.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
                });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp", policy =>
                {
                    policy.WithOrigins("https://localhost:4200") // Replace with Angular app's URL
                          .AllowCredentials()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                    //policy.AllowAnyOrigin()
                    //      .AllowCredentials()
                    //      .AllowAnyHeader()
                    //      .AllowAnyMethod();
                });
            });


            builder.Services.AddDbContext<Wv1Context>(options =>
            options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
            new MySqlServerVersion(new Version(8, 0, 26))));

            builder.Services.AddAuthentication("CookieAuth").AddCookie("CookieAuth", config =>
            {
                config.Cookie.Name = "UserLoginCookie";
                config.Cookie.HttpOnly = true;
                config.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                config.Cookie.SameSite = SameSiteMode.Lax;
                config.ExpireTimeSpan = TimeSpan.FromHours(1);
                config.LoginPath = "/Login";
            });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Patient", policy => policy.RequireRole("patient"));
                options.AddPolicy("Receptionist", policy => policy.RequireRole("receptionist"));
                options.AddPolicy("LabWorker", policy => policy.RequireRole("lab_worker"));
                options.AddPolicy("Admin", policy => policy.RequireRole("admin"));
            });
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<PatientsService>();
            builder.Services.AddScoped<TestTypesService>();
            builder.Services.AddScoped<StatisticsService>();
            builder.Services.AddScoped<MailService>();
            builder.Services.AddSingleton<ActivityLoggerService>();
            builder.Services.AddHttpContextAccessor();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.MapType<DateOnly>(() => new OpenApiSchema { Type = "string", Format = "date" });
            });
            
            var app = builder.Build();
            app.UseCors("AllowAngularApp");

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            app.Run();
        }
    }
}
