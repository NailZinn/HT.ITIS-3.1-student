namespace Dotnet.Homeworks.Mediator;

internal interface IRequestHandlerWrapper<TResponse>
{
    public Task<TResponse> Handle(IRequest<TResponse> request, IServiceProvider serviceProvider, CancellationToken cancellationToken);   
}

internal interface IRequestHandlerWrapper
{
    public Task Handle(dynamic request, IServiceProvider serviceProvider, CancellationToken cancellationToken); 
}

internal interface IDynamicRequestHandlerWrapper
{
    public Task<dynamic> Handle(dynamic request, IServiceProvider serviceProvider, CancellationToken cancellationToken);
}
