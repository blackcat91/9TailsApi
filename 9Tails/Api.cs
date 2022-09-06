namespace NineTails
{
    public static class Api
    {
        public static void ConfigureApi(this WebApplication app)
        {
            app.MapPost("/update_item", UpdatePlaylistItem);
            app.MapPost("/update_room", UpdateRoom);
            app.MapPost("/create_room", CreateRoom);
            app.MapPost("/search_animes", SearchAnime);
            app.MapGet("/get_episodes/{id}", GetLinks);
            app.MapGet("/get_anime/{id}", GetAnime);
            app.MapGet("/get_episode/{seriesId}/{episode}", GetEpisode);
            app.MapGet("/get_room/{id}", GetRoom);
            app.MapGet("/get_messages/{roomId}", GetMessages);
            app.MapPost("/send_message", SendMessage);
        }


        public static async Task<IResult> GetRoom(string id, IRoomData data)
        {
            try
            {
                var newItem = await data.GetRoom(id);
                if (newItem == null) return Results.NotFound();
                return Results.Ok(newItem);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
            
        }
        public static async Task<IResult> CreateRoom(Room room, IRoomData data)
        {
            try
            {
                var newRoom = await data.CreateRoom(room);
                if (newRoom == null) return Results.NotFound();
                return Results.Ok(newRoom);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }

        }

        public static async Task<IResult> UpdateRoom(Room room, IRoomData data)
        {
            try
            {
                var updatedRoom = await data.UpdateRoom(room);
                if (updatedRoom == null) return Results.NotFound();
                return Results.Ok(updatedRoom);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }

        }
        public static async Task<IResult> GetMessages(string roomId, IRoomData data)
        {
            try
            {
                var messages = await data.GetMessages(roomId);
                if (messages == null) return Results.NotFound();
                return Results.Ok(messages);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }

        }

        public static async Task<IResult> SendMessage(SendMessage messageBundle, IRoomData data)
        {
            try
            {
                await data.SendMessage(messageBundle);
              
                return Results.Ok("Message Sent");
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }

        }

        public static async Task<IResult> UpdatePlaylistItem(PlaylistItem item,  IRoomData data)
        {
            try
            {
                var newItem = await data.UpdatePlaylistItem(item);
                if (newItem == null) return Results.NotFound();
                return Results.Ok(newItem);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
        public static async Task<IResult> GetAnime(int id, IAnimeData data)
        {
            try
            {
                var results = await data.GetAnime(id);
                if (results == null) return Results.NotFound();
                return Results.Ok(results);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        public static async Task<IResult> SearchAnime(string query, IAnimeData data)
        {
            try
            {
                
                var results = await data.SearchAnimes(query);
                if (results == null) return Results.NotFound();
                return Results.Ok(results);
            }
            catch(Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
        public static async Task<IResult> GetLinks(int id, IAnimeData data)
        {
            try
            {
                var results = await data.GetLinks(id);
                if (results == null) return Results.NotFound();
                return Results.Ok(results);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        public static async Task<IResult> GetEpisode(int seriesId,  int episode, IAnimeData data)
        {
            try
            {
                var results = await data.GetEpisode(seriesId, episode);
                if (results == null) return Results.NotFound();
                return Results.Ok(results);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }



    }
}
