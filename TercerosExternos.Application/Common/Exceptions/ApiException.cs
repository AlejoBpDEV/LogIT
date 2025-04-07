namespace TercerosExternos.Application.Common.Exceptions
{
    public class ApiException : ApplicationException
    {
        public ApiException(string message) : base(message){}
    }
}
