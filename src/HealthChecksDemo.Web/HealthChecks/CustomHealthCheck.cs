using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;

namespace HealthChecksDemo.Web.HealthChecks
{
    public class CustomHealthCheck : IHealthCheck
    {
        private readonly IConfiguration _configuration;

        public CustomHealthCheck(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            var healthCheckResultHealthy = _configuration.GetValue<bool>("IsHealthyDemo");

            if (healthCheckResultHealthy)
            {
                return Task.FromResult(HealthCheckResult.Healthy("A healthy result."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("An unhealthy result."));
        }
    }
}