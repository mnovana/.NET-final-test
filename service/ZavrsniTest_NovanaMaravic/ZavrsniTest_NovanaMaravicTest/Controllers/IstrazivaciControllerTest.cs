using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ZavrsniTest_NovanaMaravic.Controllers;
using ZavrsniTest_NovanaMaravic.Models;
using ZavrsniTest_NovanaMaravic.Models.DTO;
using ZavrsniTest_NovanaMaravic.Models.Profiles;
using ZavrsniTest_NovanaMaravic.Repositories.Interfaces;

namespace ZavrsniTest_NovanaMaravicTest.Controllers
{
    public class IstrazivaciControllerTest
    {
        [Fact]
        public void GetIstrazivac_ValidId_ReturnsObject()
        {
            // Assert
            Istrazivac istrazivac = new Istrazivac()
            {
                Id = 1,
                Ime = "Marko",
                Prezime = "Lukic",
                Zarada = 32126.32M,
                GodinaRodjenja = 2000,
                ProjekatId = 3,
                Projekat = new Projekat() { Id = 3, Naziv = "Artist", GodinaStart = 2020, GodinaKraj = 2027 }
            };

            var mockRepository = new Mock<IIstrazivacRepository>();
            mockRepository.Setup(i => i.GetById(1)).Returns(istrazivac);

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new IstrazivacProfile()));
            IMapper mapper = new Mapper(mapperConfiguration);

            var istrazivaciController = new IstrazivaciController(mockRepository.Object, mapper);

            // Act
            var actionResult = istrazivaciController.GetIstrazivac(1) as OkObjectResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.NotNull(actionResult.Value);

            IstrazivacDTO objectResult = (IstrazivacDTO)actionResult.Value;

            Assert.Equal(istrazivac.Id, objectResult.Id);
            Assert.Equal(istrazivac.Ime, objectResult.Ime);
            Assert.Equal(istrazivac.Prezime, objectResult.Prezime);
            Assert.Equal(istrazivac.Zarada, objectResult.Zarada);
            Assert.Equal(istrazivac.GodinaRodjenja, objectResult.GodinaRodjenja);
            Assert.Equal(istrazivac.ProjekatId, objectResult.ProjekatId);
            Assert.Equal(istrazivac.Projekat.Naziv, objectResult.ProjekatNaziv);
        }

        [Fact]
        public void PutIstrazivac_InvalidId_ReturnsBadRequest()
        {
            // Assert
            Istrazivac istrazivac = new Istrazivac()
            {
                Id = 1,
                Ime = "Marko",
                Prezime = "Lukic",
                Zarada = 32126.32M,
                GodinaRodjenja = 2000,
                ProjekatId = 3,
                Projekat = new Projekat() { Id = 3, Naziv = "Artist", GodinaStart = 2020, GodinaKraj = 2027 }
            };

            var mockRepository = new Mock<IIstrazivacRepository>();

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new IstrazivacProfile()));
            IMapper mapper = new Mapper(mapperConfiguration);

            var istrazivaciController = new IstrazivaciController(mockRepository.Object, mapper);

            // Act
            var actionResult = istrazivaciController.PutIstrazivac(2, istrazivac) as BadRequestResult;

            // Assert
            Assert.NotNull(actionResult);
        }

        [Fact]
        public void DeleteIstrazivac_InvalidId_ReturnsNotFound()
        {
            // Assert
            var mockRepository = new Mock<IIstrazivacRepository>();

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new IstrazivacProfile()));
            IMapper mapper = new Mapper(mapperConfiguration);

            var istrazivaciController = new IstrazivaciController(mockRepository.Object, mapper);

            // Act
            var actionResult = istrazivaciController.DeleteIstrazivac(1) as NotFoundResult;

            // Assert
            Assert.NotNull(actionResult);
        }

        [Fact]
        public void SearchByPrice_ValidRequest_ReturnsCollection()
        {
            // Arrange
            List<Istrazivac> istrazivaci = new List<Istrazivac>()
            {
                new Istrazivac()
                {
                    Id = 1,
                    Ime = "Marko",
                    Prezime = "Lukic",
                    Zarada = 32126.32M,
                    GodinaRodjenja = 2000,
                    ProjekatId = 3,
                    Projekat = new Projekat() { Id = 3, Naziv = "Artist", GodinaStart = 2020, GodinaKraj = 2027 }
                },
                new Istrazivac() 
                { 
                    Id = 2, 
                    Ime = "Ana", 
                    Prezime = "Maric", 
                    Zarada = 105799.99M, 
                    GodinaRodjenja = 1977, 
                    ProjekatId = 1,
                    Projekat = new Projekat() { Id = 1, Naziv = "Gerudok", GodinaStart = 2020, GodinaKraj = 2025 }
                }
            };

            var mockRepository = new Mock<IIstrazivacRepository>();
            mockRepository.Setup(i => i.SearchBySalary(30000, 106000)).Returns(istrazivaci.AsQueryable());

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new IstrazivacProfile()));
            IMapper mapper = new Mapper(mapperConfiguration);

            var istrazivaciController = new IstrazivaciController(mockRepository.Object, mapper);

            FilterDTO filter = new FilterDTO() { ZaradaMin = 30000, ZaradaMax = 106000 };

            // Act
            var actionResult = istrazivaciController.SearchIstrazivaciBySalary(filter) as OkObjectResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.NotNull(actionResult.Value);

            List<IstrazivacDTO> listResult = (List<IstrazivacDTO>)actionResult.Value;

            for (int i = 0; i < listResult.Count; i++)
            {
                Assert.Equal(istrazivaci[i].Id, listResult[i].Id);
                Assert.Equal(istrazivaci[i].Ime, listResult[i].Ime);
                Assert.Equal(istrazivaci[i].Prezime, listResult[i].Prezime);
                Assert.Equal(istrazivaci[i].Zarada, listResult[i].Zarada);
                Assert.Equal(istrazivaci[i].GodinaRodjenja, listResult[i].GodinaRodjenja);
                Assert.Equal(istrazivaci[i].ProjekatId, listResult[i].ProjekatId);
                Assert.Equal(istrazivaci[i].Projekat.Naziv, listResult[i].ProjekatNaziv);
            }
        }
    }
}
