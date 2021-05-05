using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using ticketsystem_backend.Data;
using ticketsystem_backend.Migrations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ticketsystem_backend
{
    public class Startup
    {
        readonly string MySpecificOrigin = "EnableCORS";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // add cors to service and configure it
            services.AddCors(options =>
            {
                options.AddPolicy(name: MySpecificOrigin, builder =>
                {
                    // no restrictions due to prototype use
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            // add database context to service
            services.AddDbContext<TicketSystemDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("TicketContext")));
            services.AddTransient<DbSeedData>();

            // add authentication to service using JwtBearer
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // configure JwtBearer
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "*",
                    ValidAudience = "*",
                    IssuerSigningKey = new SymmetricSecurityKey(Base64UrlEncoder.DecodeBytes("MBcCT4UEs67vh3shK683Lxhn33t2LTtH"))
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DbSeedData seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // use https connection
            app.UseHttpsRedirection();

            app.UseRouting();

            // use cors that is configured in ConfigureServices
            app.UseCors(MySpecificOrigin);

            // use authentication that is configured in ConfigureServices
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // seed data for development use
            seeder.EnsureSeedData();
        }
    }
}
