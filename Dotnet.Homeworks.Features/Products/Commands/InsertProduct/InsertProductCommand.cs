using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;

namespace Dotnet.Homeworks.Features.Products.Commands.InsertProduct;

public record InsertProductCommand(string Name) : ICommand<InsertProductDto>;
