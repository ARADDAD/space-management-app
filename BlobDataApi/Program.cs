using BlobDataApi.Models;
using BlobDataApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Blob Storage configuration
builder.Services.Configure<BlobStorageConfig>(builder.Configuration.GetSection("BlobStorage"));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<BlobStorageService>();

// Configure CORS to allow localhost:3000 (React app)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", builder =>
    {
        builder.WithOrigins("http://localhost:3000")  // Allow requests from React app
               .AllowAnyMethod()  // Allow any HTTP method (GET, POST, etc.)
               .AllowAnyHeader()  // Allow any headers
               .AllowCredentials(); // Allow cookies, authorization headers, etc.
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable CORS middleware
app.UseCors("AllowLocalhost");  // Use the CORS policy

app.UseAuthorization();

app.MapControllers();

app.Run();
