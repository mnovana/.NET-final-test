using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using ZavrsniTest_NovanaMaravic.Models;
using ZavrsniTest_NovanaMaravic.Models.DTO;
using ZavrsniTest_NovanaMaravic.Repositories.Interfaces;

namespace ZavrsniTest_NovanaMaravic.Repositories
{
    public class ProjekatRepository : IProjekatRepository
    {
        private readonly AppDbContext _context;

        public ProjekatRepository(AppDbContext context)
        {
            this._context = context;
        }

        public IEnumerable<Projekat> GetAll()
        {
            return _context.Projekti.OrderBy(p => p.Naziv);
        }

        public Projekat GetById(int id)
        {
            return _context.Projekti.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<IzvestajDTO> GetIzvestaj(int granica)
        {
            return _context.Istrazivaci.Include(i => i.Projekat)
                .GroupBy(i => i.ProjekatId)
                .Select(group => new IzvestajDTO()
                {
                    NazivProjekta = _context.Projekti.Where(p => p.Id == group.Key).Select(p => p.Naziv).Single(),
                    Godine = _context.Projekti.Where(p => p.Id == group.Key).Select(p => p.GodinaKraj - p.GodinaStart).Single(),
                    ProsecnaStarost = group.Select(i => DateTime.Now.Year - i.GodinaRodjenja).Average()
                }).Where(x => x.Godine > granica).OrderBy(x => x.NazivProjekta).ToList();
        }

        public IEnumerable<StanjeDTO> GetStanje()
        {
            return _context.Istrazivaci.Include(i => i.Projekat)
                .GroupBy(i => i.ProjekatId)
                .Select(group => new StanjeDTO()
                {
                    NazivProjekta = _context.Projekti.Where(p => p.Id == group.Key).Select(p => p.Naziv).Single(),
                    BrojIstrazivaca = group.Count(),
                    NajvecaZarada = group.Max(i => i.Zarada),
                    UkupnaZarada = group.Sum(i => i.Zarada)
                }).OrderByDescending(x => x.UkupnaZarada).ToList();
        }

        public IEnumerable<Projekat> SearchByName(string ime)
        {
            return _context.Projekti.Where(p => p.Naziv.Equals(ime)).OrderBy(p => p.GodinaStart).ThenByDescending(p => p.GodinaKraj);
        }


    }
}
