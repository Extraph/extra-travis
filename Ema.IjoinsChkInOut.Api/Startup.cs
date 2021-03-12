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
using Microsoft.EntityFrameworkCore;

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

      services.Configure<UserIJoinDatabaseSettings>(Configuration.GetSection(nameof(UserIJoinDatabaseSettings)));

      services.AddSingleton<IUserIJoinDatabaseSettings>(sp => sp.GetRequiredService<IOptions<UserIJoinDatabaseSettings>>().Value);


      services.AddDbContext<userijoin_databaseContext>(options => options.UseNpgsql(Configuration["ConnectionStrings:userIJoinDbConnection"]));

      services.AddScoped<UserIjoinsServiceOld>();

      services.AddScoped<IUserIjoinsService, UserIjoinsService>();

      services.AddControllers();
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ema.IjoinsChkInOut.Api", Version = "v1" });
      });
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
