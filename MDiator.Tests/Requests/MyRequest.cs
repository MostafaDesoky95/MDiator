using MDiator;


namespace MDiator.Tests.Requests
{
    public class MyRequest : IMDiatorRequest<string>
    {
        public string Message { get; set; } = "Test";
    }

}
