<<<<<<< HEAD
﻿namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(string Name,
                                   List<string> Category,
                                   string Description,
                                   string ImageFile,
                                   decimal Price) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

internal class CreateProductCommandHandler (IDocumentSession session)
=======
﻿using BuildingBlocks.CQRS;
using Catalog.API.Models;

namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
    : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

internal class CreateProductCommandHandler 
>>>>>>> origin/main
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // create Product entity from command object
        // save to db
<<<<<<< HEAD
        // return the result

        // create Product 
=======
        // return CreateProductResult result

>>>>>>> origin/main
        var product = new Product
        {
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price
        };

<<<<<<< HEAD
        // save to db
        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);

        //return result
        return new CreateProductResult(product.Id);
=======
        //TODO
        //save to Db
        //return  result
                
        return new CreateProductResult(Guid.NewGuid());
        //throw new NotImplementedException();
>>>>>>> origin/main
    }
}

