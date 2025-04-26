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
- CancelationToken support

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

## 🚀 Performance Benchmark

MDiator is designed to be lean and fast — with no runtime reflection and minimal allocations.  
Here’s a comparison with MediatR using [BenchmarkDotNet](https://benchmarkdotnet.org/):

| Method                          | Mean      | Error     | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|-------------------------------- |----------:|----------:|---------:|------:|--------:|-------:|----------:|------------:|
| **MDiator_HandleEventAsync**    | 105.51 ns | 13.308 ns | 0.729 ns |  1.00 |    0.01 | 0.0067 |      56 B |        1.00 |
| MediatR_HandleEventAsync        | 141.35 ns | 28.874 ns | 1.583 ns |  1.34 |    0.02 | 0.0372 |     312 B |        5.57 |
|                                 |           |           |          |       |         |        |           |             |
| **MDiator_HandleRequestAsync**  |  84.64 ns |  7.637 ns | 0.419 ns |  1.00 |    0.01 | 0.0200 |     168 B |        1.00 |
| MediatR_HandleRequestAsync      | 106.22 ns | 31.459 ns | 1.724 ns |  1.25 |    0.02 | 0.0343 |     288 B |        1.71 |

> ✅ MDiator is ~33% faster and uses ~3× less memory compared to MediatR.

Tested with:
- .NET 9
- Simple `Send()` request + one handler
- Scoped resolution via Microsoft.Extensions.DependencyInjection