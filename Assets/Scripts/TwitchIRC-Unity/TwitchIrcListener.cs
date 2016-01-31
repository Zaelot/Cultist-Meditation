using UnityEngine;
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
	public string[] voteOptions = new string[] { "w", "s", "a", "d" };
	public float timer = 30.0f;
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

			if (msgString.Contains("Found no results")) {
				string finalString = voteOptions [UnityEngine.Random.Range (0, voteOptions.Length)];
				IRC.SendMsg ("Nobody voted? Let's do '" + finalString + "' then.");
				ExecuteCommand(finalString);
			}
		}
		if (user == chosenUser) {
			switch (msgString.ToLower()) {
			case "!vote w":
				chosenUserVote = "w";
				break;
			case "!vote s":
				chosenUserVote = "s";
				break;
			case "!vote left":
				chosenUserVote = "a";
				break;
			case "!vote right":
				chosenUserVote = "d";
				break;
			default:
				break;
			}//end.switch(msgString)
		}//end.if(chosenUser)
	} //OnChatMsgReceived()

	void SendActionMessage(string msgString)
	{
		// Right (100%) Total votes: 2
		// Left (50%) Right (50%) Total votes: 2
		msgString = msgString.Substring(msgString.IndexOf(":")+1);
		// Make array of Strings with values such as "Left (50%)"
		ArrayList voteArray = new ArrayList();
		while (msgString.Contains("%")) {
			voteArray.Add(msgString.TrimStart().Substring (0, msgString.IndexOf (')')+1));
			msgString = msgString.Substring (msgString.IndexOf (')')+1);
			if (!msgString.Contains ("%")) {
				break;
			}
		}//end.while(%)
		// Make array of percent values to check if there is a successful vote
		ArrayList votePercents = new ArrayList();
		int startIndex;
		int endIndex;
		for (int i = 0; i < voteArray.Count; i++) {
			startIndex = voteArray [i].ToString ().IndexOf ("(");
			endIndex = voteArray [i].ToString ().IndexOf ("%")-1;
			votePercents.Add(voteArray[i].ToString().Substring (startIndex + 1, endIndex - startIndex));
		}

		// Check if vote succeeds
		string finalString = voteArray [0].ToString().Substring (0, voteArray [0].ToString().IndexOf ("(") - 1);
		int finalIndex = 0;
		Debug.Log ("voteArray.Count = " + voteArray.Count);
		if (finalString.Contains ("Invalid") && voteArray.Count > 1) {
			Debug.Log ("voteArray[1] = " + voteArray [1].ToString ());
			finalString = voteArray [1].ToString().Substring (0, voteArray [1].ToString().IndexOf ("(") - 1);
			finalIndex = 1;
		}
		if ( !finalString.Contains("Invalid") && (votePercents.Count == 1+finalIndex || int.Parse(votePercents [finalIndex].ToString()) > int.Parse(votePercents [finalIndex+1].ToString()))) {
			// Vote succeeds.
			Debug.Log("Vote succeeds.");
			IRC.SendMsg("Vote succeeds. Let's do '" +finalString + "'.");
		} else {
			Debug.Log ("Vote fails.");
			// Tie, vote failed.
			if (chosenUserVote != "") {
				finalString = chosenUserVote;
			} else {
				finalString = voteOptions [UnityEngine.Random.Range (0, voteOptions.Length)];
				IRC.SendMsg ("Vote failed? Let's do '" + finalString + "' then.");
			}
		}
		Debug.Log (finalString);
		//TODO: Send correct action according to finalString.
		ExecuteCommand(finalString);
	} //End.SendActionMessage()

	// Use this for initialization
	void Start()
	{
		IRC = this.GetComponent<TwitchIRC>();
		cultistGO = GameObject.Find ("Cultist");
		if (cultistGO)
			cultist = cultistGO.GetComponent<Cultist>();
		//IRC.SendCommand("CAP REQ :twitch.tv/tags"); //register for additional data such as emote-ids, name color etc.
		IRC.messageRecievedEvent.AddListener(OnChatMsgReceived);

		IRC.SendMsg ("!moobot poll close");
		string votes = "";
		foreach (string vote in voteOptions) {
			votes = votes + " " + vote;
		}
		IRC.SendMsg ("!moobot poll open w, s, a, d");
		InvokeRepeating ("PollResults", timer, timer);
	} //End.Start()

	void PollReset()
	{
		Debug.Log ("Attempting a !poll");
//		IRC.SendMsg ("!poll");

		IRC.SendMsg ("!moobot poll reset");
		chosenUserVote = "";

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


	void PollResults()
	{
		IRC.SendMsg ("!moobot poll results");
	} //End.PollResults()

	public void ChoosePlayer() {
		// Invoke this when we want to choose new chosen twitch user as the cultist.
		IRC.SendMsg ("!moobot raffle userlist");
	} //End.ChoosePlayer()

	void ExecuteCommand(string finalString) {
		Debug.Log ("finalString: " + finalString);
		switch (finalString.ToLower()) {
		case "w":
			cultist.MoveUp ();
			PollReset ();
			break;
		case "s":
			cultist.MoveDown ();
			PollReset ();
			break;
		case "a":
			cultist.MoveLeft ();
			PollReset ();
			break;
		case "d":
			cultist.MoveRight ();
			PollReset ();
			break;
		case "action":
			cultist.Action ();
			PollReset ();
			break;
		default:
			Debug.Log ("Not a cultist command.");
			IRC.SendMsg("Meditation vote failed.. -> doing nothing.");
			//TODO: cultist.Idle();
			PollReset ();
			break;
		}//end.switch(finalString)
	}

} //End.TwitchIRCListeners{}