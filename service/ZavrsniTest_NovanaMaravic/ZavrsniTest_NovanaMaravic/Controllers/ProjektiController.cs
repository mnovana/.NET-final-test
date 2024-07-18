using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ZavrsniTest_NovanaMaravic.Models;
using ZavrsniTest_NovanaMaravic.Repositories.Interfaces;

namespace ZavrsniTest_NovanaMaravic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjektiController : ControllerBase
    {
        public readonly IProjekatRepository _projekatRepository;

        public ProjektiController(IProjekatRepository projekatRepository)
        {
            _projekatRepository = projekatRepository;
        }

        [HttpGet]
        public IActionResult GetProjekti()
        {

            return Ok(_projekatRepository.GetAll().ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetProjekat(int id)
        {
            Projekat projekat = _projekatRepository.GetById(id);
            if (projekat == null)
            {
                return NotFound();
            }

            return Ok(projekat);
        }

        [Route("/api/izvestaj")]
        [HttpGet]
        public IActionResult GetProjektiIzvestaj(int granica)
        {
            if (granica < 0)
            {
                return BadRequest();
            }

            return Ok(_projekatRepository.GetIzvestaj(granica));
        }

        [Route("/api/stanje")]
        [HttpGet]
        public IActionResult GetProjektiStanje()
        {
            return Ok(_projekatRepository.GetStanje());
        }

        [Route("nadji")]
        [HttpGet]
        public IActionResult SearchProjektiByName(string ime)
        {

            return Ok(_projekatRepository.SearchByName(ime).ToList());
        }
    }
}
