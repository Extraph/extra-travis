using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Ema.IjoinsChkInOut.Api.Models;
using Ema.IjoinsChkInOut.Api.EfUserModels;
using Ema.IjoinsChkInOut.Api.Services;
using Ema.IjoinsChkInOut.Api.Helpers;
using Microsoft.EntityFrameworkCore;
using System;

namespace Ema.IjoinsChkInOut.Api
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
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ema.IjoinsChkInOut.Api", Version = "v1" });
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



      var userPgHost = Environment.GetEnvironmentVariable("DB_USER_PGHOST");
      var userPgPort = Environment.GetEnvironmentVariable("DB_USER_PGPORT");
      var userPgDatabase = Environment.GetEnvironmentVariable("DB_USER_PGDATABASE");
      var userPgUser = Environment.GetEnvironmentVariable("DB_USER_PGUSER");
      var userPgPassword = Environment.GetEnvironmentVariable("DB_USER_PGPASSWORD");

      // var userPgHost = Configuration["ConnectionStrings:userPgHost"];
      // var userPgPort = Configuration["ConnectionStrings:userPgPort"];
      // var userPgDatabase = Configuration["ConnectionStrings:userPgDatabase"];
      // var userPgUser = Configuration["ConnectionStrings:userPgUser"];
      // var userPgPassword = Configuration["ConnectionStrings:userPgPassword"];

      services.AddDbContext<userijoin_databaseContext>(options => options.UseNpgsql(string.Format("Server={0};Port={1};Database={2};Username={3};Password={4}", userPgHost, userPgPort, userPgDatabase, userPgUser, userPgPassword)));



      services.AddScoped<IUserIjoinsService, UserIjoinsService>();

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ema.IjoinsChkInOut.Api v1"));
      }

      app.UseCors(x => x
          .AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader()
          );

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
