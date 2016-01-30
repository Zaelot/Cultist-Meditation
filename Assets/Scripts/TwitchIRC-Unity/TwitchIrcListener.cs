using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(TwitchIRC))]
public class TwitchIrcListener : MonoBehaviour {
	public string botName;
	public GameObject cultistGO;
	private Cultist cultist;
	private TwitchIRC IRC;
	//when message is recieved from IRC-server or our own message.
	void OnChatMsgReceived(string msg)
	{
		//parse from buffer.
		int msgIndex = msg.IndexOf("PRIVMSG #");
		string msgString = msg.Substring(msgIndex + IRC.channelName.Length + 11);
		string user = msg.Substring(1, msg.IndexOf('!') - 1);

		Debug.Log (user + ":" + msgString);
		if (user == botName) {
			//add new message.
			SendInputMessage(msgString);
		}
	}
	void SendInputMessage(string msgString)
	{
		//TODO: Send input.
		switch (msgString) {
		case "Up":
			cultist.MoveUp ();
			break;
		case "Down":
			cultist.MoveDown ();
			break;
		case "Left":
			cultist.MoveLeft ();
			break;
		case "Right":
			cultist.MoveRight ();
			break;
		case "Action":
			cultist.Action ();
			break;
		default:
			Debug.Log ("Not a game command.");
			break;
		}
	}
	// Use this for initialization
	void Start()
	{
		IRC = this.GetComponent<TwitchIRC>();
		cultist = cultistGO.GetComponent<Cultist>();
		//IRC.SendCommand("CAP REQ :twitch.tv/tags"); //register for additional data such as emote-ids, name color etc.
		IRC.messageRecievedEvent.AddListener(OnChatMsgReceived);
	}
}
