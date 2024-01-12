using Auto.Core.Entities;
using GraphQL.Types;

namespace Auto.API.GraphQL.GraphTypes;

public sealed class AnimalGraphType : ObjectGraphType<Animal> {
    public AnimalGraphType() {
        Name = "animal";
        Field(c => c.Title).Description("The name of the animals, e.g. cat, dog, turtle");
    }
}