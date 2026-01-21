using Microsoft.EntityFrameworkCore;
using Online_Store_API.Data;
using Online_Store_API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("OnlineStoreDb"));

// Register ProductService
builder.Services.AddSingleton<IProductService, ProductService>();

// Register CartService
builder.Services.AddSingleton<ICartService, CartService>();

// Register OrderService
builder.Services.AddScoped<IOrderService, OrderService>();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMVC",
        policy =>
        {
            policy.WithOrigins("https://localhost:7233", "http://localhost:5062") // Updated ports
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowMVC");

app.UseAuthorization();

app.MapControllers();

app.Run();

