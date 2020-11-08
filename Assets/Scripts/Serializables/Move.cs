using System;

namespace Serializables
{
    [Serializable]
    public class Move
    {
        public string userId;
        public string command;
        public string type;
        public int cardId;
    }
}
