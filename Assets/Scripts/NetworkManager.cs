using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
    private const string typeName = "TestMyGame1";
    private const string gameName = "RoomName";

    private bool isRefreshingHostList = false;
    private HostData[] hostList;

    public GameObject playerPrefab;


	public bool isServerUseProxy = false;
	public bool isClientUseProxy = false;

	public string proxyIP = "1.1.1.1";
	public string proxyPort = "1111";
	
	public string serverPassword = "asdf123";
	public string serverIP = "1.1.1.1";
	public string serverPort = "1234";
	public string toGuid = "1234";

	public int GUIScreenSizeX = 845;
	public int GUIScreenSizeY = 480;

	public TextMesh debugText; 
	
	//php check.	
	string phpret = "";
	public string phpurl = "https://arcleebs.appspot.com/getip";
	public string localhost = "http://localhost/getip";
	public bool uselocalhost = false;
	void Start()
	{
		SetProxyData();
		MasterServer.UnregisterHost();

		StartCoroutine("GetMyIP");


	}

	void SetProxyData()
	{
		Network.proxyIP = proxyIP;
		int port = 0;
		if (int.TryParse(proxyPort, out port))
		{
			Network.proxyPort = port;
		}

	}

    void OnGUI()
    {
		Matrix4x4 _matrix = GUI.matrix;
		_matrix.m00 = (float)Screen.width / GUIScreenSizeX;
		_matrix.m11 = (float)Screen.height / GUIScreenSizeY;
		GUI.matrix = _matrix;
		
		/*
		proxyIP = GUI.TextField(new Rect(100, 0, 250, 50), proxyIP);
		proxyPort = GUI.TextField(new Rect(100, 50, 250, 50), proxyPort);
*/
		
		serverIP = GUI.TextField(new Rect(100, 0, 250, 50), serverIP);
		serverPort = GUI.TextField(new Rect(100, 50, 250, 50), serverPort);

		toGuid = GUI.TextField(new Rect(100, 100, 250, 50), toGuid);
/*
		if (GUI.Button(new Rect(100, 100, 250, 50), "SetProxyData"))
		{
			SetProxyData();
		}
*/
        if (!Network.isClient && !Network.isServer)
        {
            if (GUI.Button(new Rect(100, 200, 250, 50), "Start Server"))
			{
                StartServer();
			}

            if (GUI.Button(new Rect(100, 250, 250, 50), "Refresh Hosts"))
			{
                RefreshHostList();
			}
			
			if (GUI.Button(new Rect(100, 300, 250, 50), "Create Server"))
			{
				//Network.incomingPassword = serverPassword;
				bool useNat = !Network.HavePublicAddress();
				int sp = 0;
				int.TryParse(serverPort, out sp);
				Network.InitializeServer(10, sp, useNat);

			}
			
			if (GUI.Button(new Rect(100, 350, 250, 50), "Connect Server"))
			{
				//bool useNat = !Network.HavePublicAddress();
				Network.useProxy = isServerUseProxy;
				int sp = 0;
				int.TryParse(serverPort, out sp);
				Network.Connect(serverIP, sp);

			}
			
			if (GUI.Button(new Rect(100, 400, 250, 50), "Connect GUID"))
			{
				//bool useNat = !Network.HavePublicAddress();
				Network.useProxy = isServerUseProxy;
				int sp = 0;
				int.TryParse(serverPort, out sp);
				Network.Connect(toGuid);
				
			}

            if (hostList != null)
            {
                for (int i = 0; i < hostList.Length; i++)
                {
                    if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
                        JoinServer(hostList[i]);
                }
            }
        }
    }

    private void StartServer()
    {
		Network.useProxy = isServerUseProxy;
		int sp = 0;
		int.TryParse(serverPort, out sp);
		Network.InitializeServer(5, sp, !Network.HavePublicAddress());
        MasterServer.RegisterHost(typeName, gameName);
    }

    void OnServerInitialized()
    {
		SpawnPlayer("Player_S");

		
		debugText.text = phpret
			+ "\n"
				+ Network.player.externalIP
				+ "\n"
				+ Network.player.ipAddress
				+ "\n"
				+ Network.player.port + " "+ Network.HavePublicAddress();
    }


    void Update()
    {
        if (isRefreshingHostList && MasterServer.PollHostList().Length > 0)
        {
            isRefreshingHostList = false;
            hostList = MasterServer.PollHostList();
        }

    }

    private void RefreshHostList()
    {
        if (!isRefreshingHostList)
        {
            isRefreshingHostList = true;
            MasterServer.RequestHostList(typeName);
        }
    }
	void OnPlayerConnected(NetworkPlayer player)
	{
		debugText.text = "Player connected from " + player.ipAddress + ":" + player.port;
		MasterServer.UnregisterHost();
	}
	void OnFailedToConnect(NetworkConnectionError error)
	{
		debugText.text = "Could not connect to server: " + error;
	}
    private void JoinServer(HostData hostData)
	{
		Network.useProxy = isClientUseProxy;
        Network.Connect(hostData);
		string tmpIp = "";
		int i = 0;
		while (i < hostData.ip.Length)
		{
			tmpIp = hostData.ip[i] + " ";
			i++;
		}

		debugText.text = "Join:" + tmpIp + ":" + hostData.port;
    }

    void OnConnectedToServer()
    {
        SpawnPlayer("Player_C");
    }

    private void SpawnPlayer(string name)
    {
		GameObject go = (GameObject)Network.Instantiate(playerPrefab, Vector3.up * 5, Quaternion.identity, 0);
		go.name = name;
    }


	
	public IEnumerator GetMyIP()
	{
		var form = new WWWForm();
		
		string url;
		
		if (uselocalhost)
		{
			url = localhost;
		}
		else
		{
			url = phpurl;
		}
				
		WWW w = new WWW(url);
		yield return w;
		
		
		if (w.error != null)
		{			
			Debug.Log("server down " + url);
			debugText.text = "getip server down " + url + " " + w.error;

			
			debugText.text += phpret
				+ "\n"
					+ Network.player.externalIP
					+ "\n"
					+ Network.player.externalPort
					+ "\n"
					+ Network.player.ipAddress
					+ "\n"
					+ Network.player.port + " "+ Network.HavePublicAddress();
		}
		else
		{
			phpret = w.text;
			bool success = false;
			if (phpret == "OK")
			{
				success = true;
			}
			else
			{
				success = false;
				
			}
			
			debugText.text = phpret
				+ "\nei"
					+ Network.player.externalIP
					+ "\nep:"
					+ Network.player.externalPort
					+ "\n:"
					+ Network.player.ipAddress
					+ "\n"
					+ Network.player.port + " "+ Network.HavePublicAddress()
					+ "\n"
					+ Network.player.guid;
		}
	}
}
