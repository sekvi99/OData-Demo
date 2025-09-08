# ASP.NET Core OData Example

This project demonstrates how to implement OData (Open Data Protocol) in ASP.NET Core, providing a RESTful API with rich querying capabilities.

## What is OData?

OData is a REST-based protocol for querying and updating data. It enables standardized API endpoints with built-in filtering, sorting, pagination, and relationship expansion capabilities without writing custom endpoints for each query combination.

## Features Implemented

- ✅ Basic CRUD operations (Create, Read, Update, Delete)
- ✅ Rich querying with $filter, $select, $orderby, $top, $skip
- ✅ Relationship expansion with $expand
- ✅ Entity count with $count
- ✅ Custom functions and actions
- ✅ Metadata discovery via $metadata endpoint
- ✅ In-memory database with sample data

## Prerequisites

- .NET 6.0 or later
- Visual Studio 2022 or Visual Studio Code
- Postman (optional, for testing)

## Getting Started

### 1. Clone and Setup

```bash
git clone <your-repository>
cd ODataExample
dotnet restore
```

### 2. Required NuGet Packages

```xml
<PackageReference Include="Microsoft.AspNetCore.OData" Version="8.2.3" />
<PackageReference Include="Microsoft.EntityFrameworkCore.InMemory" Version="7.0.0" />
```

### 3. Run the Application

```bash
dotnet run
```

The application will start on `https://localhost:7092`

### 4. Verify Setup

Open your browser and navigate to:
```
https://localhost:7092/odata/$metadata
```

You should see the OData metadata XML describing the entity model.

## Sample Data

The application seeds the following sample data:

**Categories:**
- Electronics (ID: 1)
- Books (ID: 2)

**Products:**
- Laptop ($999.99) - Electronics
- Smartphone ($599.99) - Electronics  
- Tablet ($399.99) - Electronics
- C# Programming ($49.99) - Books
- ASP.NET Core Guide ($59.99) - Books

## API Endpoints

### Base URL
```
https://localhost:7092/odata
```

### Entity Sets
- `/Products` - Product entities
- `/Categories` - Category entities
- `/$metadata` - Service metadata

## Testing with Postman

### Basic Queries

#### Get All Products
```http
GET https://localhost:7092/odata/Products
```

#### Get All Categories
```http
GET https://localhost:7092/odata/Categories
```

#### Get Single Product
```http
GET https://localhost:7092/odata/Products(1)
```

### Filtering Examples

#### Products over $500
```http
GET https://localhost:7092/odata/Products?$filter=Price gt 500
```

#### Products containing "Laptop"
```http
GET https://localhost:7092/odata/Products?$filter=contains(Name,'Laptop')
```

#### Electronics products only
```http
GET https://localhost:7092/odata/Products?$filter=CategoryId eq 1
```

#### Price range filtering
```http
GET https://localhost:7092/odata/Products?$filter=Price ge 50 and Price le 600
```

### Selection and Projection

#### Select specific fields
```http
GET https://localhost:7092/odata/Products?$select=Name,Price
```

#### Combine selection with filtering
```http
GET https://localhost:7092/odata/Products?$select=Name,Price&$filter=Price lt 100
```

### Sorting and Pagination

#### Sort by price (descending)
```http
GET https://localhost:7092/odata/Products?$orderby=Price desc
```

#### Get top 3 most expensive products
```http
GET https://localhost:7092/odata/Products?$orderby=Price desc&$top=3
```

#### Pagination (skip 1, take 2)
```http
GET https://localhost:7092/odata/Products?$top=2&$skip=1
```

### Expanding Relationships

#### Products with category details
```http
GET https://localhost:7092/odata/Products?$expand=Category
```

#### Single product with category
```http
GET https://localhost:7092/odata/Products(1)?$expand=Category
```

#### Categories with their products
```http
GET https://localhost:7092/odata/Categories?$expand=Products
```

### Counting

#### Get products with total count
```http
GET https://localhost:7092/odata/Products?$count=true
```

### Custom Functions

#### Get expensive products (over $500)
```http
GET https://localhost:7092/odata/GetExpensiveProducts
```

## CRUD Operations

### Create Product
```http
POST https://localhost:7092/odata/Products
Content-Type: application/json

{
    "Name": "Gaming Mouse",
    "Price": 79.99,
    "CategoryId": 1
}
```

### Update Product (Full Replace)
```http
PUT https://localhost:7092/odata/Products(1)
Content-Type: application/json

{
    "Id": 1,
    "Name": "Updated Laptop",
    "Price": 1199.99,
    "CategoryId": 1
}
```

### Partial Update
```http
PATCH https://localhost:7092/odata/Products(1)
Content-Type: application/json

{
    "Price": 899.99
}
```

### Delete Product
```http
DELETE https://localhost:7092/odata/Products(5)
```

### Custom Actions

#### Reset all prices to zero
```http
POST https://localhost:7092/odata/ResetPrices
```

## Response Formats

### Successful GET Response
```json
{
    "@odata.context": "https://localhost:7092/odata/$metadata#Products",
    "@odata.count": 5,
    "value": [
        {
            "Id": 1,
            "Name": "Laptop",
            "Price": 999.99,
            "CategoryId": 1,
            "Category": null
        }
    ]
}
```

### Single Entity Response
```json
{
    "@odata.context": "https://localhost:7092/odata/$metadata#Products/$entity",
    "Id": 1,
    "Name": "Laptop", 
    "Price": 999.99,
    "CategoryId": 1
}
```

### Error Response
```json
{
    "error": {
        "code": "404",
        "message": "Resource not found"
    }
}
```

## Expected HTTP Status Codes

- `200 OK` - Successful GET, PUT, PATCH
- `201 Created` - Successful POST
- `204 No Content` - Successful DELETE
- `400 Bad Request` - Invalid request data
- `404 Not Found` - Resource doesn't exist

## Advanced Testing Scenarios

### Complex Filtering
```http
GET https://localhost:7092/odata/Products?$filter=(Price gt 100 and Price lt 1000) and CategoryId eq 1
```

### Multiple Query Options
```http
GET https://localhost:7092/odata/Products?$filter=CategoryId eq 1&$orderby=Price desc&$select=Name,Price&$top=2
```

### Navigation Property Filtering
```http
GET https://localhost:7092/odata/Categories?$filter=Products/any(p: p/Price gt 500)&$expand=Products
```

## Troubleshooting

### Common Issues

1. **Port conflicts**: If 7092 is in use, check `launchSettings.json` or use `dotnet run --urls="https://localhost:XXXX"`

2. **CORS errors**: The example includes CORS configuration for development

3. **SSL certificate issues**: Use `dotnet dev-certs https --trust` to trust development certificates

### Debugging Tips

- Check `$metadata` endpoint to verify your entity model
- Use browser developer tools to inspect network requests
- Enable detailed error responses in development mode
- Check server logs for detailed error information

## Project Structure

```
/
├── Program.cs              # Application startup and OData configuration
├── Models/                 # Entity models (Product, Category)
├── Controllers/            # OData controllers
├── Services/              # Business logic services
└── Data/                  # Entity Framework DbContext
```

## Next Steps

1. Try all the Postman examples above
2. Experiment with combining different query options
3. Add validation attributes to your models
4. Implement authentication and authorization
5. Add more complex entity relationships
6. Explore OData batch operations

## Resources

- [Official OData Documentation](https://www.odata.org/)
- [ASP.NET Core OData Documentation](https://docs.microsoft.com/en-us/odata/)
- [OData Query Options Reference](https://docs.oasis-open.org/odata/odata/v4.01/odata-v4.01-part2-url-conventions.html)