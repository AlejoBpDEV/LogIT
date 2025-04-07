using AutoMapper;
using TercerosExternos.Application.DTOs;
using TercerosExternos.Application.Queries;

namespace TercerosExternos.Application.Mappings
{
    public class ExternalThirdProfile : Profile
    {
        public ExternalThirdProfile()
        {
            CreateMap<GetExternalThirdListQuery, ExternalThirdListDto>();
        }
    }
}
