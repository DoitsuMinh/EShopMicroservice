using Autofac;
using Quartz;
using Quartz.Spi;

namespace Odering.Infrastructure.Quartz;

public class JobFactory : IJobFactory
{
    private readonly IContainer _container;

    public JobFactory(IContainer container)
    {
        _container = container;
    }

    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        var job = _container.Resolve(bundle.JobDetail.JobType) as IJob;

        return job;
    }

    public void ReturnJob(IJob job)
    {
        
    }
}
