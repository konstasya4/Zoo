using Auto.Core.Entities;
using GraphQL.Types;

namespace Auto.API.GraphQL.GraphTypes;

public sealed class CategoryGraphType : ObjectGraphType<Category> {
    public CategoryGraphType() {
        Name = "category";
        Field(m => m.Title).Description("The name of this category, e.g. food, medicine, clothes");
        Field(m => m.CategoryAnimal, nullable: false, type: typeof(AnimalGraphType)).Description("Categories intended for this animal");
    }
}