# ApiDog Setup Guide with Swagger Integration

## Overview
This guide walks you through setting up ApiDog with Swagger documentation for your .NET Core API project. ApiDog is a powerful API development and testing tool that can import your Swagger/OpenAPI specifications for streamlined API documentation and testing.

## Prerequisites
- .NET Core/ASP.NET Core project
- ApiDog account and application installed
- Development environment running

## Step 1: Install Required NuGet Packages

Ensure your project includes the following two essential packages:

### Required Packages:
1. **Swashbuckle.AspNetCore.Swagger** - Provides the Swagger middleware
2. **Swashbuckle.AspNetCore.SwaggerGen** - Generates OpenAPI/Swagger documentation

### Installation Methods:

#### Package Manager Console:
```bash
Install-Package Swashbuckle.AspNetCore.Swagger
Install-Package Swashbuckle.AspNetCore.SwaggerGen
```

#### .NET CLI:
```bash
dotnet add package Swashbuckle.AspNetCore.Swagger
dotnet add package Swashbuckle.AspNetCore.SwaggerGen
```

#### PackageReference (in .csproj):
```xml
<PackageReference Include="Swashbuckle.AspNetCore.Swagger" Version="6.5.0" />
<PackageReference Include="Swashbuckle.AspNetCore.SwaggerGen" Version="6.5.0" />
```

## Step 2: Configure Swagger in Your Application

Add the following configuration to your `Program.cs` or `Startup.cs`:

```csharp
// Add services
builder.Services.AddSwaggerGen();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // optional
}
```

## Step 3: Get Your Swagger JSON URL

1. **Start your application** in development mode
2. **Copy the Swagger JSON URL**: 
   ```
   https://localhost:[port]/swagger/v1/swagger.json
   ```
   
   Replace `[port]` with your actual application port (e.g., `https://localhost:5001/swagger/v1/swagger.json`)

3. **Verify the URL** by accessing it in your browser - you should see the JSON schema of your API

## Step 4: Import into ApiDog

### Import Process:

1. **Open ApiDog** application

2. **Navigate to Import Data**:
   - Look for "Import" or "Import Data" option in the main menu
   - This is typically found in the project settings or main dashboard

3. **Select Import Type**:
   - Choose **"OpenAPI/Swagger"** from the available import options
   - This tells ApiDog to expect OpenAPI 3.0 or Swagger 2.0 format

4. **Enter the URL**:
   - Paste your Swagger JSON URL into the **"URL"** section
   - Example: `https://localhost:5001/swagger/v1/swagger.json`

5. **Complete Import**:
   - Click **"Continue"** or **"Import"** button
   - ApiDog will fetch and parse your API documentation

## Step 5: Verify Import

After successful import, verify that:
- All your API endpoints are visible in ApiDog
- Request/response models are properly imported
- Authentication requirements are correctly configured
- API documentation and descriptions are displayed

## Troubleshooting

### Common Issues:

**SSL Certificate Issues:**
- If using HTTPS locally, ensure your development certificates are trusted
- Consider using HTTP for local development if SSL causes issues

**CORS Issues:**
- If importing from a different domain, ensure CORS is properly configured
- Add ApiDog's domain to your CORS policy if needed

**Port Conflicts:**
- Verify your application is running on the expected port
- Check that the port in your URL matches your application's configuration

**Empty Import:**
- Ensure your API controllers are properly decorated with attributes
- Verify Swagger is generating documentation for your endpoints

## Best Practices

1. **Use XML Comments**: Add XML documentation to your controllers and models for better API documentation
2. **Configure API Info**: Set up API title, version, and description in Swagger configuration
3. **Regular Updates**: Re-import your API specification whenever you make significant changes
4. **Environment Management**: Use different URLs for different environments (dev, staging, production)

## Next Steps

After successful import:
- Explore your API endpoints in ApiDog
- Set up test scenarios and automated testing
- Configure environment variables for different deployment stages
- Share your API documentation with team members

## Additional Resources

- [Swashbuckle.AspNetCore Documentation](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
- [OpenAPI Specification](https://swagger.io/specification/)
- [ApiDog Documentation](https://apidog.com/help/)

---

*This guide provides a foundation for integrating your .NET Core API with ApiDog through Swagger documentation. Adjust the steps based on your specific project structure and requirements.*