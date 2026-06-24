using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceTest.Utilities
{
    public class CsvHelper<T>
    {
        public string? DataFile { get; set; }    
        public T? Model { get; set; }    
        public TestCaseData ReadFile()
        {
            return new TestCaseData(); 
        }
    }
}
