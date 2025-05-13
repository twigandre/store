namespace Store.App.Crosscutting.Commom.ViewModel
{
    public class RequestResponseData<T> : RequestResponseVM
    {
        public T? Data { get; set; }
    }
}
