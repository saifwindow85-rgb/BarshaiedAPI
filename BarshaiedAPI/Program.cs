using BusinessLayer.DTOs.ProductDTOs;
using BusinessLayer.Services;
using BusinessLayer.Validators;
using DataAccessLayer.AppDbContext;
using DataAccessLayer.Repositories;
using DataAccessLayer.DTOs.CategoryDTOs;
using DataAccessLayer.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BarshaiedDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ICategoryRepository,CategoryRespository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IValidator<AddUpdateCategoryDTO>, CategoryValidator>();
builder.Services.AddScoped<IValidator<AddUpdateProductDTO>, ProductValidator>();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
