using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreWebAPIPOC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OAuth2POCController : ControllerBase
    {


        private readonly ILogger<OAuth2POCController> _logger;

        public OAuth2POCController(ILogger<OAuth2POCController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "AuthTest")]
        [Authorize]
        [Authorize(Policy = "APIOauthPOCScope")]
        [Authorize(Policy = "HasCustomRolePOC")]
        public string Get()
        {
            return "Success";
        }
    }
}
