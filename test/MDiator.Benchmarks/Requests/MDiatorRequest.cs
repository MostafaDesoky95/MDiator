namespace MDiator.Benchmarks.Requests;

public class MDiatorRequest : IMDiatorRequest<string>
{
    public string Payload => "Ping";
}
