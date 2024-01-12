using Auto.API.GraphQL.Mutations;
using Auto.API.GraphQL.Queries;
using Auto.Data;
using GraphQL.Types;

namespace Auto.API.GraphQL.Schemas;

public class AutoSchema : Schema {
    public AutoSchema(IAutoStorage db)
    {
         Query = new ZooQuery(db);
         Mutation = new ZooMutation(db);
    }
}