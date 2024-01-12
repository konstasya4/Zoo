using System.ComponentModel;
using System.Dynamic;
using Auto.Core.Entities;
using Auto.Messages;

namespace Auto.API;
public static class HAL
{
    public static dynamic PaginateAsDynamic(string baseUrl, int index, int count, int total)
    {
        dynamic links = new ExpandoObject();
        links.self = new { href = $"{baseUrl}" };
        if (index < total)
        {
            links.next = new { href = $"{baseUrl}?index={index + count}" };
            links.final = new { href = $"{baseUrl}?index={total - (total % count)}&count={count}" };
        }
        if (index > 0)
        {
            links.prev = new { href = $"{baseUrl}?index={index - count}" };
            links.first = new { href = $"{baseUrl}?index=0" };
        }
        return links;
    }

    //public static Dictionary<string, object> PaginateAsDictionary(string baseUrl, int index, int count, int total) {
    //    var links = new Dictionary<string, object>();
    //    links.Add("self", new { href = $"{baseUrl}" });
    //    if (index < total) {
    //        links["next"] = new { href = $"{baseUrl} ?index={index + count}" };
    //        links["final"] = new { href = $"{baseUrl}?index={total - (total % count)}&count={count}" };
    //    }
    //    if (index > 0) {
    //        links["prev"] = new { href = $"{baseUrl} ?index={index - count}" };
    //        links["first"] = new { href = $"{baseUrl}?index=0" };
    //    }
    //    return links;
    //}

    public static dynamic ToResource(this Product product)
    {
        var resource = product.ToDynamic();
        resource._links = new
        {
            self = new
            {
                href = $"/api/products/{product.Serial}"
            },
            category = new
            {
                href = $"/api/categories/{product.CategoryCode}"
            }
        };
        return resource;
    }

    public static dynamic ToResource(this Category category)
    {
        var resource = category.ToDynamic();
        resource._links = new
        {
            self = new
            {
                href = $"/api/categories/{category.Code}"
            },
            animal = new
            {
                href = $"/api/animals/{category.AnimalCode}"
            }
        };
        return resource;
    }
    public static dynamic ToResource(this Animal animal)
    {
        var resource = animal.ToDynamic();
        resource._links = new
        {
            self = new
            {
                href = $"/api/animals/{animal.Code}"
            },
        };
        return resource;
    }


    public static dynamic ToDynamic(this object value)
    {
        IDictionary<string, object> result = new ExpandoObject();
        var properties = TypeDescriptor.GetProperties(value.GetType());
        foreach (PropertyDescriptor prop in properties)
        {
            if (Ignore(prop)) continue;
            result.Add(prop.Name, prop.GetValue(value));
        }
        return result;
    }

    private static bool Ignore(PropertyDescriptor prop)
    {
        return prop.Attributes.OfType<System.Text.Json.Serialization.JsonIgnoreAttribute>().Any();
    }

    public static NewProductMessage ToMessage(this Product product)
    {
        var message = new NewProductMessage()
        {
            Serial = product.Serial,
            Title = product.Title,
            AnimalTitle = product?.ProductCategory?.CategoryAnimal?.Title,
            CategoryTitle = product?.ProductCategory?.Title,
            Price = product.Price,
        };
        return message;
    }
}