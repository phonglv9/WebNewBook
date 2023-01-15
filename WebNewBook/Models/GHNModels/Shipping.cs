namespace WebNewBook.Models.GHNModels
{
    public class Shipping
    {
        public int code { get; set; }
        public string message { get; set; }
        public IEnumerable<Data> data { get; set; }
        public class Data
        {
            public int total { get; set; }
           

        }
      

    }
}
