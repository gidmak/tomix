namespace API.Helpers
{
    public class ProductParams
    {
        public decimal? Price { get; set; }
        public bool? IsAvailable { get; set; }
        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value;
        }
        const int maxPageSize = 50;
        public int PageNumber {get; set;} = 1;

        private int _pageSize= 10;
        public int PageSize 
        { 
            get
            {
                return _pageSize;
            } 
            set 
            {
                _pageSize = (value > maxPageSize)? maxPageSize : value;
            }
        }
    }
}