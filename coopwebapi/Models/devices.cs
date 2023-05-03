using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace coopwebapi.Models
{
    public class devices
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Processor { get; set; } = string.Empty;
        public float Price { get; set; } = float.MinValue;
        public int Manufacturer_id { get; set; }
    }
}
