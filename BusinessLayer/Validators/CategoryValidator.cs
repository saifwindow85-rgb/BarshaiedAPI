using Domain.DTOs.CategoryDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Validators
{
    public class AddCategoryValidator : AbstractValidator<AddCategoryDTO>
    {
       public AddCategoryValidator()
        {
            RuleFor(c => c.CategoryName).NotEmpty().MinimumLength(2).MaximumLength(50);
            RuleFor(c => c.CreatedByUserId).NotNull().WithMessage("CreatedByUserId Can Not Be Null!");
        }

       

    }
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDTO>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(c => c.CategoryName).NotEmpty().MinimumLength(2).MaximumLength(50);
            RuleFor(c => c.UpdatedByUserId).NotNull().WithMessage("UpdatedByUserId Can Not Be Null");
        }
    }
}
