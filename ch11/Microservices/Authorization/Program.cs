using Authorization.Controllers;
using Microsoft.IdentityModel.Tokens; // Importing the Microsoft.IdentityModel.Tokens namespace
using Microsoft.OpenApi.Models; // Importing the Microsoft.OpenApi.Models namespace

var builder = WebApplication.CreateBuilder(args); // Creating a new WebApplication builder
builder.Services.AddControllers(); // Adding controllers to the services collection
builder.Services.AddAuthentication("Bearer") // Adding authentication services with Bearer scheme
    .AddJwtBearer(jwtOptions => // Configuring JWT Bearer options
    {
        jwtOptions.RequireHttpsMetadata = false; // Disabling HTTPS metadata requirement
        jwtOptions.Authority = "Chapter10.Authority"; // Setting the authority for token validation
        jwtOptions.Audience = "Chapter10.Audience"; // Setting the audience for token validation
        jwtOptions.TokenValidationParameters = new TokenValidationParameters // Configuring token validation parameters
        {
            ValidateIssuerSigningKey = true, // Enabling issuer signing key validation
            ValidateIssuer = true, // Enabling issuer validation
            ValidIssuer = "Chapter10.Issuer", // Setting the valid issuer
            IssuerSigningKey = JwtSecurityKey.Create() // Setting the issuer signing key
        };

        jwtOptions.MapInboundClaims = false; // Disabling inbound claims mapping
    });

builder.Services.AddSwaggerGen(swaggerGenOptions => // Adding Swagger generation services
{
    swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo { Title = "Authorization API", Version = "v1" }); // Configuring Swagger document
    swaggerGenOptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme // Adding security definition for Bearer scheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below. Example: 'Bearer 12345abcdef'", // Setting the description for the security scheme
        Name = "Authorization", // Setting the name of the authorization header
        In = ParameterLocation.Header, // Setting the location of the authorization header
        Type = SecuritySchemeType.ApiKey, // Setting the type of the security scheme
        Scheme = "Bearer" // Setting the scheme name
    });
    swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement() // Adding security requirements
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme, // Setting the reference type to security scheme
                    Id = "Bearer" // Setting the ID of the security scheme
                },
                Scheme = "oauth2", // Setting the scheme to OAuth2
                Name = "Bearer", // Setting the name of the scheme
                In = ParameterLocation.Header, // Setting the location of the scheme
            },
            new List<string>() // Adding an empty list of scopes
        }
    });
});

var app = builder.Build(); // Building the application

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) // Checking if the environment is development
{
    app.UseSwagger(); // Enabling Swagger middleware
    app.UseSwaggerUI(); // Enabling Swagger UI middleware
}

app.UseHttpsRedirection(); // Enabling HTTPS redirection middleware

app.UseAuthorization(); // Enabling authorization middleware

app.MapControllers(); // Mapping controller routes

app.Run(); // Running the application
