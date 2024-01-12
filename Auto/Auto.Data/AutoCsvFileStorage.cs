using System.Reflection;
using Auto.Core.Entities;
using static System.Int32;
using Microsoft.Extensions.Logging;

namespace Auto.Data;

public class AutoCsvFileStorage : IAutoStorage
{
    private static readonly IEqualityComparer<string> collation = StringComparer.OrdinalIgnoreCase;

    private readonly Dictionary<string, Animal> animals = new Dictionary<string, Animal>(collation);
    private readonly Dictionary<string, Category> categories = new Dictionary<string, Category>(collation);
    private readonly Dictionary<string, Product> products = new Dictionary<string, Product>(collation);
    private readonly ILogger<AutoCsvFileStorage> logger;

    public AutoCsvFileStorage(ILogger<AutoCsvFileStorage> logger)
    {
        this.logger = logger;
        ReadAnimalsFromCsvFile("animals.csv");
        ReadCategoriesFromCsvFile("categories.csv");
        ReadProductsFromCsvFile("products.csv");
        ResolveReferences();
    }

    private void ResolveReferences()
    {
        foreach (var animal in animals.Values)
        {
            animal.Categories = categories.Values.Where(m => m.AnimalCode == animal.Code).ToList();
            foreach (var category in animal.Categories) category.CategoryAnimal = animal;
        }

        foreach (var category in categories.Values)
        {
            category.Products = products.Values.Where(v => v.CategoryCode == category.Code).ToList();
            foreach (var product in category.Products) product.ProductCategory = category;
        }
    }

    private string ResolveCsvFilePath(string filename)
    {
        var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var csvFilePath = Path.Combine(directory, "csv-data");
        return Path.Combine(csvFilePath, filename);
    }

    private void ReadProductsFromCsvFile(string filename)
    {
        var filePath = ResolveCsvFilePath(filename);
        foreach (var line in File.ReadAllLines(filePath))
        {
            var tokens = line.Split(",");
            var product = new Product
            {
                Serial = tokens[0],
                CategoryCode = tokens[1],
                Title = tokens[2],
                Price = tokens[3]
            };
            products[product.Serial] = product;
        }
        logger.LogInformation($"Loaded {products.Count} products from {filePath}");
    }

    private void ReadCategoriesFromCsvFile(string filename)
    {
        var filePath = ResolveCsvFilePath(filename);
        foreach (var line in File.ReadAllLines(filePath))
        {
            var tokens = line.Split(",");
            var category = new Category
            {
                Title = tokens[0],
                Code = tokens[1],
                AnimalCode = tokens[2]
            };
            categories.Add(category.Code, category);
        }
        logger.LogInformation($"Loaded {categories.Count} categories from {filePath}");
    }

    private void ReadAnimalsFromCsvFile(string filename)
    {
        var filePath = ResolveCsvFilePath(filename);
        foreach (var line in File.ReadAllLines(filePath))
        {
            var tokens = line.Split(",");
            var animal = new Animal
            {
                Code = tokens[0],
                Title = tokens[1]
            };
            animals.Add(animal.Code, animal);
        }
        logger.LogInformation($"Loaded {animals.Count} animals from {filePath}");
    }

    public int CountProducts() => products.Count;
    public int CountCategories() => categories.Count;
    public int CountAnimals() => animals.Count;

    public IEnumerable<Product> ListProducts() => products.Values;

    public IEnumerable<Animal> ListAnimals() => animals.Values;

    public IEnumerable<Category> ListCategories() => categories.Values;

    public Product FindProduct(string serial) => products.GetValueOrDefault(serial);

    public Category FindCategory(string code) => categories.GetValueOrDefault(code);

    public Animal FindAnimal(string code) => animals.GetValueOrDefault(code);

    public void CreateProduct(Product product)
    {
        product.ProductCategory.Products.Add(product);
        product.CategoryCode = product.ProductCategory.Code;
        UpdateProduct(product);
    }

    public void UpdateProduct(Product product)
    {
        products[product.Serial] = product;
    }

    public void DeleteProduct(Product product)
    {
        var category = FindCategory(product.CategoryCode);
        category.Products.Remove(product);
        products.Remove(product.Serial);
    }
    public void CreateCategory(Category category)
    {
        category.CategoryAnimal.Categories.Add(category);
        category.AnimalCode = category.CategoryAnimal.Code;
        UpdateCategory(category);
    }

    public void UpdateCategory(Category category)
    {
        categories[category.Code] = category;

    }

    public void DeleteCategory(Category category)
    {
        var animal = FindAnimal(category.AnimalCode);
        animal.Categories.Remove(category);
        categories.Remove(category.Code);
    }
    public void CreateAnimal(Animal animal)
    {
        UpdateAnimal(animal);
    }

    public void UpdateAnimal(Animal animal)
    {
        animals[animal.Code] = animal;

    }

    public void DeleteAnimal(Animal animal)
    {
        animals.Remove(animal.Code);
    }
}