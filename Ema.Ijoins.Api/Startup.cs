
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;
using Ema.Ijoins.Api.EfModels;
using Ema.Ijoins.Api.Services;
using Ema.Ijoins.Api.Models;
using Ema.Ijoins.Api.Helpers;
using System;

namespace Ema.Ijoins.Api
{
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

      services.AddControllers();
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ema.Ijoins.Api", Version = "v1" });
      });

      services.Configure<UserIJoinDatabaseSettings>(Configuration.GetSection(nameof(UserIJoinDatabaseSettings)));

      services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

      services.AddSingleton<IUserIJoinDatabaseSettings>(sp => sp.GetRequiredService<IOptions<UserIJoinDatabaseSettings>>().Value);

      var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
      services.AddDbContext<ema_databaseContext>(options => options.UseNpgsql(connectionString));

      //services.AddDbContext<ema_databaseContext>(options => options.UseNpgsql(Configuration["ConnectionStrings:appDbConnection"]));

      services.AddScoped<UserIjoinsService>();

      services.AddScoped<IAdminIjoinsService, AdminIjoinsService>();

      services.AddScoped<IUserService, UserService>();

      services.Configure<FormOptions>(options =>
      {
        // Set the limit to 256 MB => 268435456
        // Set the limit to 50 MB => 52428800
        options.MultipartBodyLengthLimit = 268435456;
      });

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ema.Ijoins.Api v1"));
      }

      app.UseCors(x => x
          .AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader()
          );

      app.UseRouting();

      //app.UseAuthorization();

      app.UseMiddleware<JwtMiddleware>();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
