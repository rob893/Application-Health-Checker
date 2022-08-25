using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ApplicationHealthChecker.HealthChecks
{
    public class GQLHealthCheck : IHealthCheck
    {
        private readonly IHttpClientFactory clientFactory;


        public GQLHealthCheck(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var client = this.clientFactory.CreateClient();

            var res = await client.GetAsync(new Uri("http://localhost:4000/.well-known/apollo/server-health"), cancellationToken);

            var healthCheckResultHealthy = res.IsSuccessStatusCode;

            if (healthCheckResultHealthy)
            {
                return HealthCheckResult.Healthy("GQL is running.");
            }

            return HealthCheckResult.Unhealthy("GQL is not running.");
        }
    }
}