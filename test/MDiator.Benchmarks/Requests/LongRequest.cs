using MediatR;

namespace MDiator.Benchmarks.Requests;

public class LongRequest : IRequest<string>, IMDiatorRequest<string>
{
    public string Payload => "Long Ping";
}
