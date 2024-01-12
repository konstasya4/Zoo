using System.Text.Json.Serialization;

namespace Auto.Core.Entities;

public class Animal
{
    public Animal() {
        Categories = new HashSet<Category>();
    }


    public string Code { get; set; }
    public string Title { get; set; }


    [JsonIgnore]
    public virtual ICollection<Category> Categories { get; set; }
}