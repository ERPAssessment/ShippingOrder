using ShippingOrder.Infrastructure.Workers;

namespace ShippingOrder.Infrastructure.DI;

internal static class WorkerDependencyInjectioncs
{
  private const int OUTBOX_PROCESSING_INTERVAL_SECONDS = 10;
  private const string OUTBOX_JOB_GROUP = "OutboxProcessing";

  internal static IServiceCollection AddBackgroundJobs(
    this IServiceCollection services,
    IConfiguration configuration)
  {
    var quartzConfig = configuration.GetSection("Quartz");
    var processingInterval = quartzConfig.GetValue<int?>("OutboxProcessingIntervalSeconds")
        ?? OUTBOX_PROCESSING_INTERVAL_SECONDS;

    services.AddQuartz(configure =>
    {
      configure.SchedulerName = "ShippingOrder Background Scheduler";
      configure.SchedulerId = "ShippingOrderScheduler";


      configure.UseDefaultThreadPool(tp =>
      {
        tp.MaxConcurrency = Environment.ProcessorCount;
      });

      ConfigureOutboxProcessingJob(configure, processingInterval);

    });

    services.AddQuartzHostedService(options =>
    {
      options.WaitForJobsToComplete = true;
      options.AwaitApplicationStarted = true;
    });

    return services;
  }

  private static void ConfigureOutboxProcessingJob(
      IServiceCollectionQuartzConfigurator configure,
      int intervalSeconds)
  {
    var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob), OUTBOX_JOB_GROUP);
    var triggerKey = new TriggerKey($"{nameof(ProcessOutboxMessagesJob)}_Trigger", OUTBOX_JOB_GROUP);

    configure.AddJob<ProcessOutboxMessagesJob>(jobKey, job =>
    {
      job.WithDescription("Processes outbox messages for reliable message delivery")
         .StoreDurably(false)
         .RequestRecovery(true);
    });

    configure.AddTrigger(trigger =>
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
  }
}
