using Auto.Core.Entities;
using GraphQL.Types;

namespace Auto.API.GraphQL.GraphTypes;

public sealed class ProductGraphType : ObjectGraphType<Product> {
    public ProductGraphType() {
        Name = "product";
        Field(c => c.ProductCategory, nullable: false, type: typeof(CategoryGraphType))
            .Description("The category of this product");
        Field(c => c.Serial);
        Field(c => c.Title);
        Field(c => c.Price);

    }
}