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
            //jwt�����Ϣ����ӳ�� �����棬����˵������Щ���ԣ�ӳ�������ռ䡣Ӱ�������Ϣ����
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));

            services.AddAuthentication(options =>
            {
                //ָ��Ĭ����֤�������� ʹ��Cookies ����Ϊ��֤���� �����Ʊ�����Cookies
                options.DefaultScheme = "Cookies";
                //ָ����ѭ����
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = "https://localhost:5001";
                    options.RequireHttpsMetadata = false;//����httpsЭ��
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

            app.UseAuthentication();//��֤
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
