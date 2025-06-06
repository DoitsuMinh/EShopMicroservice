﻿
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Products.DeleteProduct;

public record class DeleteProductRequest(long Id);

public record DeleteProductResponse(bool IsSuccess);

public class DeleteProductEndpoint() : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products", async ([FromBody] DeleteProductRequest request, ISender sender) =>
        {
            var command = request.Adapt<DeleteProductCommand>();

            var result = await sender.Send(command);

            var response = result.Adapt<DeleteProductResponse>();

            return Results.Ok(response);
        })
      .WithName("DeleteProduct")
      .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
      .ProducesProblem(StatusCodes.Status400BadRequest)
      .ProducesProblem(StatusCodes.Status404NotFound)
      .WithSummary("Delete Product")
      .WithDescription("Delete Product");
    }
}