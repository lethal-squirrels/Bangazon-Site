using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bangazon.Models.ProductViewModels
{
    public class ProductCountViewModel
    {
        public string TypeName { get; set; }

        public int ProductCount { get; set; }

        public int ProductTypeID { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }

}
