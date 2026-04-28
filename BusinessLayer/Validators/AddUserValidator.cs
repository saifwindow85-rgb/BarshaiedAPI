using BusinessLayer.AddUpdateDTOs.UserDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Validators
{
    public class AddUserValidator : AbstractValidator<AddUserDTO>
    {
        public AddUserValidator()
        {
            RuleFor(x => x.UserName)
               .NotEmpty()
               .WithMessage("UserName is required.")

                   .MinimumLength(3)
                 .WithMessage("UserName must be at least 3 characters long.")

                .MaximumLength(50)
               .WithMessage("UserName must not exceed 50 characters.")

               .Matches(@"^\S+$")
             .WithMessage("UserName must not contain spaces.")

              .Matches(@"^[a-zA-Z0-9_]+$")
             .WithMessage("UserName can only contain letters, numbers, and underscore (_).");
            
                     RuleFor(x => x.Password)
             .NotEmpty()
             .WithMessage("Password is required.")
            
             .MinimumLength(8)
             .WithMessage("Password must be at least 8 characters long.")
            
             .MaximumLength(100)
             .WithMessage("Password must not exceed 100 characters.")
            
             .Matches(@"^\S+$")
             .WithMessage("Password must not contain spaces.")
            
             .Matches(@"[A-Z]")
             .WithMessage("Password must contain at least one uppercase letter.")
            
             .Matches(@"[a-z]")
             .WithMessage("Password must contain at least one lowercase letter.")

             .Matches(@"[0-9]")
    .         WithMessage("Password must contain at least one number.");


            RuleFor(x => x.Permissions)
                .InclusiveBetween((byte)1, (byte)3)
                .WithMessage("Invalid permissions value.");
        }
    }
}
