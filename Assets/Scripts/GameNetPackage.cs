using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class GameNetPackage
{
	public int cmdtype;
	public object data;
};

[Serializable]
public class GameNetScore
{
	public int score;
}

[Serializable]
public class GameNetScorePackage : GameNetPackage
{
	public int score;
}
