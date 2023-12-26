namespace Dotnet.Homeworks.Features.Orders.Shared;

public interface IHasProductIdsValidation
{
    IEnumerable<Guid> ProductIds { get; }
}