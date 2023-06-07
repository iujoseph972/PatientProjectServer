using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PatientProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // configuration and services

            services.AddControllers();

            services.Configure<PatientServiceApiOptions>(Configuration.GetSection("PatientServiceApi"));

            services.AddHttpClient<IPatientServiceApiClient, PatientServiceApiClient>();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                        ValidateIssuer = true,
                        ValidIssuer = Configuration["Authentication:Schemes:Bearer:ValidIssuer"],
                        ValidateAudience = true,
                        ValidAudiences = Configuration.GetSection("Authentication:Schemes:Bearer:ValidAudiences").Get<List<string>>(),
                    };
                });


            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });

         
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            //middleware configurations
            app.UseCors(); // setting up cors
            app.UseAuthentication(); // setting up Authentication
            app.UseAuthorization();// setting up Authorization

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
