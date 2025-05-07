namespace Store.App.Crosscutting.Commom.ViewModel.Pagination
{
    public class PagedOptions
    {
        public int? Page { get; set; } = 1;
        public bool? Reverse { get; set; }
        public int? Size { get; set; } = 10;
        public string? Sort { get; set; }
    }
}
