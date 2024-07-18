using System.ComponentModel.DataAnnotations;

namespace ZavrsniTest_NovanaMaravic.Models
{
    public class Istrazivac
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Ime { get; set; }
        [Required]
        [StringLength(80, MinimumLength = 2)]
        public string Prezime { get; set; }
        [Required]
        [Range(1900, 2024)]
        public int GodinaRodjenja { get; set; }
        [Required]
        [Range(10000.0, 500000.0)]
        public decimal Zarada { get; set; }

        [Required]
        public int ProjekatId { get; set; }
        public Projekat Projekat { get; set; }
    }
}
