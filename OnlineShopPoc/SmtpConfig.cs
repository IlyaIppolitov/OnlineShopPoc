using System.ComponentModel.DataAnnotations;

namespace OnlineShopPoc;

public class SmtpConfig
{
    // [Required, RegularExpression("")] public string Host { get; set; }
    [Required] public string Host { get; set; }
    
    [EmailAddress, Required] public string UserName { get; set; }
    
    [Required] public string Password { get; set; }
    
    [Range(1, ushort.MaxValue)] public int Port { get; set; }
}