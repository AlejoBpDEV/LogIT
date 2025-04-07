using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TercerosExternos.Application.Common.DTOs;
using TercerosExternos.Application.DTOs;
using TercerosExternos.Domain.Interfaces;

namespace TercerosExternos.Application.Queries
{
    public class GetExternalThirdListQuery : IRequest<ResponseDto<ExternalThirdListDto>>
    {
        public string RazonSocial { get; set; } = "Prueba Alejo";
        public string NroIdentificacion { get; set; } = string.Empty;
        public int TipoIdentificacion { get; set; }
        public string CodigoIntegracion { get; set; } = string.Empty;
        public string Funciones { get; set; } = "0";
        public int EmpresaID { get; set; } = 1093;
        public string UserName { get; set; } = "albetancur@logit.com.co";
        public int Page { get; set; }
        public int PageSize { get; set; } = 10;
    }

    public class GetExternalThirdListQueryHandler : IRequestHandler<GetExternalThirdListQuery, ResponseDto<ExternalThirdListDto>>
    {
        private readonly ILogger<GetExternalThirdListQueryHandler> _logger;
        private readonly IDapperRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GetExternalThirdListQueryHandler(
            IDapperRepository repository,
            IMapper mapper, 
            ILogger<GetExternalThirdListQueryHandler> logger, 
            ICacheService cacheService)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        private string GetCacheKey(GetExternalThirdListQuery request)
        {
            return $"ExternalThirdList_{request.RazonSocial}_{request.NroIdentificacion}_{request.TipoIdentificacion}_{request.CodigoIntegracion}_{request.Funciones}_{request.EmpresaID}_{request.UserName}_{request.Page}_{request.PageSize}";
        }

        public async Task<ResponseDto<ExternalThirdListDto>> Handle(GetExternalThirdListQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = GetCacheKey(request);
            var cachedResponse = await _cacheService.GetAsync<ResponseDto<ExternalThirdListDto>>(cacheKey);

            if (cachedResponse != null)
            {
                _logger.LogInformation("Se ha encontrado información en caché.");
                return cachedResponse;
            }

            var parameters = new
            {
                pRazonSocial = request.RazonSocial,
                pNroIdentificacion = request.NroIdentificacion,
                pTipoIdentificacion = request.TipoIdentificacion,
                pCodigoIntegracion = request.CodigoIntegracion,
                pFunciones = request.Funciones,
                pEmpresaID = request.EmpresaID,
                pUserName = request.UserName,
                pPage = request.Page,
                pPageSize = request.PageSize
            };

            const string sql = "spTercerosExternos_ConsultarGeneral_Paginacion";

            try
            {
                var result = await _repository.QueryAsync<dynamic>(sql, parameters);

                if(result is null || !result.Any())
                {
                    _logger.LogInformation("No se ha encontrado información.");

                    var response = new ResponseDto<ExternalThirdListDto>
                    {
                        Message = "No hay información para mostrar.",
                        Importance = "Warning",
                    };

                    await _cacheService.SetAsync(cacheKey, response, TimeSpan.FromHours(1));
                    return response;
                }

                var externalThirdList = _mapper.Map<IEnumerable<ExternalThirdListDto>>(result);

                _logger.LogInformation("Consulta ejecutada exitosamente.");

                var successfulResponse = new ResponseDto<ExternalThirdListDto>
                {
                    Message = "Consulta exitosa",
                    Importance = "Success",
                    DataList = externalThirdList
                };

                await _cacheService.SetAsync(cacheKey, successfulResponse, TimeSpan.FromHours(1));
                return successfulResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ha ocurrido un error al intentar ejecutar el query.");

                return new ResponseDto<ExternalThirdListDto>
                {
                    Message = "Ocurrió un error al procesar la solicitud.",
                    Importance = "Danger",
                };
            }
        }
    }
}
