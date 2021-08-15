using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Demo.Mvc
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
            services.AddControllersWithViews();
            //jwt身份信息类型映射 它里面，比如说，把有些属性，映射命名空间。影响身份信息交互
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));

            services.AddAuthentication(options =>
            {
                //指定默认认证方案名称 使用Cookies 来作为认证方案 把令牌保存在Cookies
                options.DefaultScheme = "Cookies";
                //指定遵循方案
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = "https://localhost:5001";
                    options.RequireHttpsMetadata = false;//必须https协议
                    options.ClientId = "demo_mvc_client";
                    options.ClientSecret = "secret";
                    options.ResponseType = "code";
                    options.Scope.Add("api1");
                    options.SaveTokens = true;
                });

        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();//认证
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
