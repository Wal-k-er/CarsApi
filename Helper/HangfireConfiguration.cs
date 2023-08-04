using Cars.Features.CarFeatures.Commands;
using Hangfire;
using MediatR;
using Newtonsoft.Json;

namespace Cars.Helper;
[AutomaticRetry(Attempts = 3)]
public class HangfireConfiguration
{
    private readonly IMediator _mediator;
    private readonly IRecurringJobManager _recurringJobManager;

    public HangfireConfiguration(IMediator mediator, IRecurringJobManager recurringJobManager)
    {
        _mediator = mediator;
        _recurringJobManager = recurringJobManager;
    }
    
    public void ScheduleUpdateTask()
    {
        var command = new UpdateCarPricesRandomly();
        var json = JsonConvert.SerializeObject(command, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        });
        var mediatorWrapper = new MediatorWrapper(_mediator);
        
        _recurringJobManager.AddOrUpdate("UpdatePrices",
            () => mediatorWrapper.Send(json), "* * * * *");
    }
}