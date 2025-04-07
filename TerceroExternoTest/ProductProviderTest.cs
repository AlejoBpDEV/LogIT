using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using TercerosExternos.Application.Queries;
using TercerosExternos.Domain.Interfaces;

namespace TerceroExternoTest
{
    [TestFixture]
    public class ProductProviderTest
    {
        [Test]
        public void GetExternalThirdList_InputParameters_ReturnThirdList()
        {
            //Arrange
            var mockRepository = new Mock<IDapperRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger<GetExternalThirdListQueryHandler>>();
            var mockCache = new Mock<ICacheService>();

            var queryHandler = new GetExternalThirdListQueryHandler(mockRepository.Object, mockMapper.Object, mockLogger.Object, mockCache.Object);
            var query = new GetExternalThirdListQuery
            {
                RazonSocial = "Prueba Alejo",
                NroIdentificacion = "",
                TipoIdentificacion = 0,
                CodigoIntegracion = "",
                Funciones = "0",
                EmpresaID = 1093,
                UserName = "albetancur@logit.com.co",
                Page = 0,
                PageSize = 10,
            };

            //Act
            var response = queryHandler.Handle(query, CancellationToken.None);

            //Assert
            Assert.IsTrue(response.IsCompletedSuccessfully);
        }
    }
}
