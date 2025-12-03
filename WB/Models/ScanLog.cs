using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WB.Models
{
    public class ScanLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Line { get; set; }

        [Required]
        [DisplayName("Material Number")]
        public string MaterialNumber { get; set; }

        [Required]
        [DisplayName("Component Number")]
        public string ComponentNumber { get; set; }

        [DisplayName("Supplier")]
        public string? Supplier { get; set; }

        [DisplayName("Production Date")]
        public string? ProductionDate { get; set; }

        [DisplayName("Batch Number")]
        public string? BatchNumber { get; set; }

        public string? Status { get; set; }

        [Required]
        [DisplayName("Component Description")]
        public DateOnly ScanDate { get; set; }

        [Required]
        public string? ScanTime { get; set; }

    }
}
