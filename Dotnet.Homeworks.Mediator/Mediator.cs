using System.Collections.Concurrent;

namespace Dotnet.Homeworks.Mediator;

public class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    private static readonly Type GenericRequest = typeof(IRequest<>);
    private static readonly Type NonGenericRequest = typeof(IRequest);
    private static readonly Type RequestHandlerWrapperWithResponse = typeof(RequestHandlerWrapper<,>);
    private static readonly Type RequestHandlerWrapperWithoutResponse = typeof(RequestHandlerWrapper<>);
    private static readonly Type DynamicRequestHandlerWrapperWithResponse = typeof(DynamicRequestHandlerWrapper<,>);

    private static readonly ConcurrentDictionary<Type, IRequestHandlerWrapperBase> HandlerWrappers = new();
    
    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var handlerWrapper = (IRequestHandlerWrapper<TResponse>) HandlerWrappers.GetOrAdd(request.GetType(), static requestType =>
        {
            var wrapperType = RequestHandlerWrapperWithResponse.MakeGenericType(requestType, typeof(TResponse));
            var wrapper = Activator.CreateInstance(wrapperType)!;
            return (IRequestHandlerWrapperBase) wrapper;
        });
    
        return handlerWrapper.Handle(request, _serviceProvider, cancellationToken);
    }
    
    public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
    {
        return InternalRequestHandler.Handle(request, _serviceProvider, cancellationToken);
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
            var handlerWrapper = (IDynamicRequestHandlerWrapper) HandlerWrappers.GetOrAdd(someRequest, static requestType =>
            {
                var wrapperType = DynamicRequestHandlerWrapperWithResponse.MakeGenericType(
                    requestType, requestType.GetGenericArguments()[0]);
                var wrapper = Activator.CreateInstance(wrapperType);
                return (IRequestHandlerWrapperBase) wrapper!;
            });
            
            return handlerWrapper.Handle(request, _serviceProvider, cancellationToken);
        }
        else
        {
            var handlerWrapper = (IRequestHandlerWrapper)HandlerWrappers.GetOrAdd(someRequest, static requestType =>
            {
                var wrapperType = RequestHandlerWrapperWithoutResponse.MakeGenericType(requestType);
                var wrapper = Activator.CreateInstance(wrapperType);
                return (IRequestHandlerWrapperBase)wrapper!;
            });
            
            return handlerWrapper.Handle(request, _serviceProvider, cancellationToken);
        }
    }
}