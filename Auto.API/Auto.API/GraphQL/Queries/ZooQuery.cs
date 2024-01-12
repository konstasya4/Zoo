using Auto.API.GraphQL.GraphTypes;
using Auto.Core.Entities;
using Auto.Data;
using GraphQL;
using GraphQL.Types;

namespace Auto.API.GraphQL.Queries;

public class ZooQuery : ObjectGraphType {
    private readonly IAutoStorage _db;

    public ZooQuery(IAutoStorage db) {
        this._db = db;

        Field<ListGraphType<AnimalGraphType>>("Animals", "Query to retrieve all Animals",
            resolve: GetAllAnimals);

        Field<AnimalGraphType>("Animal", "Query to retrieve a specific Animal",
            new QueryArguments(MakeNonNullStringArgument("code", "The code of the Animal")),
            resolve: GetAnimal);

        Field<ListGraphType<CategoryGraphType>>("Categories", "Query to retrieve all Categories",
            resolve: GetAllCategories);

        Field<CategoryGraphType>("Category", "Query to retrieve a specific Category",
            new QueryArguments(MakeNonNullStringArgument("code", "The code of the Category")),
            resolve: GetCategory);
        Field<ListGraphType<ProductGraphType>>("Products", "Query to retrieve all products",
            resolve: GetAllProducts);

        Field<ProductGraphType>("Product", "Query to retrieve a specific Product",
            new QueryArguments(MakeNonNullStringArgument("serial", "The serial number of the Product")),
            resolve: GetProduct);

        Field<ListGraphType<ProductGraphType>>("ProductsByPrice", "Query to retrieve all Products matching the specified price",
            new QueryArguments(MakeNonNullStringArgument("price", "The name of a price, eg '1000', '999'")),
            resolve: GetProductsByPrice);

    }

    private QueryArgument MakeNonNullStringArgument(string name, string description) {
        return new QueryArgument<NonNullGraphType<StringGraphType>> {
            Name = name, Description = description
        };
    }

    private IEnumerable<Animal> GetAllAnimals(IResolveFieldContext<object> context) => _db.ListAnimals();

    private Animal GetAnimal(IResolveFieldContext<object> context) {
        var code = context.GetArgument<string>("code");
        return _db.FindAnimal(code);
    }
    private IEnumerable<Category> GetAllCategories(IResolveFieldContext<object> context) => _db.ListCategories();

    private Category GetCategory(IResolveFieldContext<object> context)
    {
        var code = context.GetArgument<string>("code");
        return _db.FindCategory(code);
    }
    private IEnumerable<Product> GetAllProducts(IResolveFieldContext<object> context) => _db.ListProducts();

    private Product GetProduct(IResolveFieldContext<object> context)
    {
        var serial = context.GetArgument<string>("serial");
        return _db.FindProduct(serial);
    }

    private IEnumerable<Product> GetProductsByPrice(IResolveFieldContext<object> context)
    {
        var price = context.GetArgument<string>("price");
        var products = _db.ListProducts().Where(v => v.Price.Contains(price, StringComparison.InvariantCultureIgnoreCase));
        return products;
    }
}