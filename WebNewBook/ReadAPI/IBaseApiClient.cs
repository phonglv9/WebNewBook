namespace WebNewBook.ReadAPI
{
    public interface IBaseApiClient
    {
        Task<List<T>> GetListAsync<T>(string url);
    }
}
