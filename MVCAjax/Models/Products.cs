using System.ComponentModel.DataAnnotations;

namespace MVCAjax.Models
{
    public class Products
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        //public DateTime FechaVencimiento { get; set; }
        public string FechaVencimiento { get; set; }


    }
}
