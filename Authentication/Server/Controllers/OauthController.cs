using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Server.Controllers
{
    public class OauthController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="response_type">Tipo de Autorización</param>
        /// <param name="client_id">Identidicador de Cliente</param>
        /// <param name="redirect_uri"></param>
        /// <param name="scope">Información solicitada</param>
        /// <param name="state">String random generado para verificar que va al cliente</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Authorize(string response_type, string client_id, string redirect_uri, string scope, string state)
        {
            var query = new QueryBuilder();

            query.Add("redirectUri", redirect_uri);
            query.Add("state", state);

            
            return View(model: query.ToString());
        }

        [HttpPost]
        public IActionResult Authorize(string username, string redirectUri, string state)
        {

            const string code = "CODE";

            var query = new QueryBuilder();

            query.Add("code", code);
            query.Add("state", state);


            return Redirect($"{redirectUri}{query.ToString()}");
        }

        public async Task<IActionResult> Token(string grant_type, string code, string redirect_uri, string client)
        {
            var claims = new[]
            {
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub, "some_id"),
                new Claim("granny", "cookie")
            };

            var secretBytes = Encoding.UTF8.GetBytes(Constants.Secret);

            var key = new SymmetricSecurityKey(secretBytes);

            var algorithm = SecurityAlgorithms.HmacSha256;

            var signinCredentials = new SigningCredentials(key, algorithm);

            var token = new JwtSecurityToken(
                Constants.Issuer,
                Constants.Audiance,
                claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddHours(1),
                signinCredentials
            );

            var access_token = new JwtSecurityTokenHandler().WriteToken(token);

            var responseObject = new
            {
                access_token,
                token_type = "Bearer",
                raw_claim = "oauthtutorial"
            };

            var responseJson = JsonConvert.SerializeObject(responseObject);

            var responseBytes = Encoding.UTF8.GetBytes(responseJson);

            await Response.Body.WriteAsync(responseBytes, 0, responseBytes.Length);

            return Redirect(redirect_uri);
        }
    }
}
