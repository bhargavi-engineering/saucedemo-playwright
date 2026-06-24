using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceTest.Models
{
    public class TestConfig
    {
        public string Environment { get; set; } = String.Empty; 
        public string ApiBaseUrl { get; set; } = String.Empty;
        public string SiteUrl { get; set; } = String.Empty;
        public string DbConnection { get; set; } = String.Empty;
    }
}
