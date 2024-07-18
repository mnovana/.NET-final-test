using System.Collections.Generic;
using ZavrsniTest_NovanaMaravic.Models;

namespace ZavrsniTest_NovanaMaravic.Repositories.Interfaces
{
    public interface IIstrazivacRepository
    {
        IEnumerable<Istrazivac> GetAll();
        Istrazivac GetById(int id);
        void Add(Istrazivac istrazivac);
        void Update(Istrazivac istrazivac);
        void Delete(Istrazivac istrazivac);
        IEnumerable<Istrazivac> SearchByNameOrSurnameOrProject(string upit);
        IEnumerable<Istrazivac> SearchBySalary(decimal min, decimal max);
    }
}
