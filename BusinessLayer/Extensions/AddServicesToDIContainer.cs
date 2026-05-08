using BusinessLayer.Services;
using BusinessLayer.Validators;
using Domain.DTOs.CategoryDTOs;
using Domain.DTOs.ProductDTOs;
using Domain.DTOs.UserDTOs;
using Domain.Interfaces.Services_Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Extensions
{
    public static class AddServicesToDIContainer
    {
       public static  IServiceCollection Applications(this IServiceCollection services)
        {
            services.AddScoped<ICategoryServices, CategoryService>();
           services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            return services;
        }


        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
           services.AddScoped<IValidator<AddCategoryDTO>, AddCategoryValidator>();
           services.AddScoped<IValidator<AddProductDTO>, AddProductValidator>();
           services.AddScoped<IValidator<UpdateCategoryDTO>, UpdateCategoryValidator>();
           services.AddScoped<IValidator<UpdateProductDTO>, UpdateProductValidator>();
           services.AddScoped<IValidator<AddUserDTO>, AddUserValidator>();
            return services;
        }
    }


  
}
