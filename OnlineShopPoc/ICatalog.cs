using System.Collections.Concurrent;

namespace OnlineShopPoc;

public interface ICatalog
{
    /// <summary>
    /// Получить коллекцию товаров
    /// </summary>
    /// <returns>Коллекция товаров</returns>
    ConcurrentDictionary<Guid, Product> GetProducts();

    /// <summary>
    /// Добавить товар
    /// </summary>
    /// <param name="product">Товар</param>
    void AddProduct(Product product);

    /// <summary>
    /// Удалить товар
    /// </summary>
    /// <param name="productId">Id товара</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    void DeleteProduct(Guid productId);

    /// <summary>
    /// Обновить информацию о товаре
    /// </summary>
    /// <param name="product">Товар</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    void UpdateProduct(Product newProduct);

    /// <summary>
    /// Получить товар по значению ID
    /// </summary>
    /// <param name="productId"> ID </param>
    /// <returns>Товар</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    Product GetProductById(Guid productId);

    /// <summary>
    /// Очистить каталог
    /// </summary>
    void Clear();
}