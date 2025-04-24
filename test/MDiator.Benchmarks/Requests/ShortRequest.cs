using MediatR;

namespace MDiator.Benchmarks.Requests;

public class ShortRequest : IRequest<string>, IMDiatorRequest<string>
{
    public string Payload => "Short Ping";
}
