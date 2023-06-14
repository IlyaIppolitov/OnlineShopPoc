using System.Text.Json.Serialization;

namespace OnlineShopPoc;

/// <summary>
/// Модель данных для товара в магазине
/// </summary>
public class Product
{
    
    /// <summary>
    /// Конструтор
    /// </summary>
    /// <param name="name"></param> Название товара
    /// <param name="price"></param> Цена
    [JsonConstructor]
    public Product(string name, decimal price)
    {
        // Валидация параметров
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
        if (price < 0) throw new ArgumentOutOfRangeException(nameof(price));

        Id = Guid.NewGuid();
        Name = name;
        Price = price;
    }

    public Product(Guid id, string name, string? description, decimal price, DateTime producedAt, DateTime expiredAt, double stock)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        ProducedAt = producedAt;
        ExpiredAt = expiredAt;
        Stock = stock;
    }

    /// <summary> ID товара </summary>
    public Guid Id { get; init; }
    
    /// <summary> Название товара </summary>
    public string Name { get; set; }
    
    /// <summary> Описание </summary>
    public string? Description { get; set; }
    
    /// <summary> Цена </summary>
    public decimal Price { get; set; }
    
    /// <summary> Дата производства </summary>
    public DateTime ProducedAt { get; set; }
    
    /// <summary> Дата окончания срока действия </summary>
    public DateTime ExpiredAt { get; set; }
    
    /// <summary> Количество товара на складе </summary>
    public double Stock { get; set; }
}