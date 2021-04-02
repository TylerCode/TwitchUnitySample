using System.Text.RegularExpressions;
using System;
using TwitchLib.Client.Models;
using TwitchLib.Unity;
using UnityEngine;

public class TwitchClient : MonoBehaviour
{
	[SerializeField] //[SerializeField] Allows the private field to show up in Unity's inspector. Way better than just making it public
	private string _channelToConnectTo = TwitchSecrets.UserName;

	private string _channel;
	private Client _client;
	private GameScript _gameScript;

	private void Start()
	{
		// To keep the Unity application active in the background, you can enable "Run In Background" in the player settings:
		// Unity Editor --> Edit --> Project Settings --> Player --> Resolution and Presentation --> Resolution --> Run In Background
		// This option seems to be enabled by default in more recent versions of Unity. An aditional, less recommended option is to set it in code:
		// Application.runInBackground = true;

		//Create Credentials instance
		ConnectionCredentials credentials = new ConnectionCredentials(TwitchSecrets.BotUserName, TwitchSecrets.BotAccessToken);

		// Create new instance of Chat Client
		_client = new Client();

		// Initialize the client with the credentials instance, and setting a default channel to connect to.
		_client.Initialize(credentials, _channelToConnectTo);

		// Bind callbacks to events
		_client.OnConnected += OnConnected;
		_client.OnJoinedChannel += OnJoinedChannel;
		_client.OnMessageReceived += OnMessageReceived;
		_client.OnChatCommandReceived += OnChatCommandReceived;
		_client.OnUserJoined += OnUserJoined;
		_client.OnUserLeft += OnUserLeft;

		// Connect
		_client.Connect();

		//Get the GameLogic/Controller
		_gameScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameScript>();

		//Make sure you keep this in mind if you change scenes a lot
		DontDestroyOnLoad(this.gameObject);
	}

	private void OnConnected(object sender, TwitchLib.Client.Events.OnConnectedArgs e)
	{
		Debug.Log($"The bot {e.BotUsername} succesfully connected to Twitch.");

		if (!string.IsNullOrWhiteSpace(e.AutoJoinChannel))
			Debug.Log($"The bot will now attempt to automatically join the channel provided when the Initialize method was called: {e.AutoJoinChannel}");
	}

	private void OnUserJoined(object sender, TwitchLib.Client.Events.OnUserJoinedArgs e)
	{
		Debug.Log($"Looks like {e.Username} decided to join the party!");
	}

	private void OnUserLeft(object sender, TwitchLib.Client.Events.OnUserLeftArgs e)
	{
		Debug.Log($"Looks like {e.Username} decided to leave us T.T");
	}

	private void OnJoinedChannel(object sender, TwitchLib.Client.Events.OnJoinedChannelArgs e)
	{
		Debug.Log($"The bot {e.BotUsername} just joined the channel: {e.Channel}");

		_channel = e.Channel;
	}

	private void OnMessageReceived(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
	{
		Debug.Log($"Message received from {e.ChatMessage.Username}: {e.ChatMessage.Message}");
	}

	private void OnChatCommandReceived(object sender, TwitchLib.Client.Events.OnChatCommandReceivedArgs e)
	{
		string inputString = e.Command.CommandText.ToLower();

		switch (inputString)
		{
			case "help":
				_client.SendMessage(e.Command.ChatMessage.Channel, "Hi, give !feed");
				break;
			case "feed":
				_gameScript.YeetFood();
				break;
			case "pet":
				Debug.Log("Still needs to be implemented.");
				break;
			default:
				//_client.SendMessage(e.Command.ChatMessage.Channel, $"Unknown chat command: {e.Command.CommandIdentifier}{e.Command.CommandText}");
				Debug.Log("Unknown command, is it for another app?");
				break;
		}
	}

	//Use this method if you need to send a message BACK to twitch
	public void SendMessageToChat(string message)
	{
		_client.SendMessage(_channel, message);
	}
}