using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products.DTOs
{
    public class PropertyImageDto
    {
        public Guid PropertyId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string ImageBase64 { get; set; } = string.Empty;
        public bool IsDefault { get; set; }
    }
}
