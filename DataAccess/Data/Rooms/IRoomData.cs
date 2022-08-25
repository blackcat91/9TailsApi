using DataAccess.Models;

namespace DataAccess.Data.Rooms
{
    public interface IRoomData
    {
        Task<PlaylistItem?> CheckLink(PlaylistItem item);
        Task<Room> CreateRoom(Room room);
        Task<IEnumerable<Message>> GetMessages(string roomId);
        Task<Room> GetRoom(string id);
        Task<IEnumerable<Room>?> SearchRooms(string query);
        Task SendMessage(SendMessage messageBundle);
        Task<PlaylistItem?> UpdatePlaylistItem(PlaylistItem item);
        Task<Room> UpdateRoom(Room room);
    }
}