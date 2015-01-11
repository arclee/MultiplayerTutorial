using UnityEngine;
using System.Collections;
using System;
using GooglePlayGames;

public class GameGPS : MonoBehaviour {
	
	public int GUIScreenSizeX = 845;
	public int GUIScreenSizeY = 480;
	static public GameGPS Instance;

	static public Action<bool>	onSignIn;
	static public Action<bool>	onHighScore;

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
		PlayGamesPlatform.Instance.RealTime.CreateWithInvitationScreen(MinOpponents, MaxOpponents,
		                                                               GameVariant, new GameGPSRTInvListener());
	}

	
	public void RealTimeMatchQuickGame()
	{
		const int MinOpponents = 1, MaxOpponents = 3;
		const int GameVariant = 1;
		PlayGamesPlatform.Instance.RealTime.CreateQuickGame(MinOpponents, MaxOpponents,
		                                                    GameVariant, new GameGPSRTInvListener());
	}

	
	public void RealTimeMatchLeaveRoom()
	{
		PlayGamesPlatform.Instance.RealTime.LeaveRoom();
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
		if (GUI.Button(new Rect (0, 200, 200, 50), "RealTimeMatchLeaveRoom"))
		{
			RealTimeMatchLeaveRoom();
		}		
	}
}
