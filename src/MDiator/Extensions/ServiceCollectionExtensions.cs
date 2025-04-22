using MDiator;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMDiator(this IServiceCollection services, params Assembly[] assemblies)
        {
            // Register the core mediator
            services.AddScoped<IMediator, Mediator>();

            var handlerInterfaceType = typeof(IMDiatorHandler<,>);
            var eventHandlerInterfaceType = typeof(IMDiatorEventHandler<>);

            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();

                foreach (var type in types.Where(t => t.IsClass && !t.IsAbstract))
                {
                    var interfaces = type.GetInterfaces();

                    foreach (var iface in interfaces)
                    {
                        if (!iface.IsGenericType) continue;

                        var definition = iface.GetGenericTypeDefinition();

                        if (definition == handlerInterfaceType || definition == eventHandlerInterfaceType)
                        {
                            services.AddScoped(iface, type);
                        }
                    }
                }
            }

            return services;
        }
    }
}