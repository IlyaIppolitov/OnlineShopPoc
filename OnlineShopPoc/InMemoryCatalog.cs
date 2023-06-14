using System.Collections.Concurrent;

namespace OnlineShopPoc;

public class InMemoryCatalog : ICatalog
{

    /// <summary>
    /// Получить коллекцию товаров
    /// </summary>
    /// <returns>Коллекция товаров</returns>
    public ConcurrentDictionary<Guid, Product> GetProducts()
    {
        return _products;
    }

    /// <summary>
    /// Добавить товар
    /// </summary>
    /// <param name="product">Товар</param>
    public void AddProduct(Product product)
    {
        if (!_products.TryAdd(product.Id, product))
            throw new ArgumentException($"Product with ID {product.Id} already exists");
    }

    /// <summary>
    /// Удалить товар
    /// </summary>
    /// <param name="productId">Id товара</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void DeleteProduct(Guid productId)
    {
        if (!_products.TryRemove(productId, out var ignored))
        {
            throw new ArgumentOutOfRangeException($"Product with ID: {productId} not found");
        }
    }

    /// <summary>
    /// Обновить информацию о товаре
    /// </summary>
    /// <param name="product">Товар</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void UpdateProduct(Product newProduct)
    {
        if (!_products.TryGetValue(newProduct.Id, out var oldProduct))
        {
            throw new ArgumentOutOfRangeException($"Product with ID: {newProduct.Id} not found");
        }

        if(_products.TryUpdate(newProduct.Id, newProduct, oldProduct))
        {
            throw new ArgumentOutOfRangeException($"Product with ID: {newProduct.Id} not found");
        }
        
    }

    /// <summary>
    /// Получить товар по значению ID
    /// </summary>
    /// <param name="productId"> ID </param>
    /// <returns>Товар</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public Product GetProductById(Guid productId)
    {
        if (_products.TryGetValue(productId, out var product))
        {
            return product;
        }
        throw new ArgumentOutOfRangeException($"Product with ID: {productId} not found");
    }

    /// <summary>
    /// Очистить каталог
    /// </summary>
    public void Clear()
    {
        _products.Clear();
    }

    /// <summary>
    /// Заполнение перечня товаров по умолчанию
    /// </summary>
    private ConcurrentDictionary<Guid, Product> _products = GenerateProducts(5);
    
    static ConcurrentDictionary<Guid, Product> GenerateProducts(int count)
    {

        // Массив реальных названий товаров
        var productNames = new string[]
        {
            "Молоко",
            "Хлеб",
            "Яблоки",
            "Макароны",
            "Сахар",
            "Кофе",
            "Чай",
            "Рис",
            "Масло подсолнечное",
            "Сыр"
        };

        // Проверка введеного количества товаров на соответствие с количеством имеющихся имён
        if (count > productNames.Length)
            throw new ArgumentOutOfRangeException(
                $"Wrong number of products, select less then {productNames.Length}");
        
        var random = new Random();
        var products = new ConcurrentDictionary<Guid, Product>();

        for (int i = 0; i < count; i++)
        {
            var name = productNames[i];
            var price = random.Next(50, 500);
            var producedAt = DateTime.Now.AddDays(-random.Next(1, 30));
            var expiredAt = producedAt.AddDays(random.Next(1, 365));
            var stock = random.NextDouble() * 100;

            var product = new Product(name, price);
            product.Description = "Описание " + name;
            product.ProducedAt = producedAt;
            product.ExpiredAt = expiredAt;
            product.Stock = stock;
            products.TryAdd(product.Id, product);
        }
        
        return products;
    }
    
}