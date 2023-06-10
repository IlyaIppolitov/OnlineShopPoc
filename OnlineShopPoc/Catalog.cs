namespace OnlineShopPoc;

public class Catalog
{

    /// <summary>
    /// Получить коллекцию товаров
    /// </summary>
    /// <returns>Коллекция товаров</returns>
    public List<Product> GetProducts()
    {
        return _products;
    }

    /// <summary>
    /// Добавить товар
    /// </summary>
    /// <param name="product">Товар</param>
    public void AddProduct(Product product)
    {
        _products.Add(product);
    }
    
    /// <summary>
    /// Заполнение перечня товаров по умолчанию
    /// </summary>
    private List<Product> _products = GenerateProducts(5);
    
    static List<Product> GenerateProducts(int count)
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
        var products = new Product[count];

        for (int i = 0; i < count; i++)
        {
            var name = productNames[i];
            var price = random.Next(50, 500);
            var producedAt = DateTime.Now.AddDays(-random.Next(1, 30));
            var expiredAt = producedAt.AddDays(random.Next(1, 365));
            var stock = random.NextDouble() * 100;

            products[i] = new Product(name, price)
            {
                Description = "Описание " + name,
                ProducedAt = producedAt,
                ExpiredAt = expiredAt,
                Stock = stock
            };
        }
        
        return products.ToList();
    }
    
}