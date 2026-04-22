using DataAccessLayer.DTOs.CategoryDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Validators
{
    public class CategoryValidator : AbstractValidator<AddCategoryDTO>
    {
       public CategoryValidator()
        {
            RuleFor(c => c.CategoryName).NotEmpty().MinimumLength(2).MaximumLength(50);
        }
    }
}
