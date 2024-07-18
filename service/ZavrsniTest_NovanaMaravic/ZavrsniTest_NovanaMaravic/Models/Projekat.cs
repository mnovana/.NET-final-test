using System.ComponentModel.DataAnnotations;

namespace ZavrsniTest_NovanaMaravic.Models
{
    public class Projekat
    {
        public int Id { get; set; }
        [Required]
        [StringLength(149)]
        public string Naziv { get; set; }
        [Required]
        [Range(2000,2023)]
        public int GodinaStart { get; set; }
        [Required]
        [Range(2023, 2030)]
        public int GodinaKraj { get; set; }
    }
}
