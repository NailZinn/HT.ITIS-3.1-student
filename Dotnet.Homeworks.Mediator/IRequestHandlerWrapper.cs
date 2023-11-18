namespace Dotnet.Homeworks.Mediator;

internal interface IRequestHandlerWrapperBase { }

internal interface IRequestHandlerWrapper<TResponse> : IRequestHandlerWrapperBase
{
    public Task<TResponse> Handle(IRequest<TResponse> request, IServiceProvider serviceProvider, CancellationToken cancellationToken);   
}

internal interface IRequestHandlerWrapper : IRequestHandlerWrapperBase
{
    public Task Handle(dynamic request, IServiceProvider serviceProvider, CancellationToken cancellationToken); 
}

internal interface IDynamicRequestHandlerWrapper : IRequestHandlerWrapperBase
{
    public Task<dynamic> Handle(dynamic request, IServiceProvider serviceProvider, CancellationToken cancellationToken);
}
