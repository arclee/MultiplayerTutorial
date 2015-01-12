using UnityEngine;
using System.Collections;
using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;

public class GameGPS : MonoBehaviour {
	
	public int GUIScreenSizeX = 845;
	public int GUIScreenSizeY = 480;
	static public GameGPS Instance;

	static public Action<bool>	onSignIn;
	static public Action<bool>	onHighScore;

	Invitation myInvitation;
	
	GameGPSRTInvListener el = new GameGPSRTInvListener();
	public TextMesh debugText; 
	void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(transform.gameObject);
		}		
	}


	// Use this for initialization
	void Start ()
	{
		GooglePlayGames.BasicApi.PlayGamesClientConfiguration config = new GooglePlayGames.BasicApi.PlayGamesClientConfiguration.Builder()
			// registers a callback to handle game invitations.
			.WithInvitationDelegate(OnInvitationReceived) 
				.Build();
		
		PlayGamesPlatform.InitializeInstance(config);
		
		// recommended for debugging:
		PlayGamesPlatform.DebugLogEnabled = true;

		el.gameGPS = this;
		//PlayGamesPlatform.DebugLogEnabled = true;
		SignIn();

	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void SignIn()
	{
		if (IsSignIn())
		{
			return;
		}

		PlayGamesPlatform.Instance.localUser.Authenticate(SignInCB);
	}

	public void SignInCB(bool success)
	{

		if (onSignIn != null)
		{
			onSignIn(success);
		}
		
	}

	public bool IsSignIn()
	{
		return PlayGamesPlatform.Instance.IsAuthenticated();
	}
	
	public void SignOut()
	{
		if (!IsSignIn())
		{
			return;
		}
		PlayGamesPlatform.Instance.SignOut();
	}

	public void ShowLeaderboardUI()
	{
		if (!IsSignIn())
		{
			SignIn();
			return;
		}
		PlayGamesPlatform.Instance.ShowLeaderboardUI();
	}
	
	public void ShowAchievementsUI()
	{
		if (!IsSignIn())
		{
			SignIn();
			return;
		}

		PlayGamesPlatform.Instance.ShowAchievementsUI();
	}
	


	public void HighScore(int score)
	{
		if (!IsSignIn())
		{
			return;
		}

		PlayGamesPlatform.Instance.ReportScore((long)score, "CgkIgrHW7YIGEAIQCA", HighScoreCB);
	}
	
	public void HighScoreCB(bool success)
	{
		if (!IsSignIn())
		{
			return;
		}
		
		if (onHighScore != null)
		{
			onHighScore(success);	
		}
		
	}

	public void RealTimeMatchInvitationScreen()
	{
		const int MinOpponents = 1, MaxOpponents = 3;
		const int GameVariant = 0;
		//GameGPSRTInvListener el = new GameGPSRTInvListener();
		el.gameGPS = this;
		PlayGamesPlatform.Instance.RealTime.CreateWithInvitationScreen(MinOpponents, MaxOpponents,
		                                                               GameVariant, el);
	}

	
	public void RealTimeMatchQuickGame()
	{
		const int MinOpponents = 1, MaxOpponents = 3;
		const int GameVariant = 1;
		//GameGPSRTInvListener el = new GameGPSRTInvListener();
		el.gameGPS = this;
		PlayGamesPlatform.Instance.RealTime.CreateQuickGame(MinOpponents, MaxOpponents,
		                                                    GameVariant, el);
	}
	
	public void RealTimeMatchAcceptFromInbox()
	{
		//GameGPSRTInvListener el = new GameGPSRTInvListener();
		el.gameGPS = this;
		PlayGamesPlatform.Instance.RealTime.AcceptFromInbox(el);
	}
	public void RealTimeMatchAcceptInvitation()
	{
		//GameGPSRTInvListener el = new GameGPSRTInvListener();
		el.gameGPS = this;
		//PlayGamesPlatform.Instance.RealTime.AcceptInvitation(el);
	}
	
	public void RealTimeMatchLeaveRoom()
	{
		PlayGamesPlatform.Instance.RealTime.LeaveRoom();
	}

	
	public void OnInvitationReceived(Invitation invitation, bool shouldAutoAccept)
	{ 
		Debug.Log("OnInvitationReceived " + invitation.InvitationId + " " + shouldAutoAccept.ToString());
		if (shouldAutoAccept)
		{
			// Invitation should be accepted immediately. This happens if the user already
			// indicated (through the notification UI) that they wish to accept the invitation,
			// so we should not prompt again.
			//ShowWaitScreen();
			
			//GameGPSRTInvListener el = new GameGPSRTInvListener();
			el.gameGPS = this;
			PlayGamesPlatform.Instance.RealTime.AcceptInvitation(invitation.InvitationId, el);
		} else {
			// The user has not yet indicated that they want to accept this invitation.
			// We should *not* automatically accept it. Rather we store it and 
			// display an in-game popup:
			myInvitation = invitation;
		}

	}

	void OnGUI()
	{
		Matrix4x4 _matrix = GUI.matrix;
		_matrix.m00 = (float)Screen.width / GUIScreenSizeX;
		_matrix.m11 = (float)Screen.height / GUIScreenSizeY;
		GUI.matrix = _matrix;

		if (GUI.Button(new Rect (0, 100, 200, 50), "RealTimeMatchInvitationScreen"))
		{
			RealTimeMatchInvitationScreen();
		}
		if (GUI.Button(new Rect (0, 150, 200, 50), "RealTimeMatchQuickGame"))
		{
			RealTimeMatchQuickGame();
		}
		if (GUI.Button(new Rect (0, 200, 200, 50), "RealTimeMatchInBox"))
		{
			RealTimeMatchAcceptFromInbox();
		}
		if (GUI.Button(new Rect (0, 250, 200, 50), "RealTimeMatchLeaveRoom"))
		{
			RealTimeMatchLeaveRoom();
		}

		
		if (myInvitation != null)
		{
			// show the popup
			string who = (myInvitation.Inviter != null && 
			              myInvitation.Inviter.DisplayName != null) ?
				myInvitation.Inviter.DisplayName : "Someone";
			GUI.Label(new Rect (0, 300, 200, 50), who + " is challenging you to a race!");
			if (GUI.Button(new Rect (0, 350, 200, 50), "Accept!")) {
				// user wants to accept the invitation!
				//ShowWaitScreen();
				PlayGamesPlatform.Instance.RealTime.AcceptInvitation(
					myInvitation.InvitationId, el);
			}
			if (GUI.Button(new Rect (0, 400, 200, 50), "Decline")) {
				// user wants to decline the invitation
				PlayGamesPlatform.Instance.RealTime.DeclineInvitation(
					myInvitation.InvitationId);
			}
		}
	}
}
