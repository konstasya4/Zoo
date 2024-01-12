using System.Text.Json.Serialization;

namespace Auto.Core.Entities;

public class Category
{
    public Category() {
        Products = new HashSet<Product>();
    }

    public string Title { get; set; }
    public string Code { get; set; }
    public string AnimalCode { get; set; }

    public virtual Animal CategoryAnimal { get; set; }
    [JsonIgnore]
    public virtual ICollection<Product> Products { get; set; }
}