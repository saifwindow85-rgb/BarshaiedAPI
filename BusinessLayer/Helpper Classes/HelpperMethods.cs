using BusinessLayer.Results;
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
        public static bool CanBeConvertedToInt(string value)
        {
            if(int.TryParse(value,out int result))
            {
                return true;
            }
            return false;
        }


    }
}
