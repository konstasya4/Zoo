using Auto.NamesEngine;
using Auto.NamesServer;
using Grpc.Core;

namespace Auto.NamesServer.Services
{
    public class NamerService : Namer.NamerBase {
        private readonly ILogger<NamerService> logger;
        public NamerService(ILogger<NamerService> logger) {
            this.logger = logger;
        }

        public override Task<NameReply> GetName(NameRequest request, ServerCallContext context)
        {
            return Task.FromResult(new NameReply() { WeightCode = "85g", Name = "Sterilised" });
        }
    }
}