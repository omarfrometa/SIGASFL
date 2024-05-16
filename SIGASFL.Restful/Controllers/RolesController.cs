using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIGASFL.Models;
using SIGASFL.Models.Views;
using SIGASFL.Services.Interface;

namespace SIGASFL.Restful.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly ILogger<RolesController> _logger;
        private readonly IRolesService rolesService;

        public RolesController(ILogger<RolesController> logger, IRolesService rolesService)
        {
            _logger = logger;
            this.rolesService = rolesService;
        }

        // GET: api/<Controller>
        [HttpGet, AllowAnonymous]
        public async Task<ClientResponse<IEnumerable<RolesView>>> Get()
        {
            return await rolesService.GetAll();
        }

        // GET api/<Controller>/5
        [HttpGet("{id}")]
        public async Task<ClientResponse<RolesView>> Get(string id)
        {
            return await rolesService.GetById(id);
        }

        // POST api/<Controller>
        [HttpPost]
        public async Task<ClientResponse<RolesView>> Post(RolesView _view)
        {
            return await rolesService.Create(_view);
        }

        // PUT api/<Controller>/5
        [HttpPut]
        public async Task<ClientResponse<RolesView>> Put(RolesView _view)
        {
            return await rolesService.Edit(_view);
        }

        // DELETE api/<Controller>/5
        [HttpDelete("{id}")]
        public async Task<ClientResponse<bool>> Delete(string id)
        {
            return await rolesService.Delete(id);
        }
    }
}
