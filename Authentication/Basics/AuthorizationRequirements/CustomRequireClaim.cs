using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basics.AuthorizationRequirements
{
    /// <summary>
    /// Estructura de solicitud para ser autorizado por el manejador
    /// </summary>
    public class CustomRequireClaim : IAuthorizationRequirement
    {
        public string ClaimType { get; }
        public CustomRequireClaim(string claimType)
        {
            ClaimType = claimType;
        }
    }

    /// <summary>
    /// Manejador de la solicitud de autorización
    /// </summary>
    public class CustomRequireClaimHandler : AuthorizationHandler<CustomRequireClaim>
    {
        /// <summary>
        /// Implementar conexiones a BD y servicios acá
        /// </summary>
        public CustomRequireClaimHandler()
        {

        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomRequireClaim requirement)
        {
            var hasClaim = context.User.Claims.Any(x => x.Type == requirement.ClaimType);
            if(hasClaim)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public static class AuthorizationePolicyBuilderExtensions
    {
        public static AuthorizationPolicyBuilder RequireCustomClaim(this AuthorizationPolicyBuilder builder, string claimType)
        {
            builder.AddRequirements(new CustomRequireClaim(claimType));
            return builder;
        }
    }
}
