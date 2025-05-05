namespace Store.App.Crosscutting.ViewModels.Pagination
{
    public class PagedItems<T>
    {
        public IList<T> Items { get; set; }
        public long? Total { get; set; }
    }
}
