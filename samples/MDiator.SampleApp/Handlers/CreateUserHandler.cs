using MDiator.SampleApp.Requests;

namespace MDiator.SampleApp.Handlers
{
    public class CreateUserHandler : IMDiatorHandler<CreateUserCommand, string>
    {
        public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            await Task.Delay(1000, cancellationToken); // Simulate some async work
            
            return await Task.FromResult($"✅ User '{request.UserName}' created.");
        }
    }
}
