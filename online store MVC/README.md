# Online Store - ASP.NET Core MVC

A fully functional online store built with ASP.NET Core MVC and Web API.

## Project Structure

This solution contains two projects:

1. **Online Store API** - Web API that provides product data
2. **online store MVC** - MVC application that displays products to users

## Features Implemented

### ? Home Page
- Displays all products in a responsive grid layout
- Shows product image, name, category, and price
- "View Details" button for each product
- Hover effects on product cards
- Loads data from Web API

### ? Product Details Page
- Comprehensive product information
- Product image, name, description, price
- Stock availability indicator
- "Add to Cart" button (ready for shopping cart implementation)
- Breadcrumb navigation
- Back to store button

### ? API Features
- RESTful API with GET endpoints
- `/api/Products` - Get all products
- `/api/Products/{id}` - Get product by ID
- CORS configured for MVC application
- 8 sample products pre-loaded

## How to Run

### Step 1: Update API Port (if needed)

1. Check the API project's port by looking at `launchSettings.json` in the API project
2. Update the API URL in the MVC project's `Program.cs`:
   ```csharp
   client.BaseAddress = new Uri("https://localhost:YOUR_API_PORT/");
   ```

### Step 2: Run Both Projects

**Option A: Using Visual Studio**
1. Right-click the solution in Solution Explorer
2. Select "Configure Startup Projects"
3. Choose "Multiple startup projects"
4. Set both projects to "Start"
5. Press F5

**Option B: Using Command Line**

Terminal 1 (API):
```bash
cd "Online Store API"
dotnet run
```

Terminal 2 (MVC):
```bash
cd "online store MVC"
dotnet run
```

### Step 3: Access the Application

- Open your browser and navigate to the MVC application (typically `https://localhost:7001`)
- You should see the home page with 8 products
- Click "View Details" on any product to see the details page

## Project Architecture

### API Layer
- **Models/Product.cs** - Product data model
- **Services/ProductService.cs** - Business logic and in-memory data
- **Controllers/ProductsController.cs** - RESTful API endpoints

### MVC Layer
- **Models/Product.cs** - Product model for views
- **Controllers/HomeController.cs** - Home page with product list
- **Controllers/ProductsController.cs** - Product details page
- **Views/Home/Index.cshtml** - Product grid layout
- **Views/Products/Details.cshtml** - Product details page
- **Views/Shared/_Layout.cshtml** - Main layout with navigation

## Sample Products

The application comes with 8 pre-loaded products:
1. Wireless Headphones - $149.99
2. Smartphone - $799.99
3. Laptop - $1,299.99
4. Running Shoes - $89.99
5. Smart Watch - $249.99
6. Coffee Maker - $79.99
7. Backpack - $49.99
8. Gaming Mouse - $59.99

## Next Steps (To Be Implemented)

- [ ] Shopping Cart functionality
- [ ] Order placement system
- [ ] Admin panel for product management
- [ ] User authentication
- [ ] Database integration (currently using in-memory data)
- [ ] Search and filter functionality
- [ ] Category browsing

## Technologies Used

- ASP.NET Core 10.0
- MVC Pattern
- Web API
- Bootstrap 5
- Bootstrap Icons
- HttpClient for API calls
- Dependency Injection

## Notes

- The API uses in-memory data storage, so all changes are lost when the API restarts
- CORS is configured to allow requests from the MVC application
- Product images are loaded from Unsplash for demonstration purposes
- The shopping cart icon in the navigation is a placeholder for future implementation

## Troubleshooting

**Problem: Products not loading on home page**
- Solution: Ensure the API project is running before the MVC project
- Check that the API URL in MVC's `Program.cs` matches the API's actual port

**Problem: CORS errors**
- Solution: Verify the CORS policy in API's `Program.cs` includes your MVC application's URL

**Problem: 404 on product details**
- Solution: Ensure you're using a valid product ID (1-8)
