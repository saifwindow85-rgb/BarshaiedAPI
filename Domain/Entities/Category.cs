using DataAccessLayer.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class Category :UserAbstract
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<Product>Products { get; set; } = new List<Product>();
    }
}
