using ExploreCalifornia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExploreCalifornia.Services
{
    public class InMemoryChatRoomService : IChatRoomService
    {
        private readonly Dictionary<Guid, ChatRoom> _roomInfo
            = new Dictionary<Guid, ChatRoom>();

        private readonly Dictionary<Guid, List<ChatMessage>> _messagesHistory
            = new Dictionary<Guid, List<ChatMessage>>();

        public Task<Guid> CreateRoom(string coonectionId)
        {
            var id = Guid.NewGuid();
            _roomInfo[id] = new ChatRoom
            {
                OwnerConnectionId = coonectionId
            };
            return Task.FromResult(id);
        }       

        public Task<Guid> GetRoomForConnectionId(string coonectionId)
        {
            var foundRoom = _roomInfo.FirstOrDefault(
                    x => x.Value.OwnerConnectionId == coonectionId
                );

            if(foundRoom.Key == Guid.Empty)
                throw new ArgumentNullException("Invalid Connection ID");

            return Task.FromResult(foundRoom.Key);
        }

        public Task SetRoomName(Guid roomId, string roomName)
        {
            if(!_roomInfo.ContainsKey(roomId))
                throw new ArgumentNullException("Invalid Room ID");

            _roomInfo[roomId].Name = roomName;

            return Task.CompletedTask;
        }

        public Task AddMessage(Guid roomId, ChatMessage chatMessage)
        {
            if (!_messagesHistory.ContainsKey(roomId))
            {
                _messagesHistory[roomId] = new List<ChatMessage>();
            }
            _messagesHistory[roomId].Add(chatMessage);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<ChatMessage>> GetMessageHistory(Guid roomId)
        {
            _messagesHistory.TryGetValue(roomId, out var messages);

            messages = messages ?? new List<ChatMessage>();

            var sortedMessages = messages.OrderBy(x => x.SentAt).AsEnumerable();

            return Task.FromResult(sortedMessages);
        }

        public Task<IReadOnlyDictionary<Guid, ChatRoom>> GetAllRooms()
        {
            return Task.FromResult(_roomInfo as IReadOnlyDictionary<Guid, ChatRoom>);
        }
    }
}
