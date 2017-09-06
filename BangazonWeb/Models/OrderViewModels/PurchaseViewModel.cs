using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bangazon.Models.OrderViewModels
{
    public class PurchaseViewModel
    {
        public Order Order { get; set; }

        public Product Product { get; set; }
    }
}
