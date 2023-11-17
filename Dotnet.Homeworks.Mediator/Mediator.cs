using Microsoft.Extensions.DependencyInjection;

namespace Dotnet.Homeworks.Mediator;

public class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    private static readonly Type GenericRequest = typeof(IRequest<>);
    private static readonly Type NonGenericRequest = typeof(IRequest);
    private static readonly Type RequestHandlerWrapperWithResponse = typeof(RequestHandlerWrapper<,>);
    private static readonly Type RequestHandlerWrapperWithoutResponse = typeof(RequestHandlerWrapper<>);
    private static readonly Type DynamicRequestHandlerWrapperWithResponse = typeof(DynamicRequestHandlerWrapper<,>);
    
    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var wrapperType = RequestHandlerWrapperWithResponse.MakeGenericType(request.GetType(), typeof(TResponse));
        var wrapper = (IRequestHandlerWrapper<TResponse>) Activator.CreateInstance(wrapperType)!;
    
        return wrapper.Handle(request, _serviceProvider, cancellationToken);
    }
    
    public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
    {
        var handler = _serviceProvider.GetRequiredService<IRequestHandler<TRequest>>();
    
        return handler.Handle(request, cancellationToken);
    }

    public Task<dynamic?> Send(dynamic request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        var interfaces = requestType.GetInterfaces();
        
        var someRequest = ((Type[])interfaces)
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == GenericRequest || i == NonGenericRequest);

        if (someRequest is null)
        {
            return Task.FromResult<dynamic?>(null);
        }
        
        if (someRequest.IsGenericType)
        {
            var wrapperType = DynamicRequestHandlerWrapperWithResponse.MakeGenericType(
                requestType, someRequest.GetGenericArguments()[0]);
            var wrapper = (IDynamicRequestHandlerWrapper) Activator.CreateInstance(wrapperType);
            return wrapper.Handle(request, _serviceProvider, cancellationToken);
        }
        else
        {
            var wrapperType = RequestHandlerWrapperWithoutResponse.MakeGenericType(requestType);
            var wrapper = (IRequestHandlerWrapper) Activator.CreateInstance(wrapperType);
            return wrapper.Handle(request, _serviceProvider, cancellationToken);
        }
    }
}