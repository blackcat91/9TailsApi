using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MongoDB.Driver;
using NineTails.DataAccess.Helpers;
using MongoDB.Bson;
using System.Diagnostics;
using NineTails.DataAccess.DBAccess;
using NineTails.DataAccess.Models;

namespace NineTails.DataAccess.Data.Rooms
{
    public class RoomData : IRoomData
    {
        private readonly MongoDBAccess _mongoDB;
        private readonly HelperClass _helper;
        private readonly IMongoCollection<Room> _rooms;


        public RoomData(MongoDBAccess mongoDB, SQLAccess sql)
        {
            _mongoDB = mongoDB;
            _helper = new HelperClass(sql);
            _rooms = _mongoDB.ConnectToMongo<Room>("Rooms");
        }

        public async Task<Room> CreateRoom(Room room)
        {

            await _rooms.InsertOneAsync(room);
            var filter = Builders<Room>.Filter.Eq("Id", room.Id);
            var r = (await _rooms.FindAsync<Room>(filter)).FirstOrDefault();
            for (var i = 0; i < r.Playlist.Count; i++)
            {
                r.Playlist[i].RoomId = r.Id;
            }
            await _rooms.ReplaceOneAsync(filter, r, new ReplaceOptions { IsUpsert = true });
            return r;
        }

        public async Task<Room> GetRoom(string id)
        {

            var filter = Builders<Room>.Filter.Eq("Id", id);
            var room = (await _rooms.FindAsync(filter)).FirstOrDefault();

            return room;
        }

        public async Task<Room> UpdateRoom(Room room)
        {
            var filter = Builders<Room>.Filter.Eq("Id", room.Id);
            await _rooms.ReplaceOneAsync(filter, room, new ReplaceOptions { IsUpsert = true });

            return room;
        }

        public async Task<IEnumerable<Room>?> SearchRooms(string query)
        {
            var filter = Builders<Room>.Filter.Regex("Name", new BsonRegularExpression(query, "i"));

            try
            {
                var results = _rooms.Find(filter);
                var r = await results.ToListAsync();

                return r;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public async Task<IEnumerable<Message>> GetMessages(string roomId)
        {
            var room = await GetRoom(roomId);
            var messages = room.Messages.OrderBy(m => m.Time).Take(100);
            return messages;
        }

        public async Task? SendMessage(SendMessage messageBundle)
        {

            var filter = Builders<Room>.Filter.Eq("Name", messageBundle.Name);
            var room = (await _rooms.FindAsync(filter)).FirstOrDefault();
            var filter2 = Builders<Room>.Filter.Eq("Id", room.Id);
            var def = Builders<Room>.Update.AddToSet("Messages", messageBundle.Message);
            var updateOptions = new UpdateOptions { IsUpsert = false };
            await _rooms.UpdateOneAsync(filter2, def, updateOptions);

        }

        public async Task<PlaylistItem?> UpdatePlaylistItem(PlaylistItem item)
        {

            try
            {
                if (item.RoomId != null)
                {
                    var room = _rooms.Find(r => r.Id == item.RoomId).FirstOrDefault();
                    var itemIndex = room.Playlist.FindIndex(i => i.SeriesId == item.SeriesId && i.Url == item.Url && i.Episode == item.Episode);
                    Debug.WriteLine(itemIndex);
                    var newItem = await CheckLink(item);
                    var def = Builders<Room>.Update.Set(r => r.Playlist[itemIndex], newItem);
                    var updateOptions = new UpdateOptions { IsUpsert = false };
                    if (newItem != null)
                    {
                        await _rooms.UpdateOneAsync(r => r.Id == item.RoomId, def, updateOptions);
                        return newItem;
                    }
                    else
                    {
                        return null;
                    }

                }
                item.Url = await _helper.CheckLink(item)!;


                if (item.Url == null) return null;
                return item;
            }
            catch (Exception ex)
            {
                return item;
            }


        }

        public async Task<PlaylistItem?> CheckLink(PlaylistItem item)
        {
            item.Url = await _helper.CheckLink(item)!;


            if (item.Url == null) return null;
            return item;
        }
    }
}
