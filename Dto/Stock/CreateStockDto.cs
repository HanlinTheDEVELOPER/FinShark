using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinShark.Dto.Stock
{
    public class CreateStockDto
    {
        [Required]
        [MaxLength(10)]
        public string Symbol { get; set; } = string.Empty;
        [Required]
        public string Company { get; set; } = string.Empty;
        [Required]
        [Range(0, 1000000000000)]
        public decimal Purchase { get; set; }
        [Required]
        [Range(0.001, 100)]
        public decimal LastDiv { get; set; }
        [Required]
        public string Industry { get; set; } = string.Empty;
        [Required]
        [Range(0, 10000000000000000)]
        public long MarketCap { get; set; }
    }
}