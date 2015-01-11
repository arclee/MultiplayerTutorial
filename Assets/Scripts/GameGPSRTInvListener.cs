using UnityEngine;
using System.Collections;
using GooglePlayGames.BasicApi.Multiplayer;

public class GameGPSRTInvListener : RealTimeMultiplayerListener
{
	
	/// <summary>
	/// Called during room setup to notify of room setup progress.
	/// </summary>
	/// <param name="percent">The room setup progress in percent (0.0 to 100.0).</param>
	void RealTimeMultiplayerListener.OnRoomSetupProgress(float percent)
	{
		Debug.Log("OnRoomSetupProgress:" + percent);
	}
	
	/// <summary>
	/// Notifies that room setup is finished. If <c>success == true</c>, you should
	/// react by starting to play the game; otherwise, show an error screen.
	/// </summary>
	/// <param name="success">Whether setup was successful.</param>
	void RealTimeMultiplayerListener.OnRoomConnected(bool success)
	{
		
		Debug.Log("OnRoomConnected:" + success);
	}

	
	/// <summary>
	/// Notifies that the current player has left the room. This may have happened
	/// because you called LeaveRoom, or because an error occurred and the player
	/// was dropped from the room. You should react by stopping your game and
	/// possibly showing an error screen (unless leaving the room was the player's
	/// request, naturally).
	/// </summary>
	void RealTimeMultiplayerListener.OnLeftRoom()
	{
		Debug.Log("OnLeftRoom:");
		
	}

	
	/// <summary>
	/// Called when peers connect to the room.
	/// </summary>
	/// <param name="participantIds">Participant identifiers.</param>
	void RealTimeMultiplayerListener.OnPeersConnected(string[] participantIds)
	{
		for (int i = 0; i < participantIds.Length; i++)
		{
			Debug.Log("OnPeersConnected:" + participantIds[i]);

		}
	}

	
	/// <summary>
	/// Called when peers disconnect from the room.
	/// </summary>
	/// <param name="participantIds">Participant identifiers.</param>
	void RealTimeMultiplayerListener.OnPeersDisconnected(string[] participantIds)
	{
		for (int i = 0; i < participantIds.Length; i++)
		{
			Debug.Log("OnPeersDisconnected:" + participantIds[i]);
			
		}
		
	}

	
	/// <summary>
	/// Called when a real-time message is received.
	/// </summary>
	/// <param name="isReliable">Whether the message was sent as a reliable message or not.</param>
	/// <param name="senderId">Sender identifier.</param>
	/// <param name="data">Data.</param>
	void RealTimeMultiplayerListener.OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
	{
		Debug.Log("OnRealTimeMessageReceived:" + " isReliable:" + isReliable + " senderId:"+senderId
		          + data);
		
	}



}
