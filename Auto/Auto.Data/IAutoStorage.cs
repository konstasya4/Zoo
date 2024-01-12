using Auto.Core.Entities;

namespace Auto.Data;

public interface IAutoStorage
{
    public int CountProducts();
    public int CountCategories();
    public int CountAnimals();
    public IEnumerable<Product> ListProducts();
    public IEnumerable<Animal> ListAnimals();
    public IEnumerable<Category> ListCategories();

    public Product FindProduct(string serial);
    public Category FindCategory(string code);
    public Animal FindAnimal(string code);

    public void CreateProduct(Product product);
    public void UpdateProduct(Product product);
    public void DeleteProduct(Product product);
    public void CreateCategory(Category category);
    public void UpdateCategory(Category category);
    public void DeleteCategory(Category category);
    public void CreateAnimal(Animal animal);
    public void UpdateAnimal(Animal animal);
    public void DeleteAnimal(Animal animal);
}