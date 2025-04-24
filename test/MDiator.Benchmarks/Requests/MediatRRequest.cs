using MediatR;

namespace MDiator.Benchmarks.Requests;

public class MediatRRequest : IRequest<string>
{
    public string Payload => "Ping";
}
