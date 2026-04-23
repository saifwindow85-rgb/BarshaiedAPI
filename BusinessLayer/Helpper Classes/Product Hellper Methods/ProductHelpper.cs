using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Helpper_Classes.Product_Hellper_Methods
{
    public class ProductHelpper
    {
        public static async  Task<bool> CheckRelatedData(int userId,int categoryId,IUnitOfWork unitOfWork)
        {
            bool IsUserExist = await unitOfWork.Users.IsUserExsist(userId);
            bool IsCategoryExist = await unitOfWork.Categories.IsCategoryExist(categoryId);
            if(!IsUserExist|| !IsCategoryExist)
                return false;

            return true;
        }
    }
}
