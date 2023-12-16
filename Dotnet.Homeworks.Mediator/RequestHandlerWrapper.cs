using Microsoft.Extensions.DependencyInjection;

namespace Dotnet.Homeworks.Mediator;

internal static class InternalRequestHandler
{
    public static Task<TResponse> Handle<TRequest, TResponse>(TRequest request, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        where TRequest : IRequest<TResponse>
    {
        return serviceProvider
            .GetServices<IPipelineBehavior<TRequest, TResponse>>()
            .Reverse()
            .Aggregate((RequestHandlerDelegate<TResponse>) Handler, 
                (next, pipeline) => () => pipeline.Handle(request, next, cancellationToken))();

        Task<TResponse> Handler() =>
            serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>()
                .Handle(request, cancellationToken);
    }
    
    public static Task Handle<TRequest>(TRequest request, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        where TRequest : IRequest
    {
        var handler = serviceProvider.GetRequiredService<IRequestHandler<TRequest>>();

        return handler.Handle(request, cancellationToken);
    }
}

internal class RequestHandlerWrapper<TRequest, TResponse> : IRequestHandlerWrapper<TResponse>
    where TRequest : IRequest<TResponse>
{
    public Task<TResponse> Handle(IRequest<TResponse> request, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        return InternalRequestHandler.Handle<TRequest, TResponse>((TRequest) request, serviceProvider, cancellationToken);
    }
}

internal class RequestHandlerWrapper<TRequest> : IRequestHandlerWrapper
    where TRequest : IRequest
{
    public Task Handle(dynamic request, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        return InternalRequestHandler.Handle((TRequest) request, serviceProvider, cancellationToken);
    }
}

internal class DynamicRequestHandlerWrapper<TRequest, TResponse> : IDynamicRequestHandlerWrapper
    where TRequest : IRequest<TResponse>
{
    public Task<dynamic> Handle(dynamic request, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        return (dynamic) InternalRequestHandler.Handle<TRequest, TResponse>((TRequest) request, serviceProvider, cancellationToken);
    }
}