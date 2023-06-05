namespace OnlineShopPoc;

public class Catalog
{
    private List<Product> _products = GenerateProducts(5);

    public List<Product> GetProducts()
    {
        return _products;
    }

    public void AddProduct(Product product)
    {
        _products.Add(product);
    }
    
    static List<Product> GenerateProducts(int count)
    {
        var random = new Random();
        var products = new Product[count];

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