namespace TercerosExternos.Application.Common.DTOs
{
    public class ResponseDto<T>
    {
        public string Message { get; set; }
        public string Importance { get; set; }
        public IEnumerable<T> DataList { get; set; }
        public T Data { get; set; }
    }
}
