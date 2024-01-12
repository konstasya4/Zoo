using Auto.API.GraphQL.GraphTypes;
using Auto.Core.Entities;
using Auto.Data;
using GraphQL;
using GraphQL.Types;

namespace Auto.API.GraphQL.Mutations;

public class ZooMutation: ObjectGraphType
{
    private readonly IAutoStorage _db;

    public ZooMutation(IAutoStorage db)
    {
        this._db = db;

        //animal
        Field<AnimalGraphType>(
            "createAnimal",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "code"},
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "title"}
            ),
            resolve: context =>
            {
                var code = context.GetArgument<string>("code");
                var title = context.GetArgument<string>("title");

                var animal = new Animal
                {
                    Code = code,
                    Title = title,
                };
                _db.CreateAnimal(animal);
                return animal;
            }
        );

        Field<AnimalGraphType>(
            "updateAnimal",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "code" },
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "title" }
            ),
            resolve: context =>
            {
                var code = context.GetArgument<string>("code");
                var title = context.GetArgument<string>("title");

                var animal = new Animal
                {
                    Code = code,
                    Title = title,
                };
                _db.UpdateAnimal(animal);
                return animal;
            }
        );

        Field<AnimalGraphType>(
            "deleteAnimal",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "code" },
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "title" }
            ),
            resolve: context =>
            {
                var code = context.GetArgument<string>("code");
                var title = context.GetArgument<string>("title");

                var animal = new Animal
                {
                    Code = code,
                    Title = title,
                };
                _db.DeleteAnimal(animal);
                return animal;
            }
        );

        //categories
        Field<CategoryGraphType>(
            "createCategory",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "title" },
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "code" },
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "animalCode" }
            ),
            resolve: context =>
            {
                var title = context.GetArgument<string>("title");
                var code = context.GetArgument<string>("code");
                var animalCode = context.GetArgument<string>("animalCode");

                var categoryAnimal = db.FindAnimal(animalCode);
                var category = new Category
                {
                    Title = title,
                    Code = code,
                    CategoryAnimal = categoryAnimal,
                    AnimalCode = categoryAnimal.Code
                };
                _db.CreateCategory(category);
                return category;
            }
        );

        Field<CategoryGraphType>(
           "updateCategory",
           arguments: new QueryArguments(
               new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "title" },
               new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "code" },
               new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "animalCode" }
           ),
           resolve: context =>
           {
               var title = context.GetArgument<string>("title");
               var code = context.GetArgument<string>("code");
               var animalCode = context.GetArgument<string>("animalCode");

               var categoryAnimal = db.FindAnimal(animalCode);
               var category = new Category
               {
                   Title = title,
                   Code = code,
                   CategoryAnimal = categoryAnimal,
                   AnimalCode = categoryAnimal.Code
               };
               _db.UpdateCategory(category);
               return category;
           }
       );

        Field<CategoryGraphType>(
           "deleteCategory",
           arguments: new QueryArguments(
               new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "title" },
               new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "code" },
               new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "animalCode" }
           ),
           resolve: context =>
           {
               var title = context.GetArgument<string>("title");
               var code = context.GetArgument<string>("code");
               var animalCode = context.GetArgument<string>("animalCode");

               var categoryAnimal = db.FindAnimal(animalCode);
               var category = new Category
               {
                   Title = title,
                   Code = code,
                   CategoryAnimal = categoryAnimal,
                   AnimalCode = categoryAnimal.Code
               };
               _db.DeleteCategory(category);
               return category;
           }
       );
        //products
        Field<ProductGraphType>(
            "createProduct",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "serial" },
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "title" },
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "price" },
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "categoryCode" }
            ),
            resolve: context =>
            {
                var serial = context.GetArgument<string>("serial");
                var title = context.GetArgument<string>("title");
                var price = context.GetArgument<string>("price");
                var categoryCode = context.GetArgument<string>("categoryCode");

                var productCategory = db.FindCategory(categoryCode);
                var product = new Product
                {
                    Serial = serial,
                    Title = title,
                    Price = price,
                    ProductCategory = productCategory,
                    //CategoryCode = productCategory.Code
                };
                _db.CreateProduct(product);
                return product;
            }
        );

        Field<ProductGraphType>(
            "updateProduct",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "serial" },
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "title" },
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "price" },
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "categoryCode" }
            ),
            resolve: context =>
            {
                var serial = context.GetArgument<string>("serial");
                var title = context.GetArgument<string>("title");
                var price = context.GetArgument<string>("price");
                var categoryCode = context.GetArgument<string>("categoryCode");

                var productCategory = db.FindCategory(categoryCode);
                var product = new Product
                {
                    Serial = serial,
                    Title = title,
                    Price = price,
                    ProductCategory = productCategory,
                    CategoryCode = productCategory.Code
                };
                _db.UpdateProduct(product);
                return product;
            }
        );

        Field<ProductGraphType>(
            "deleteProduct",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "serial" },
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "title" },
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "price" },
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "categoryCode" }
            ),
            resolve: context =>
            {
                var serial = context.GetArgument<string>("serial");
                var title = context.GetArgument<string>("title");
                var price = context.GetArgument<string>("price");
                var categoryCode = context.GetArgument<string>("categoryCode");

                var productCategory = db.FindCategory(categoryCode);
                var product = new Product
                {
                    Serial = serial,
                    Title = title,
                    Price = price,
                    ProductCategory = productCategory,
                    CategoryCode = productCategory.Code
                };
                _db.DeleteProduct(product);
                return product;
            }
        );
    }
}