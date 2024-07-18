using System.Collections.Generic;
using ZavrsniTest_NovanaMaravic.Models;
using ZavrsniTest_NovanaMaravic.Models.DTO;

namespace ZavrsniTest_NovanaMaravic.Repositories.Interfaces
{
    public interface IProjekatRepository
    {
        IEnumerable<Projekat> GetAll();
        Projekat GetById(int id);
        IEnumerable<IzvestajDTO> GetIzvestaj(int granica);
        IEnumerable<StanjeDTO> GetStanje();
        IEnumerable<Projekat> SearchByName(string ime);

    }
}
