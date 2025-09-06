using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHKDotNetTrainingInPersonBatch1.WindowForm
{
    public class ProductDTO
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }

        public string ProductCode {  get; set; }

        public decimal Price { get; set; }

        public int Quantity {  get; set; }

        public bool DeleteFlag { get; set; }
    }
}
