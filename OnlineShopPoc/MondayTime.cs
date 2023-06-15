namespace OnlineShopPoc;

/// <summary>
/// Генератор понедельника
/// </summary>
public class MondayTime : ICurrentTime
{
    
    /// <summary>
    /// Получить понедельник в качестве текущего времени
    /// </summary>
    /// <returns>Понедельник</returns>
    public DateTime getCurrentTime()
    {
        return new DateTime(2023, 06, 12, 00, 00, 01);
    }
}