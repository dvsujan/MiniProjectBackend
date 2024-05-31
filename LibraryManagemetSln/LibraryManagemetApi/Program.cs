
using LibraryManagemetApi.Contexts;
using LibraryManagemetApi.Interfaces;
using LibraryManagemetApi.Models;
using LibraryManagemetApi.Repositories;
using LibraryManagemetApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace LibraryManagemetApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddLogging(l => l.AddLog4Net());

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
              {
                  options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                  {
                      ValidateIssuer = false,
                      ValidateAudience = false,
                      ValidateIssuerSigningKey = true,
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey:JWT"]))
                  };
              });

            builder.Services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    new string[] { }
                }
            });
            });

            #region database_connection
            builder.Services.AddDbContext<LibraryManagementContext>(options =>
            {
                options.UseSqlServer("Data Source=794GBX3\\INSTANCE_1;Integrated Security=true;Initial Catalog=LibraryManagementApi; TrustServerCertificate=True"); 
            });
            #endregion

            #region repositories
            builder.Services.AddScoped<IRepository<int , Book>, BookRepository>();
            builder.Services.AddScoped<IRepository<int, User>, UserRepository>();
            builder.Services.AddScoped<IRepository<int, Role>, RoleRepository>();
            builder.Services.AddScoped<IRepository<int, Author>, AuthorRepository>();
            builder.Services.AddScoped<IRepository<int, Category>, CategoryRepository>();
            builder.Services.AddScoped<IRepository<int, Publisher>, PublisherRepository>();
            builder.Services.AddScoped<IRepository<int, Publisher>, PublisherRepository>();
            builder.Services.AddScoped<IRepository<int, Review>, ReviewRepository>();
            builder.Services.AddScoped<IRepository<int, Stock>, StockRepository>();
            builder.Services.AddScoped<IRepository<int, Location>, LocationRepository>();
            builder.Services.AddScoped<IRepository<int, Borrowed>, BorrowedRepository>();
            builder.Services.AddScoped<IRepository<int, Reservation>, ReservationRepository>();
            builder.Services.AddScoped<IRepository<int, Payment>, PaymentRepository>();
            builder.Services.AddScoped<IRepository<int, Card>, CardRepository>();
            #endregion

            #region services
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IBookService, BookService>();
            builder.Services.AddScoped<IborrowService, BorrowService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();
            builder.Services.AddScoped<IReservationService, ReservationService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IAnalyticsService, AnaylticsService>();
            #endregion

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
