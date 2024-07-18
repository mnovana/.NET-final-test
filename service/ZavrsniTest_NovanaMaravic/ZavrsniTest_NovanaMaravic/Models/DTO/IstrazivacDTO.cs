using System.ComponentModel.DataAnnotations;

namespace ZavrsniTest_NovanaMaravic.Models.DTO
{
    public class IstrazivacDTO
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public int GodinaRodjenja { get; set; }
        public decimal Zarada { get; set; }
        
        public int ProjekatId { get; set; }
        public string ProjekatNaziv { get; set; }
    }
}
