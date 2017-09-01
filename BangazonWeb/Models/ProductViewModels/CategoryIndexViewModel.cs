using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bangazon.Models.ProductViewModels
{
    public class CategoryIndexViewModel
    { 

        public IEnumerable<Product> Products { get; set; }

        public List<ProductCountViewModel> productTypeCounts { get; set; }
    }
}
