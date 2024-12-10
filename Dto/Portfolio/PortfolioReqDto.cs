using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinShark.Dto.Portfolio
{
    public class PortfolioReqDto
    {
        [Required]
        public string Symbol { get; set; } = string.Empty;
    }
}