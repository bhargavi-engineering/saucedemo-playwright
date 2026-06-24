using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceTest.Models
{
    public class InventoryItem
    {
        public string Name { get; set; } = String.Empty; 

        public string Desc { get; set; }  = String.Empty;

        public string ImageLink { get; set; } = String.Empty;

        public string Price { get; set; } = String.Empty;
    }
}
