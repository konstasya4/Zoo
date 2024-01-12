using System.Text.Json.Serialization;

namespace Auto.Core.Entities;

public class Product
{
    public string Serial { get; set; }
    public string CategoryCode { get; set; }
    public string Title { get; set; }
    public string Price { get; set; }

    [JsonIgnore]
    public virtual Category ProductCategory { get; set; }
}