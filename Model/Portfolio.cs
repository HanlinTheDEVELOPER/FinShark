using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinShark.Model
{
    public class Portfolio
    {
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public int StockId { get; set; }
        public Stock? Stock { get; set; }

    }
}