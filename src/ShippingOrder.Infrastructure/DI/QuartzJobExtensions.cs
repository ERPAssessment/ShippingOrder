using ShippingOrder.Infrastructure.Workers;

namespace ShippingOrder.Infrastructure.DI;

// Extension class for better organization of job-related configurations
public static class QuartzJobExtensions
{
  public static IServiceCollectionQuartzConfigurator AddOutboxProcessingJob(
      this IServiceCollectionQuartzConfigurator configurator,
      IConfiguration configuration,
      string jobGroup = "OutboxProcessing")
  {
    var intervalSeconds = int.Parse(configuration["Quartz:OutboxIntervalSeconds"]!);

    var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob), jobGroup);
    var triggerKey = new TriggerKey($"{nameof(ProcessOutboxMessagesJob)}_Trigger", jobGroup);

    configurator.AddJob<ProcessOutboxMessagesJob>(jobKey, job =>
    {
      job.WithDescription("Processes outbox messages for reliable message delivery")
         .StoreDurably(false)
         .RequestRecovery(true);
    });

    configurator.AddTrigger(trigger =>
    {
      trigger.ForJob(jobKey)
             .WithIdentity(triggerKey)
             .WithDescription($"Triggers outbox processing every {intervalSeconds} seconds")
             .WithSimpleSchedule(schedule =>
             {
               schedule.WithIntervalInSeconds(intervalSeconds)
                            .RepeatForever()
                            .WithMisfireHandlingInstructionIgnoreMisfires();
             })
             .StartNow();
    });

    return configurator;
  }
}