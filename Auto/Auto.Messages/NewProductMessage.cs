namespace Auto.Messages;

public class NewProductMessage
{
    public string Serial { get; set; }
    public string CategoryTitle { get; set; }
    public string AnimalTitle { get; set; }
    public string Title { get; set; }
    public string Price { get; set; }
}

public class NewProductNameMessage : NewProductMessage
{
    public string Name { get; set; }
    public string WeightCode { get; set; }

    public NewProductNameMessage()
    {
    }

    public NewProductNameMessage(NewProductMessage product, string name, string weightCode)
    {
        this.Serial = product.Serial;
        this.Title = product.Title;
        this.Price = product.Price;
        this.CategoryTitle = product.CategoryTitle;
        this.AnimalTitle = product.AnimalTitle;
        this.Name = name;
        this.WeightCode = weightCode;
    }
}
