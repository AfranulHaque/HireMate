using Quartz;

namespace Hiremate.Background.Scheduler.BackGroundProcessor
{
    public class DummyJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Dummy job executed at: " + DateTime.Now);
        }
    }
}
