using Scalar.AspNetCore;
using FinShark.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using FinShark.Interface;
using FinShark.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(optionsAction =>
{
    optionsAction.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IStockRepo, StockRepo>();
builder.Services.AddScoped<ICommentRepo, CommentRepo>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(
    options =>
    {
        options.AddPolicy("CorsPolicy",
            builder =>
            {
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            }
        );
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(
        option =>
        {
            option.DarkMode = true;
            option.HideModels = false;
            option.Title = "FinShark API Doc ";
        }
    );
}
app.UseCors("CorsPolicy");

app.UseHttpsRedirection();
app.MapGet("/", () =>
{
    return "Hello World!!!";
});
app.MapControllers();
app.Run();


