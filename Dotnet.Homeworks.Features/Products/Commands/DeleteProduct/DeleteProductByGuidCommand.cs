using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;

namespace Dotnet.Homeworks.Features.Products.Commands.DeleteProduct;

public record DeleteProductByGuidCommand(Guid Guid) : ICommand;