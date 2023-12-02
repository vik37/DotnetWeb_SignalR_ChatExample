using ExploreCalifornia.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExploreCalifornia.Services
{
    public interface IChatRoomService
    {
        Task<Guid> CreateRoom(string coonectionId);

        Task<Guid> GetRoomForConnectionId(string coonectionId);

        Task SetRoomName(Guid roomId, string roomName);

        Task AddMessage(Guid roomId, ChatMessage chatMessage);

        Task<IEnumerable<ChatMessage>> GetMessageHistory(Guid roomId);

        Task<IReadOnlyDictionary<Guid, ChatRoom>> GetAllRooms();
    }
}
