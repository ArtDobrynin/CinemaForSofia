using Capy.Common.Interfaces;
using Capy.Common.Options;
using Capy.Common.Services;
using Capy.Domain.Models.Auth;
using CapyAuth.Application.Handlers.Commands.LoginUser;
using CapyAuth.Application.Handlers.Commands.RegisterUser;
using CapyAuth.Infrastructure;
using CapyAuth.Infrastructure.Interfaces;
using CapyFilms.Application.Handlers.Commands.RandomizerFilms;
using CapyFilms.Application.Handlers.Queries.GetCinemaByGenre;
using CapyFilms.Application.Handlers.Queries.GetNewCinema;
using CapyFilms.Application.Handlers.Queries.GetSearchCinema;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace CapyFilms.Api
{
    public class Startup
    {
        private readonly IConfiguration Configuration;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            var credentials = Configuration.GetSection("SecurityCredentials");
            var kinopoisk = Configuration.GetSection("KinopoiskCredentials");
            var credentialsJwt = new SecurityCredentials();

            services.Configure<PasswordHasherOptions>(opt => opt.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3);
            services.Configure<SecurityCredentials>(credentials);
            services.Configure<KinopoiskCredentials>(kinopoisk);
            Configuration.Bind("SecurityCredentials", credentialsJwt);
            services.Configure<PathToValidateText>(Configuration.GetSection("PathToValidateText"));

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IValidator<RegisterUserCommand>, RegisterUserCommandValidator>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            services.AddControllers();

            services.AddDbContext<ICapyDbContext, CapyDbContext>(options => options.UseNpgsql(connectionString));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommand).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(LoginUserCommand).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(GetNewFilmsQuery).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(GetCinemaByGenreQuery).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(GetSearchCinemaQuery).Assembly);
            });

            services.AddHttpClient<GetNewFilmsQueryHandler>();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = credentialsJwt.Issuer,
                            ValidateAudience = true,
                            ValidAudience = credentialsJwt.Audience,
                            ValidateLifetime = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                                               .GetBytes(credentialsJwt.SecurityKey)),
                            ValidateIssuerSigningKey = true,
                        };
                    });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost",
                    builder => builder
                        .WithOrigins("http://localhost:3002") 
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            services.AddAuthorization();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseSwaggerUI(options => 
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
      
            app.UseRouting();
            app.UseCors("AllowLocalhost");
            app.UseAuthentication(); 
            app.UseAuthorization();

            app.UseEndpoints(endpoint =>
            {
                endpoint.MapControllers();
            });
        }
    }
}
