using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameNetPackageTest : MonoBehaviour {

	public static byte[] ObjectToByteArray(object obj)
	{
		if(obj == null)
			return null;
		BinaryFormatter bf = new BinaryFormatter();
		MemoryStream ms = new MemoryStream();
		bf.Serialize(ms, obj);
		return ms.ToArray();
	}

	public static object ByteArrayToObject(byte[] arrBytes)
	{
		MemoryStream memStream = new MemoryStream();
		BinaryFormatter binForm = new BinaryFormatter();
		memStream.Write(arrBytes, 0, arrBytes.Length);
		memStream.Seek(0, SeekOrigin.Begin);
		object obj = (object) binForm.Deserialize(memStream);
		return obj;
	}

	// Use this for initialization
	void Start ()
	{
		GameNetPackage gk = new GameNetPackage();
		gk.cmdtype = 123;

		GameNetScore gns = new GameNetScore();
		gns.score = 9987;
		gk.data = gns;

		byte[] inbyte = ObjectToByteArray(gk);

		GameNetPackage ogk = (GameNetPackage)ByteArrayToObject(inbyte);

		Debug.Log(ogk.cmdtype);
		GameNetScore ogs = (GameNetScore)ogk.data;
		Debug.Log(ogs.score);

	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
