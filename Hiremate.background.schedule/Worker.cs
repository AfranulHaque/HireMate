
namespace Hiremate.Background.Scheduler
{
    public class Worker : BackgroundService
    {
        //private readonly ILogger<Worker> _logger;
        ////private readonly IAgentService _agentService;

        //public Worker(ILogger<Worker> logger, IAgentService agentServic)
        //{
        //    _logger = logger;
        //    _agentService = agentService;
        //}

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
            //    await _agentService.IsCandidateEligible("asdasd", "agag");
            //    if (_logger.IsEnabled(LogLevel.Information))
            //    {
            //        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //    }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
