namespace Store.App.Crosscutting.Commom.ViewModel.Pagination
{
    public class PagedItems<T>
    {
        public IList<T> Items { get; set; } = new List<T>();
        public long? Total { get; set; } = 0;
    }
}
