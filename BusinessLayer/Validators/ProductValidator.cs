using BusinessLayer.DTOs.ProductDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Validators
{
    public class ProductValidator : AbstractValidator<AddUpdateProductDTO>
    {
        public ProductValidator()
        {
            RuleFor(x => x.ProductName)
           .Transform(name => name.Trim())
           .NotEmpty().WithMessage("Product name is required")
           .MinimumLength(3).WithMessage("Minimum length is 3")
           .MaximumLength(100).WithMessage("Maximum length is 100");

            // Barcode (اختياري)
            RuleFor(x => x.Barcode)
                .Length(8, 20)
                .When(x => !string.IsNullOrEmpty(x.Barcode))
                .WithMessage("Barcode must be between 8 and 20 characters")
                .Matches(@"^\d+$")
                .When(x => !string.IsNullOrEmpty(x.Barcode))
                .WithMessage("Barcode must contain only digits");

            // CategoryId
            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("CategoryId must be valid");

            // CostPrice
            RuleFor(x => x.CostPrice)
                .GreaterThan(0).WithMessage("Cost price must be greater than 0");

            // SellPrice
            RuleFor(x => x.SellPrice)
                .GreaterThan(0).WithMessage("Sell price must be greater than 0")
                .GreaterThanOrEqualTo(x => x.CostPrice)
                .WithMessage("Sell price must be >= cost price");

            // Quantity
            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("Quantity cannot be negative");

            // MinQuantity
            RuleFor(x => x.MinQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("MinQuantity cannot be negative")
                .LessThanOrEqualTo(x => x.Quantity)
                .WithMessage("MinQuantity cannot exceed Quantity");


        }
    }
}
