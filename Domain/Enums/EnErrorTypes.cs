using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum EnErrorTypes { NotFound = 1, InvalidData = 2, InvalidRefrenceData = 3, InvalidAuthenticatedUserId  = 4,ExistedResource = 5}
    public enum EnActionTypes {Add = 1, Update = 2, Delete = 3 }
    public enum EnEntityTypes { Category = 1, Product = 2 ,User = 3,RefreshToken =4}

}
