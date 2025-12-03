using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WB.Models
{
    public class SetVal
    {
        public string X1 { get; set; }
        public List<string> X2 { get; set; }
        public string X3 { get; set; }
        public string X4 { get; set; }
        public string X5 { get; set; }
        public string checkVal { get; set; }
        public string X6 { get; set; }
    };
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Material Number")]
        public string MaterialNumber { get; set; }

        [Required]
        [DisplayName("Component Number")]
        public string ComponentNumber { get; set; }

        [Required]
        [DisplayName("Component Description")]
        public string ComponentDescription { get; set; }

        [Required]
        public double Quantity { get; set; }

        [Required]
        [DisplayName("Change Number")]
        public string ChangeNumber { get; set; }

        public DateTime CreateDateTime { get; set; } = DateTime.Now;
    }


}
