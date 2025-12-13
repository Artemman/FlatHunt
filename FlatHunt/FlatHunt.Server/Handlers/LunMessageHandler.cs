using FlatHunt.Server.Exceptions;

namespace FlatHunt.Server.Handlers
{
    public class LunMessageHandler : DelegatingHandler
    {
        private readonly ILogger<LunMessageHandler> _logger;

        public LunMessageHandler(ILogger<LunMessageHandler> logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
#if DEBUG
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogInformation("Lun reponse: {0}", content);
#endif
            EnsureSuccessStatusCode(response);

            return response;
        }

        private static void EnsureSuccessStatusCode(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new LunApiException();
            }
        }
    }
}
