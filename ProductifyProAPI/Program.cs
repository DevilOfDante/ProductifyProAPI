using Microsoft.OpenApi.Models;
using ProductifyProAPI.Context;
using ProductifyProAPI.DAL;
using ProductifyProAPI.Helpers;
using ProductifyProAPI.IDAL;


namespace ProductifyProAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            //IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                     builder => builder.AllowAnyOrigin()
                     .AllowAnyMethod()
                     .AllowAnyHeader());
            });

            // configure automapper with all automapper profiles from this assembly
            builder.Services.AddAutoMapper(typeof(Program));

            //var connectionString = "C:\\Users\\mauro\\source\\repos\\ProductifyProAPI\\ProductifyProAPI\\bin\\Debug\\net6.0\\LiteDB.dll\"";
            //builder.Services.AddSingleton<LiteDbContext>(_ => new LiteDbContext(connectionString));
            builder.Services.AddSingleton<LiteDB.LiteDatabase>(_ =>
            {
                // Replace with your connection string or database file path
                return new LiteDB.LiteDatabase("ProductifyPro.db");
            });
            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "ProductifyPro - V1",
                        Version = "v0.9.0"
                    }
                );

                var filePath = Path.Combine(System.AppContext.BaseDirectory, "ProductifyProAPI.xml");
                c.IncludeXmlComments(filePath);
            }
            );

            //All services
            builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            builder.Services.AddScoped<IProductDAL, ProductDAL>();
            builder.Services.AddScoped<ICategoryDAL, CategoryDAL>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors(MyAllowSpecificOrigins);

            app.MapControllers();

            app.Run();
        }
    }
}