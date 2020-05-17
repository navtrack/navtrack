using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Models;
using Navtrack.Api.Services;

namespace Navtrack.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProtocolsController : ControllerBase
    {
        private readonly IProtocolService protocolService;

        public ProtocolsController(IProtocolService protocolService)
        {
            this.protocolService = protocolService;
        }

        [HttpGet]
        public List<ProtocolModel> GetProtocols()
        {
            return protocolService.GetProtocols();
        }
    }
}