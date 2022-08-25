using DataAccess.Models;

namespace DataAccess.Data.Anime
{
    public interface IAnimeData
    {
        Task<IEnumerable<Links>?> GetLinks(int id);
        Task<IEnumerable<Details>?> SearchAnimes(string query);
    }
}