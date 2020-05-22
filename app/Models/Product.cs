using System.Text;

namespace App.Models
{
    public class Product
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public bool InStock { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Name:{Name}");
            sb.AppendLine($"Description:{Description}");
            sb.AppendLine($"Category:{Category}");
            sb.AppendLine($"Price:{Price}");
            sb.AppendLine($"InStock:{InStock}");
            return sb.ToString();
        }
    }
}