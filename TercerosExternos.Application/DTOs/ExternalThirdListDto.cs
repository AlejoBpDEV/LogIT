namespace TercerosExternos.Application.DTOs
{
    public class ExternalThirdListDto
    {
        public string RazonSocial { get; set; }
        public string NroIdentificacion { get; set; }
        public int TipoIdentificacion { get; set; }
        public string CodigoIntegracion { get; set; }
        public string Funciones { get; set; }
        public int EmpresaID { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string UserName { get; set; }
    }
}
