using System;

namespace BankWebApi.Helpers
{
    [Serializable]
    public class Response<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
