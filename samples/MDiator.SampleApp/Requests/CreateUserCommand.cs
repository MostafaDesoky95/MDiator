namespace MDiator.SampleApp.Requests
{
    public class CreateUserCommand : IMDiatorRequest<string>
    {
        public string UserName { get; set; }
    }
}
