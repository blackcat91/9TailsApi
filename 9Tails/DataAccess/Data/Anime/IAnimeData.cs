using NineTails.DataAccess.Models;

namespace NineTails.DataAccess.Data.Anime
{
    public interface IAnimeData
    {
        Task<Details?> GetAnime(int id);
        Task<Links?> GetEpisode(int seriesId, int episode);
        Task<IEnumerable<Links>?> GetLinks(int id);
        Task<IEnumerable<Details>?> SearchAnimes(string query);
    }
}