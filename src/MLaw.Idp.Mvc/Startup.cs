using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MLaw.Idp.Mvc.Models;
using MLaw.Idp.Mvc.Models.Validators;
using MLaw.Idp.Mvc.Services;

namespace MLaw.Idp.Mvc
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<IdpSettings.IdpSettings>(options => Configuration.GetSection("IdpSettings").Bind(options));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMvc().AddFluentValidation();
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = "localhost:6379";
                options.InstanceName = "RedisInstance";
            });
        
            services.AddSingleton<ILoggedUsersStorage, LoggedUsersDistributedStorage>();
            services.AddSingleton<CacheKeyCreator>();
            services.AddTransient<IValidator<LoggedinUserModel>, LoggedinUserModelValidator>();
            services.AddTransient<IValidator<IdentityServerRequestModel>, IdentityServerRequestModelValidator>();
            services.AddTransient<AbstractValidator<LogingViewModel>, LoginUserModelValidator>();
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseStatusCodePages(async context =>
            {
                context.HttpContext.Response.ContentType = "text/plain";
                await context.HttpContext.Response.WriteAsync(
                    "Status code page, status code: " +
                    context.HttpContext.Response.StatusCode);
            });
            app.UseMvcWithDefaultRoute();
        }
    }
}