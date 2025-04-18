# MDiator 🧩

**MDiator** is a lightweight, extensible in-process messaging library for .NET.  
It provides clean abstractions for sending requests and publishing events using a mediator pattern — without the bloat.

Inspired by MediatR, built for performance and clarity.

---

## 🚀 Features

- `Send<TRequest>`: Handle a single request with a typed response
- `Publish<TEvent>`: Broadcast events to multiple handlers
- Auto-registration of handlers via assembly scanning
- No reflection at runtime – uses compiled delegates
- Minimal dependencies

---

## 📦 Installation

Add it to your project (once published to NuGet):

```bash
dotnet add package MDiator
```

Register in Program.cs
```
services.AddMDiator(typeof(Program).Assembly);
```


🧪 Example Usage
Define a Command

```
public class CreateUserCommand : IMDiatorRequest<string>
{
    public string UserName { get; set; }
}

public class CreateUserHandler : IMDiatorHandler<CreateUserCommand, string>
{
    public Task<string> Handle(CreateUserCommand command)
        => Task.FromResult($"Created user: {command.UserName}");
}
```
```
var result = await mediator.Send(new CreateUserCommand { UserName = "Mostafa" });
```

📣 Event Example
```
public class OrderCreatedEvent : IMDiatorEvent
{
    public int OrderId { get; set; }
}

public class NotifyTeamHandler : IMDiatorEventHandler<OrderCreatedEvent>
{
    public Task Handle(OrderCreatedEvent e)
    {
        Console.WriteLine($"Notify team about order {e.OrderId}");
        return Task.CompletedTask;
    }
}
```
```
await mediator.Publish(new OrderCreatedEvent { OrderId = 123 });
```
