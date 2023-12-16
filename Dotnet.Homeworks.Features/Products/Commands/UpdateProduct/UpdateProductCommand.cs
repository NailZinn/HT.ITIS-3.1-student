using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;

namespace Dotnet.Homeworks.Features.Products.Commands.UpdateProduct;

public record UpdateProductCommand(Guid Guid, string Name) : ICommand;