using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using ZavrsniTest_NovanaMaravic.Models;
using ZavrsniTest_NovanaMaravic.Repositories.Interfaces;

namespace ZavrsniTest_NovanaMaravic.Repositories
{
    public class IstrazivacRepository : IIstrazivacRepository
    {
        private readonly AppDbContext _context;

        public IstrazivacRepository(AppDbContext context)
        {
            this._context = context;
        }

        public void Add(Istrazivac istrazivac)
        {
            _context.Istrazivaci.Add(istrazivac);
            _context.SaveChanges();
        }

        public void Delete(Istrazivac istrazivac)
        {
            _context.Istrazivaci.Remove(istrazivac);
            _context.SaveChanges();
        }

        public IEnumerable<Istrazivac> GetAll()
        {
            return _context.Istrazivaci.Include(i => i.Projekat).OrderBy(i => i.Prezime);
        }

        public Istrazivac GetById(int id)
        {
            return _context.Istrazivaci.Include(i => i.Projekat).FirstOrDefault(i => i.Id == id);
        }

        public void Update(Istrazivac istrazivac)
        {
            _context.Entry(istrazivac).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public IEnumerable<Istrazivac> SearchByNameOrSurnameOrProject(string upit)
        {
            return _context.Istrazivaci.Include(i => i.Projekat).Where(i => i.Ime.Contains(upit) || i.Prezime.Contains(upit) || i.Projekat.Naziv.Contains(upit)).OrderByDescending(i => i.GodinaRodjenja);
        }

        public IEnumerable<Istrazivac> SearchBySalary(decimal min, decimal max)
        {
            return _context.Istrazivaci.Include(i => i.Projekat).Where(i => i.Zarada > min && i.Zarada < max).OrderByDescending(i => i.Zarada);
        }

    }
}
