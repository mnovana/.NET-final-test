using AutoMapper;
using ZavrsniTest_NovanaMaravic.Models.DTO;

namespace ZavrsniTest_NovanaMaravic.Models.Profiles
{
    public class IstrazivacProfile : Profile
    {
        public IstrazivacProfile()
        {
            CreateMap<Istrazivac, IstrazivacDTO>();
        }
    }
}
