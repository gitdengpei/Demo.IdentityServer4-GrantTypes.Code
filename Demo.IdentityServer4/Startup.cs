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
			//ע��mvc����
			services.AddControllersWithViews();

			#region IdentityServer4 ����
			var builder = services.AddIdentityServer();
			builder.AddDeveloperSigningCredential();//���ɵ���Կ�������浽�ļ�ϵͳ���Ա��ڷ�������������֮�䱣���ȶ�������ͨ������������false��
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
			app.UseStaticFiles();//ע���ļ��м��
			app.UseRouting(); //ע��·��
			app.UseIdentityServer();//������֤
			app.UseAuthorization();//ע����Ȩ�м��
			app.UseEndpoints(enpoints =>
			{
				enpoints.MapDefaultControllerRoute();//ʹ��Ĭ�Ͽ�����·��
			});
		}
	}
}
