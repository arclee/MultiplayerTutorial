using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

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
	public string name;
	public List<GameNetScore> scores = new List<GameNetScore>();
}

[Serializable]
public class GameNetScorePackage : GameNetPackage
{
	public int score;
}
