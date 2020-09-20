using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model.Protocols;
using Navtrack.Api.Services.Protocols;

namespace Navtrack.Api.Controllers
{
    public class ProtocolsController : BaseController
    {
        private readonly IProtocolService protocolService;

        public ProtocolsController(IProtocolService protocolService, IServiceProvider serviceProvider) : base(
            serviceProvider)
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