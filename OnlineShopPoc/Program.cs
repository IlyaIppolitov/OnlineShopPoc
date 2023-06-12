using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using OnlineShopPoc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

Catalog catalog = new Catalog();

//RPC
app.MapGet("/get_products", GetProducts);
app.MapPost("/add_product", AddProduct);
app.MapPost("/update_product", UpdateProduct);
app.MapPost("/delete_product", DeleteProduct);
app.MapGet("/get_product", GetProductById);
app.MapPost("/clear_products", ClearProducts);

//REST
app.MapGet("/products/all", GetProducts);
app.MapPost("/products/new", AddProduct);
app.MapPut("/products/{productId}", UpdateProductById);
app.MapDelete("/products/{productId}", DeleteProduct);


void AddProduct(Product product)
{
    catalog.AddProduct(product);
}
    
List<Product> GetProducts(HttpContext context)
{
    return catalog.GetProducts();
}

Product GetProductById(string productId)
{
    return catalog.GetProductById(Guid.Parse(productId));
}

void DeleteProduct(string productId)
{
    catalog.DeleteProduct(Guid.Parse(productId));
}

void UpdateProduct(Product product)
{
    catalog.UpdateProduct(product);
}

void UpdateProductById(string productId, Product product)
{
    catalog.UpdateProduct(product);
}

void ClearProducts()
{
    catalog.Clear();
}

app.Run();