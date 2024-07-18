using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ZavrsniTest_NovanaMaravic.Models;
using ZavrsniTest_NovanaMaravic.Models.DTO;
using ZavrsniTest_NovanaMaravic.Repositories.Interfaces;

namespace ZavrsniTest_NovanaMaravic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IstrazivaciController : ControllerBase
    {
        private readonly IIstrazivacRepository _istrazivacRepository;
        private readonly IMapper _mapper;

        public IstrazivaciController(IIstrazivacRepository istrazivacRepository, IMapper mapper)
        {
            _istrazivacRepository = istrazivacRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetIstrazivaci()
        {

            return Ok(_istrazivacRepository.GetAll().AsQueryable().ProjectTo<IstrazivacDTO>(_mapper.ConfigurationProvider).ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetIstrazivac(int id)
        {
            Istrazivac istrazivac = _istrazivacRepository.GetById(id);
            if (istrazivac == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IstrazivacDTO>(istrazivac));
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult PutIstrazivac(int id, Istrazivac istrazivac)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != istrazivac.Id)
            {
                return BadRequest();
            }

            try
            {
                _istrazivacRepository.Update(istrazivac);
            }
            catch
            {
                return BadRequest();
            }

            return Ok(istrazivac);
        }

        [HttpPost]
        public IActionResult PostIstrazivac(Istrazivac istrazivac)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _istrazivacRepository.Add(istrazivac);
            return CreatedAtAction("GetIstrazivac", new { id = istrazivac.Id }, _mapper.Map<IstrazivacDTO>(istrazivac));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteIstrazivac(int id)
        {
            var istrazivac = _istrazivacRepository.GetById(id);
            if (istrazivac == null)
            {
                return NotFound();
            }

            _istrazivacRepository.Delete(istrazivac);
            return NoContent();
        }

        [Route("potrazi")]
        [HttpGet]
        public IActionResult SearchIstrazivaciByNameOrSurnameOrProject(string upit)
        {

            return Ok(_istrazivacRepository.SearchByNameOrSurnameOrProject(upit).AsQueryable().ProjectTo<IstrazivacDTO>(_mapper.ConfigurationProvider).ToList());
        }

        [Authorize]
        [Route("/api/pretraga")]
        [HttpPost]
        public IActionResult SearchIstrazivaciBySalary(FilterDTO filter)
        {
            if (filter.ZaradaMin < 10000 || filter.ZaradaMin > 500000 || filter.ZaradaMax < 10000 || filter.ZaradaMax > 500000 || filter.ZaradaMin > filter.ZaradaMax)
            {
                return BadRequest();
            }

            return Ok(_istrazivacRepository.SearchBySalary(filter.ZaradaMin, filter.ZaradaMax).AsQueryable().ProjectTo<IstrazivacDTO>(_mapper.ConfigurationProvider).ToList());
        }
    }
}
