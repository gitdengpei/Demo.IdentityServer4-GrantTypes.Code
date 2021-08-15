using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Demo.WebAPI
{
	public class Startup
	{

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();
			//认证
			services.AddAuthentication("Bearer")
				.AddJwtBearer("Bearer", options =>
				 {
					 options.Authority = "https://localhost:5001";
					 options.RequireHttpsMetadata = false;
					 options.TokenValidationParameters = new TokenValidationParameters
					 {
						 ValidateAudience = false
					 };
				 });
			//授权
			services.AddAuthorization(options =>
			{
				//1.添加策略名称
				options.AddPolicy("ApiScope", builder =>
				 {
					 //判断用户是否通过认证
					 builder.RequireAuthenticatedUser();
					 builder.RequireClaim("scope", "api1");
				 });
			});
		}


		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();
			app.UseAuthentication();//认证
			app.UseAuthorization();//授权

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				//应用所有api控制器
				//.RequireAuthorization("ApiScope");
			});
		}
	}
}
