using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Demo.IdentityServer4
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			//注册mvc配置
			services.AddControllersWithViews();

			#region IdentityServer4 配置
			var builder = services.AddIdentityServer();
			builder.AddDeveloperSigningCredential();//生成的密钥将被保存到文件系统，以便在服务器重新启动之间保持稳定（可以通过传递来禁用false）
			builder.AddInMemoryClients(Config.Clients);
			builder.AddInMemoryApiScopes(Config.ApiScopes);
			builder.AddInMemoryApiResources(Config.ApiResources);
			builder.AddInMemoryIdentityResources(Config.IdentityResources);
			builder.AddTestUsers(Config.TestUsers); 
			#endregion
		}
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			app.UseStaticFiles();//注册文件中间件
			app.UseRouting(); //注册路由
			app.UseIdentityServer();//包含认证
			app.UseAuthorization();//注册授权中间件
			app.UseEndpoints(enpoints =>
			{
				enpoints.MapDefaultControllerRoute();//使用默认控制器路由
			});
		}
	}
}
