using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace API.AuthRequirement
{
    public class JwtRequirement : IAuthorizationRequirement 
    {

    }

    public class JwtRequirementhandler : AuthorizationHandler<JwtRequirement>
    {
        private readonly HttpClient _client;
        private readonly HttpContext _httpContext;

        public JwtRequirementhandler(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _client = httpClientFactory.CreateClient();
            _httpContext = httpContextAccessor.HttpContext;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, JwtRequirement requirement)
        {
            if(_httpContext.Request.Headers.TryGetValue("Authorization",out var authHeader))
            {
                var accessToken = authHeader.ToString().Split(" ").Last();

                var response = await _client.GetAsync($"https://localhost:44390/oauth/validate?access_token={accessToken}");

                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}
