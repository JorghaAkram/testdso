//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Prove.API.Models.Auth;
using Prove.Data.Data;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
//using System.Security.Claims;
//using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Prove.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    //[Produces("application/json")]
    public class AuthController : BaseController
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ProveExtContext _proveExtDbContext;
        //private readonly AppUserCoreDbContext _appUserCoreDbContext;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<IdentityUser> userManager, IHttpClientFactory httpFactory, ProveExtContext proveExtDbContext, IConfiguration configuration)
        {
            _userManager = userManager;
            _httpFactory = httpFactory;
            _proveExtDbContext = proveExtDbContext;
            _configuration = configuration;
            //_appUserCoreDbContext = appUserCoreDbContext;
        }

        /// <summary>
        /// Untuk Login dongg
        /// </summary>
        /// <param name="q"></param>
        /// <param name="cancelationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel q, CancellationToken cancelationToken)
        {
            //HttpClient client = _httpFactory.CreateClient("AUTH");
            HttpClient clientLogin = _httpFactory.CreateClient("LoginIdaman");

            MultipartFormDataContent test = new MultipartFormDataContent();

            test.Add(new StringContent("f16a148a-02f5-4463-86a3-7675d331e2b8"), "client_id");
            test.Add(new StringContent("api.auth user.readAll position.read position.readAll user.role user.read"), "scope");
            test.Add(new StringContent("client_credentials"), "grant_type");
            test.Add(new StringContent("58b25cd3-082c-4bfd-91cb-6b75ae220fec"), "client_secret");

            //HttpResponseMessage httpResponse = await client.PostAsJsonAsync("/api/v1/auth/login", new { UserNameOrEmail = q.Email, q.Password });
            var httpResponse = await clientLogin.PostAsync($"/connect/token?username={q.Email}&password={q.Password}", test);

            //AuthenticationPayload authenticationPayload = null;
            AuthIdaman authIdaman = null;

            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //authenticationPayload = await httpResponse.Content.ReadAsAsync<AuthenticationPayload>();
                authIdaman = await httpResponse.Content.ReadAsAsync<AuthIdaman>();
                Response.Cookies.Append("JWT", authIdaman.access_token);
            }

            HttpClient clientAPI = _httpFactory.CreateClient("Idaman");

            //clientAPI.DefaultRequestHeaders.Add("Authorization", authIdaman.access_token);
            clientAPI.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authIdaman.access_token);

            //IdentityUser identityUser = await _userManager.FindByEmailAsync(q.Email) ?? await _userManager.FindByNameAsync(q.Email);
            var r = await clientAPI.GetAsync($"/v1/users?searchText={q.Email}");
            var res = await r.Content.ReadAsAsync<UserResponse>();

            //if (identityUser == null && authenticationPayload == null)
            if (res == null && authIdaman == null)
                return Unauthorized();

            var rl = await clientAPI.GetAsync($"/v1/roles/{res.value[0].id}");
            var resl = await rl.Content.ReadAsAsync<List<Roles>>();

            //if (identityUser == null && authenticationPayload != null)
            //{
            //    IdentityResult result = await _userManager.CreateAsync(new IdentityUser
            //    {
            //        Email = authenticationPayload.Response.Email,
            //        UserName = authenticationPayload.Response.Username
            //    });

            //    //if (result.Succeeded)
            //    //{
            //    //    identityUser = await _userManager.FindByEmailAsync(authenticationPayload.Response.Email);
            //    //    IdentityUser coreUser = await _appUserCoreDbContext.Users.FirstOrDefaultAsync(b => b.Email == identityUser.Email, cancelationToken);
            //    //    identityUser.PasswordHash = coreUser.PasswordHash;
            //    //    identityUser.ConcurrencyStamp = coreUser.ConcurrencyStamp;
            //    //    identityUser.SecurityStamp = coreUser.SecurityStamp;
            //    //    _simontanaDbContext.Users.Update(identityUser);
            //    //    await _simontanaDbContext.SaveChangesAsync(cancelationToken);
            //    //}
            //}
            //SimontanaAccount account = await _simontanaDbContext.Accounts.Include(b => b.Area).SingleOrDefaultAsync(b => b.Email == identityUser.Email, cancelationToken);

            //if (identityUser != null && authenticationPayload != null)
            //if (r != null && authIdaman != null)
            //    return Ok(GenerateToken(identityUser, await _userManager.GetRolesAsync(identityUser)));
            //return Ok(GenerateToken(identityUser, await _userManager.GetRolesAsync(identityUser)));

            //bool isPasswordValid = await _userManager.CheckPasswordAsync(identityUser, q.Password);

            //if (!isPasswordValid)
            //    return Unauthorized();

            string token = GenerateToken(res, resl, authIdaman.access_token);
            return Ok(token);
        }

        private string GenerateToken(UserResponse res, List<Roles> resl, string tokenIdaman)
        {
            List<Claim> claims = new List<Claim> {
                new Claim("Name",res.value[0].displayName),
                new Claim("TokenIdaman", tokenIdaman),
                new Claim("Username", res.value[0].email.Substring(0, Math.Max(res.value[0].email.IndexOf('@'), 0))),
                new Claim("Email",res.value[0].email),
                new Claim("JobTitle",res.value[0].jobTitle),
                new Claim("PositionName",res.value[0].position.name),
                new Claim("OrganizationName",res.value[0].position.organization.name),
                new Claim("CompanyCode",res.value[0].companyCode),
                new Claim("CompanyName",res.value[0].companyName)
                };

            claims.AddRange(resl[0].roles.Select(b => new Claim(ClaimTypes.Role, b.name)));
            //var claimsdata = new[]
            //   {
            //         new  Claim("name", res.value[0].displayName),
            //         new  Claim("name", res.value[0].displayName),
            //   };
            //if (account != null && account.Area != null)
            //    claims.Add(new Claim("area", JsonSerializer.Serialize(account.area))); //new Claim("area", JsonSerializer.Serialize(account.Area)));

            // Create the credentials used to generate the token
            var credentials = new SigningCredentials(
                // Get the secret key from configuration
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Secret:SecretKey"])),
                // Use HS256 algorithm
                SecurityAlgorithms.HmacSha256Signature);

            // Generate the Jwt Token
            var token = new JwtSecurityToken(
                issuer: _configuration["Secret:Issuer"],
                audience: _configuration["Secret:Audience"],
                claims: claims,
                signingCredentials: credentials,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Secret:ExpiryMinutes"])));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
