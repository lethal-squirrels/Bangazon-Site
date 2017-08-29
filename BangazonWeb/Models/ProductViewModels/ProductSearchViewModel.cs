using Bangazon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bangazon.Models.ProductViewModels
{
    public class ProductSearchViewModel
    {
        public IEnumerable<Product> Products { get; set; }

        public string SearchTerms { get; set; }

        public string SearchRadio { get; set; }

    }
}
