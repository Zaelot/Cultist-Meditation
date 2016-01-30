using UnityEngine;
using System.Collections;
using System.Collections.Generic;



namespace Cultist
{

    [RequireComponent(typeof(TwitchIRC))]
    public class TwitchIrcListener : MonoBehaviour
    {
        public string botName;
        public GameObject playerGO;
        private Player player;
        private TwitchIRC IRC;
        //when message is recieved from IRC-server or our own message.
        void OnChatMsgReceived(string msg)
        {
            //parse from buffer.
            int msgIndex = msg.IndexOf("PRIVMSG #");
            string msgString = msg.Substring(msgIndex + IRC.channelName.Length + 11);
            string user = msg.Substring(1, msg.IndexOf('!') - 1);

            Debug.Log(user + ":" + msgString);
            if (user == botName)
            {
                //add new message.
                SendInputMessage(msgString);
            }
        }
        void SendInputMessage(string msgString)
        {
            //TODO: Send input.
            switch (msgString)
            {
                case "Up":
                    player.MoveUp();
                    break;
                case "Down":
                    player.MoveDown();
                    break;
                case "Left":
                    player.MoveLeft();
                    break;
                case "Right":
                    player.MoveRight();
                    break;
                case "Action":
                    player.Action();
                    break;
                default:
                    Debug.Log("Not a game command.");
                    break;
            }
        }
        // Use this for initialization
        void Start()
        {
            IRC = this.GetComponent<TwitchIRC>();
            player = playerGO.GetComponent<Player>();
            //IRC.SendCommand("CAP REQ :twitch.tv/tags"); //register for additional data such as emote-ids, name color etc.
            IRC.messageRecievedEvent.AddListener(OnChatMsgReceived);
        }
    }
}