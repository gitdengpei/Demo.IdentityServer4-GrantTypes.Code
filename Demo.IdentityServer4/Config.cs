using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;

namespace Demo.IdentityServer4
{
	public static class Config
	{
		/// <summary>
		/// api资源列表
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
		{
			
		};
		/// <summary>
		/// 4.0版本需要添加apiscope  保护的范围
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<ApiScope> ApiScopes => new ApiScope[]
		{
			new ApiScope("api1","api service")
		};
		public static IEnumerable<Client> Clients => new Client[]
		{
			new Client
			{
				ClientId = "demo_mvc_client",
				ClientName = "demo mvc client",
				ClientSecrets = { new Secret("secret".Sha256()) },
				//授权码
				AllowedGrantTypes = GrantTypes.Code,
				//登录
				RedirectUris = {"https://localhost:5004/signin-oidc"}, //signin-oidc 标准端口协议名称 在规范里定义
				//登出
				PostLogoutRedirectUris = { "https://localhost:5004/siginout-callbac-oidc" },

				AllowedScopes = {
					IdentityServerConstants.StandardScopes.OpenId,
					IdentityServerConstants.StandardScopes.Profile,
					"api1"
				},
				//是否需要用户同意 
				RequireConsent = true
			}
		};
		/// <summary>
		/// 身份资源。 要保护的资源 允许可访问的身份资源
		/// </summary>
		public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>
		{
			new IdentityResources.OpenId(),//必须要保护的
			new IdentityResources.Profile(), //昵称 、头像
		};
		public static List<TestUser> TestUsers => new List<TestUser>()
		{
			new TestUser
			{
				SubjectId = Guid.NewGuid().ToString(),
				Username = "admin",
				Password = "123"
			}
		};
	}
}
