using MDiator.SampleApp.Requests;

namespace MDiator.SampleApp.Handlers
{
    public class CreateUserHandler : IMDiatorHandler<CreateUserCommand, string>
    {
        public Task<string> HandleAsync(CreateUserCommand request)
        {
            return Task.FromResult($"✅ User '{request.UserName}' created.");
        }
    }
}
