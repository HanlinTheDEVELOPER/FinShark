using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinShark.Helper
{
    public class QueryObject
    {
        public string? Symbol { get; set; } = null;
        public string? Company { get; set; } = null;
        public string? SortBy { get; set; } = null;
        public bool IsDecending { get; set; } = false;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}