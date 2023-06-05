using OnlineShopPoc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

Catalog catalog = new Catalog();

app.MapGet("/get_products", GetProducts);
app.MapPost("/add_product", AddProduct);

void AddProduct(Product product)
{
    catalog.AddProduct(product);
}
    
List<Product> GetProducts(HttpContext context)
{
    return catalog.GetProducts();
}

app.Run();