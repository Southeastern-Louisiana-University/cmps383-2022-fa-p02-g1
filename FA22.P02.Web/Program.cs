using static FA22.P02.Web.Features.Products;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/products", () =>
{
    return Products;
})
.WithName("GetAll");

app.MapGet("/api/products/{id}", (int id) =>
{
    if (Products.Where(p => p.Id == id).Any())
    {
        var productsToShow = Products.First(p => p.Id == id);

        return Results.Ok(productsToShow);
    }
    else
    {
        return Results.NotFound();
    }
})
.WithName("GetById");

app.MapPost("/api/products", (ProductDto product) =>
{
    if (!Products.Where(p => p.Id == product.Id).Any() && product.Name != null && product.Name.Length < 120
        && product.Description != null && product.Price != null && product.Price > 0)
    {
        product.Id = SetIdForProduct();
        Products.Add(product);

        return Results.Created($"http://localhost/api/products/{product.Id}", product);
    }
    else
    {
        return Results.BadRequest();
    }
})
.WithName("POST");

app.Run();


//see: https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0
// Hi 383 - this is added so we can test our web project automatically. More on that later
public partial class Program { }