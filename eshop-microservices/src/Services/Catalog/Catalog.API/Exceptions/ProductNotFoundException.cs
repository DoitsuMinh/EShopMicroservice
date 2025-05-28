namespace Catalog.API.Exceptions;

public class ProductNotFoundException(long id) : NotFoundException("Product", id)
{
}

