using System.ComponentModel.DataAnnotations;

namespace OnlineShopPoc;

public class SentryConfig
{
    [Required] public string Dsn { get; set; }
    
    [Required] public string MinimumBreadcrumbLevel { get; set; }
    
    [Required] public string MinimumEventLevel { get; set; }
    
    [Required] public int MaxBreadcrumbs { get; set; }
    
}