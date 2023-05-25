using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProEventos.Application.Interfaces;
using ProEventos.Application.Services;
using ProEventos.Persistence;
using ProEventos.Persistence.Context;
using ProEventos.Persistence.Interfaces;
using Microsoft.Extensions.FileProviders;
using System.Text.Json.Serialization;
using ProEventos.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ProEventos.API;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ProEventosContexto>(
            context => context.UseSqlite(Configuration.GetConnectionString("Default"))
        );

        services.AddIdentityCore<User>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 4;
        })
        .AddRoles<Role>()
        .AddRoleManager<RoleManager<Role>>()
        .AddSignInManager<SignInManager<User>>()
        .AddRoleValidator<RoleValidator<Role>>()
        .AddEntityFrameworkStores<ProEventosContexto>()
        .AddDefaultTokenProviders();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenKey"])),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

        services.AddControllers()
                .AddJsonOptions(options => 
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
                )
                .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
        services.AddCors();
        services.AddScoped<IGeralPersist, GeralPersist>();
        services.AddScoped<IEventoPersist, EventoPersist>();
        services.AddScoped<IEventosServices, EventosServices>();
        services.AddScoped<ILotePersist, LotePersist>();
        services.AddScoped<ILoteServices, LoteServices>();
        services.AddScoped<IPalestrantePersist, PalestrantePersist>();


        services.AddScoped<IUserPersist, UserPersist>();
        services.AddScoped<ITokenServices, TokenServices>();
        services.AddScoped<IAccountServices, AccountServices>();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProEventos.API", Version = "v1" });
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProEventos.API v1"));
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseCors(x => x.AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowAnyOrigin());

        app.UseStaticFiles(new StaticFileOptions() {
            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Resources")),
            RequestPath = new PathString("/Resources")
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
