namespace Warehouse.Core.Helpers
{
    public class Response<T> 
    {
        public Response()
        {
        }
        public Response(T data)
        {
            Succeeded = true;
            Message = null;
            Data = data;
        }
        public T Data { get; set; }
        public bool Succeeded { get; set; } = false;
        public string Message { get; set; }
    }
}
