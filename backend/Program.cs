using backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";

// Add services to the container.
// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Add OpenAPI/Swagger
builder.Services.AddEndpointsApiExplorer(); // Needed for minimal APIs
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });
});

var csBuilder = new Npgsql.NpgsqlConnectionStringBuilder
{
    Host = "db.ftfwbxndgoaxyqwjqouj.supabase.co",
    Port = 5432,
    Database = "postgres",
    Username = "postgres",
    Password = "9#$saW-U!c#iz&7",
    SslMode = Npgsql.SslMode.Require
};

try
{
    using var conn = new Npgsql.NpgsqlConnection(csBuilder.ConnectionString);
    conn.Open();
    Console.WriteLine("✅ DB OK");
}
catch (Exception ex)
{
    Console.WriteLine("❌ DB FAIL: " + ex.ToString());
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(csBuilder.ConnectionString));

builder.Services.AddControllers();

var app = builder.Build();
app.Urls.Add($"http://*:{port}");
app.UseCors("AllowReactApp"); // <-- включаем CORS

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        Console.WriteLine(error?.Error);
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("Internal Server Error");
    });
});

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();
