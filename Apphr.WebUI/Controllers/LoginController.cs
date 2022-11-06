using Apphr.WebUI.Security;
using System.Net;
using System.Threading;
using System.Web.Http;

namespace Apphr.WebUI.Controllers
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    /// <summary>
    /// login controller class for authenticate users
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        [HttpGet]
        [Route("echoping")]
        public IHttpActionResult EchoPing()
        {
            return Ok(true);
        }

        [HttpGet]
        [Route("echouser")]
        public IHttpActionResult EchoUser()
        {
            var identity = Thread.CurrentPrincipal.Identity;
            return Ok($" IPrincipal-user: {identity.Name} - IsAuthenticated: {identity.IsAuthenticated}");
        }

        [HttpPost]
        [Route("authenticate")]
        public IHttpActionResult Authenticate(LoginRequest login)
        {
            if (login == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            //TODO: Validate credentials Correctly, this code is only for demo !!
            bool isCredentialValid = (login.Password == "3#yWbo>#Xp0^%J7lM-zc0E[v_wWzX=" && login.Username == "me@juanfrancisco.io");
            //var tokenHandler = new JwtSecurityTokenHandler();
            if (isCredentialValid)
            {
                var token =  TokenGenerator.GenerateTokenJwt(login.Username);
               // var token = JwtManager.GenerateToken(login.Username);
                return Ok(new { success = true, token });
            }
            else
            {
                return Ok(new { success = false }); //Unauthorized();
            }
        }
    }
}
