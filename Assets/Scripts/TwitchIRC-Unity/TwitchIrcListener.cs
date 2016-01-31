﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(TwitchIRC))]
public class TwitchIrcListener : MonoBehaviour {
	public string botName = "moobot";
	public string chosenUser = "";
	private string chosenUserVote = "";
	public GameObject cultistGO;
	private Cultist cultist;
	private TwitchIRC IRC;
	public string[] voteOptions;
	public float timer = 15.0f;
	//when message is recieved from IRC-server or our own message.
	void OnChatMsgReceived(string msg)
	{
		//parse from buffer.
		int msgIndex = msg.IndexOf("PRIVMSG #");
		string msgString = msg.Substring(msgIndex + IRC.channelName.Length + 11);
		string user = msg.Substring(1, msg.IndexOf('!') - 1);

		Debug.Log (user + ":" + msgString);
		if (user == botName) {
			if (msgString.Contains("Total votes:")) {
				// Vote results get.
				Debug.Log("Found poll results.");
				SendActionMessage(msgString);
			}
			if (msgString.Contains ("Drew user ")) {
				int lastIndex = msgString.IndexOf ("(");
				chosenUser = msgString.Substring (10, lastIndex - 11);
				Debug.Log ("Drew user: " + chosenUser);
			}
		}
		if (user == chosenUser) {
			switch (msgString.ToLower()) {
			case "!vote up":
				chosenUserVote = "up";
				break;
			case "!vote down":
				chosenUserVote = "down";
				break;
			case "!vote left":
				chosenUserVote = "left";
				break;
			case "!vote right":
				chosenUserVote = "right";
				break;
			default:
				break;
			}
		}
	}
	void SendActionMessage(string msgString)
	{
		// Right (100%) Total votes: 2
		// Left (50%) Right (50%) Total votes: 2

		// Make array of Strings with values such as "Left (50%)"
		ArrayList voteArray = new ArrayList();
		while (msgString.Contains("%")) {
			voteArray.Add(msgString.TrimStart().Substring (0, msgString.IndexOf (')')+1));
			msgString = msgString.Substring (msgString.IndexOf ('%')+2);
			if (!msgString.Contains ("%")) {
				break;
			}
		}
		// Make array of percent values to check if there is a successful vote
		ArrayList votePercents = new ArrayList();
		for (int i = 0; i < voteArray.Count; i++) {
			int startIndex = voteArray [i].ToString ().IndexOf ("(");
			int endIndex = voteArray [i].ToString ().IndexOf ("%")-1;
			votePercents.Add(voteArray[i].ToString().Substring (startIndex + 1, endIndex - startIndex));
		}

		// Check if vote succeeds
		string finalString = "";
		if (votePercents.Count == 1 || (int)votePercents [0] > (int)votePercents [1]) {
			// Vote succeeds.
			Debug.Log("Vote succeeds.");
			finalString = voteArray [0].ToString().Substring (0, voteArray [0].ToString().IndexOf ("(") - 1);
		} else {
			Debug.Log ("Vote fails.");
			// Tie, vote failed.
			finalString = chosenUserVote; // TODO: Put here the actual cultist players vote, or if empty put random.
		}
		Debug.Log (finalString);
		//TODO: Send correct action according to finalString.
		switch (finalString.ToLower()) {
		case "up":
			cultist.MoveUp ();
			PollReset ();
			break;
		case "down":
			cultist.MoveDown ();
			PollReset ();
			break;
		case "left":
			cultist.MoveLeft ();
			PollReset ();
			break;
		case "right":
			cultist.MoveRight ();
			PollReset ();
			break;
		case "action":
			cultist.Action ();
			PollReset ();
			break;
		default:
			Debug.Log ("Not a cultist command.");
			IRC.SendMsg("Meditation vote failed.. doing nothing.");
			//TODO: cultist.Idle();
			PollReset ();
			break;
		}
	}
	// Use this for initialization
	void Start()
	{
		IRC = this.GetComponent<TwitchIRC>();
		if (cultistGO)
			cultist = cultistGO.GetComponent<Cultist>();
		//IRC.SendCommand("CAP REQ :twitch.tv/tags"); //register for additional data such as emote-ids, name color etc.
		IRC.messageRecievedEvent.AddListener(OnChatMsgReceived);

		IRC.SendMsg ("!moobot poll close");
		string votes = "";
		foreach (string vote in voteOptions) {
			votes = votes + " " + vote;
		}
		IRC.SendMsg ("!moobot poll open up, down, left, right");
		InvokeRepeating ("PollResults", timer, timer);
	}

	void PollReset()
	{
<<<<<<< HEAD
		Debug.Log ("Attempting a !poll");
		IRC.SendMsg ("!poll");
	} //End.PollReset()

	public void SetCultist() {
		if (cultistGO)
			cultist = cultistGO.GetComponent<Cultist> ();
		else {
			cultistGO = GameObject.Find ("Cultist");
			if (cultistGO)
				cultist = cultistGO.GetComponent<Cultist> ();
			else
				Debug.LogWarning ("Cultist not found.");
		}
			
	} //End.SetCultist()
} //End.TwitchIRCListeners{}
=======
		IRC.SendMsg ("!moobot poll reset");
		chosenUserVote = "";
	}

	void PollResults()
	{
		IRC.SendMsg ("!moobot poll results");
	}

	public void ChoosePlayer() {
		// Invoke this when we want to choose new chosen twitch user as the cultist.
		IRC.SendMsg ("!moobot raffle userlist");
	}
}
>>>>>>> master
