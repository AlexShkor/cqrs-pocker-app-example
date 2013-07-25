using Quartz;
using Quartz.Impl.Triggers;
using Quartz.Impl;

namespace PAQK.Platform.Scheduling
{
    public interface IScheduledJob : IJob
    {
        JobDetailImpl ConfigureJob();

        SimpleTriggerImpl ConfigureTrigger();

        bool IsEnabled { get; }
    }
}
