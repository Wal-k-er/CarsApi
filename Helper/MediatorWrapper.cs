using MediatR;
using Newtonsoft.Json;

namespace Cars.Helper;

public class MediatorWrapper
{
    private readonly IMediator _mediator;

    public MediatorWrapper(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Send(string json)
    {
        var request = JsonConvert.DeserializeObject(json, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        });

        var task = (Task)_mediator.Send(request);
        await task.ConfigureAwait(false);
    }
}