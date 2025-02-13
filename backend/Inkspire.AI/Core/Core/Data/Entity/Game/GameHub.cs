using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Data.Entity.Game;
using Core.Data.Entity.User;
using InkSpire.Application.Abstractions;

public class GameHub : Hub
{
    public static Dictionary<Guid, GameRoom> ActiveRooms = new();
    private readonly IGroqLLM _groqService;

    public GameHub(IGroqLLM groqService)
    {
        _groqService = groqService;
    }

    public async Task JoinGame(Guid userId, string language)
    {
        var connectionId = Context.ConnectionId;

        if (ActiveRooms.Values.Any(r => r.Language == language && r.Players.Count == 1))
        {
            var room = ActiveRooms.Values.First(r => r.Language == language && r.Players.Count == 1);
            room.Players.Add(new AppUser { Id = userId });

            await Groups.AddToGroupAsync(connectionId, room.RoomId.ToString());
            await Clients.Group(room.RoomId.ToString()).SendAsync("GameMatched", room.RoomId);
        }
        else
        {
            var newRoom = new GameRoom { Language = language };
            newRoom.Players.Add(new AppUser { Id = userId });
            ActiveRooms[newRoom.RoomId] = newRoom;

            await Groups.AddToGroupAsync(connectionId, newRoom.RoomId.ToString());
        }
    }

    public async Task RequestTopic(Guid roomId)
    {
        if (!ActiveRooms.TryGetValue(roomId, out var room))
        {
            return;
        }

        var topic = await _groqService.GenerateGameTopicAsync(room.Language);
        room.Topic = topic.Topic;
        await Clients.Group(roomId.ToString()).SendAsync("GameTopic", topic.Topic);

        var firstPlayer = room.Players[0];
        room.CurrentTurn = firstPlayer.Id;

        await StartGame(roomId, firstPlayer.Id);
    }


    public async Task SubmitAnswer(Guid roomId, Guid userId, string answer)
    {
        if (!ActiveRooms.TryGetValue(roomId, out var room))
        {
            return;
        }

        if (room.UsedAnswers.Contains(answer.ToLower()))
        {
            await Clients.Client(userId.ToString()).SendAsync("DuplicateAnswer", userId, answer);
            return;
        }

        var checkResult = await _groqService.CheckAnswerAsync(answer, room.Topic, room.Language);
        if (checkResult.IsCorrect)
        {
            room.UsedAnswers.Add(answer.ToLower());
            await Clients.Group(roomId.ToString()).SendAsync("CorrectAnswer", userId, answer);
            room.CurrentTurn = room.Players.First(p => p.Id != userId).Id;
            await StartGame(roomId, room.CurrentTurn);
        }
        else
        {
            await Clients.Group(roomId.ToString()).SendAsync("WrongAnswer", userId, answer);
        }
    }

    private async Task StartGame(Guid roomId, Guid playerId)
    {
        if (!ActiveRooms.ContainsKey(roomId)) return;
        try
        {
            await Clients.Group(roomId.ToString()).SendAsync("StartGame", playerId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending StartGame event: {ex.Message}");
        }
    }
}
