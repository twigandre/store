namespace Store.App.Crosscutting.Commom.ViewModel
{
    public class RequestResponseData<T> : RequestResponse
    {
        public T? Data { get; set; }
    }
}
