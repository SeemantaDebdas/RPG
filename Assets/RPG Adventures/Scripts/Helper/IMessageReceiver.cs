using System.Collections;
using UnityEngine;

namespace RPG
{
    public enum MessageType
    {
        Damaged,
        Dead
    }

    public interface IMessageReceiver 
    {
        void OnReceiveMessage(MessageType type);
    }
}