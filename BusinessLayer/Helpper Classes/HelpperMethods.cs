using Domain.Results;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Helpper_Classes
{
    public class HelpperMethods
    {
        public static bool IsValidId(string? Id)
        {
            if (int.TryParse(Id, out int id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}
