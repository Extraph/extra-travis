
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
using Ema.Ijoins.Api.EfAdminModels;
using Ema.Ijoins.Api.EfUserModels;
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

      services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));


      // var envAwsAccessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY");
      // var envAwsAccessSecret = Environment.GetEnvironmentVariable("AWS_ACCESS_SECRET");
      // var envAwsBucket = Environment.GetEnvironmentVariable("AWS_BUCKET");
      // services.Configure<AWSSetting>(
      // options =>
      // {
      //   options.AccessKey = envAwsAccessKey;
      //   options.AccessSecret = envAwsAccessSecret;
      //   options.Bucket = envAwsBucket;
      // });

      services.Configure<AWSSetting>(
      options =>
      {
        options.AccessKey = Configuration["AWSSetting:AccessKey"];
        options.AccessSecret = Configuration["AWSSetting:AccessSecret"];
        options.Bucket = Configuration["AWSSetting:Bucket"];
      });



      var adminPgHost = Environment.GetEnvironmentVariable("DB_ADMIN_PGHOST");
      var adminPgPort = Environment.GetEnvironmentVariable("DB_ADMIN_PGPORT");
      var adminPgDatabase = Environment.GetEnvironmentVariable("DB_ADMIN_PGDATABASE");
      var adminPgUser = Environment.GetEnvironmentVariable("DB_ADMIN_PGUSER");
      var adminPgPassword = Environment.GetEnvironmentVariable("DB_ADMIN_PGPASSWORD");

      var userPgHost = Environment.GetEnvironmentVariable("DB_USER_PGHOST");
      var userPgPort = Environment.GetEnvironmentVariable("DB_USER_PGPORT");
      var userPgDatabase = Environment.GetEnvironmentVariable("DB_USER_PGDATABASE");
      var userPgUser = Environment.GetEnvironmentVariable("DB_USER_PGUSER");
      var userPgPassword = Environment.GetEnvironmentVariable("DB_USER_PGPASSWORD");

      // var adminPgHost = Configuration["ConnectionStrings:adminPgHost"];
      // var adminPgPort = Configuration["ConnectionStrings:adminPgPort"];
      // var adminPgDatabase = Configuration["ConnectionStrings:adminPgDatabase"];
      // var adminPgUser = Configuration["ConnectionStrings:adminPgUser"];
      // var adminPgPassword = Configuration["ConnectionStrings:adminPgPassword"];

      // var userPgHost = Configuration["ConnectionStrings:userPgHost"];
      // var userPgPort = Configuration["ConnectionStrings:userPgPort"];
      // var userPgDatabase = Configuration["ConnectionStrings:userPgDatabase"];
      // var userPgUser = Configuration["ConnectionStrings:userPgUser"];
      // var userPgPassword = Configuration["ConnectionStrings:userPgPassword"];

      services.AddDbContext<adminijoin_databaseContext>(options => options.UseNpgsql(string.Format("Server={0};Port={1};Database={2};Username={3};Password={4}", adminPgHost, adminPgPort, adminPgDatabase, adminPgUser, adminPgPassword)));
      services.AddDbContext<userijoin_databaseContext>(options => options.UseNpgsql(string.Format("Server={0};Port={1};Database={2};Username={3};Password={4}", userPgHost, userPgPort, userPgDatabase, userPgUser, userPgPassword)));



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

      app.UseRouting();

      app.UseCors(x => x
         .AllowAnyOrigin()
         .AllowAnyMethod()
         .AllowAnyHeader()
         );

      app.UseMiddleware<JwtMiddleware>();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
