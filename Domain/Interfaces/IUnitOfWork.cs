using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Services_Interfaces;
namespace Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Categories { get; }
        IProductRepository Products { get; }
        IUserRepository Users { get; }
        IRefreshTokenRepository RfereshTokens { get; }
        IShoppingListPageRepository ShoppingListPageRepository { get; }
        Task<int> CompleteAsync();
    }
}
