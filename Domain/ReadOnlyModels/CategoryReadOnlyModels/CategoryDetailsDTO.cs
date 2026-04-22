using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.ReadOnlyModels.CategoryReadOnlyModels
{
    public class CategoryDetailsDTO
    {
        public int Id { get; set; }
        public string? CategoryName { get; set; }
        public int ?NumberOfProducts { get; set; }
        public decimal ?TotalCostPrice { get; set; }
        public decimal? TotalSellPrice { get; set; }
        public decimal ?AvrageCostPrice { get; set; }
        public decimal ? AvrageSellPrice { get; set; }
        public decimal ? AvrageProfitMargin { get; set; }
    }
}
