namespace AnL.ViewModel
{
    public class PaginationBaseResponse<T>
    {
        public PaginationBaseResponse()
        {
        }
        public PaginationBaseResponse(T data)
        {
            Succeeded = true;
            ResponseCode = 0;
            ResponseMessage = string.Empty;
            Data = data;
        }
        public T Data { get; set; }
        public bool Succeeded { get; set; }
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
    }
}
