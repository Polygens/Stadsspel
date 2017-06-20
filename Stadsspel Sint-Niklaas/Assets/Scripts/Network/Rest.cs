using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

public class Rest
{
	//private const string BASE_URL = "https://stadspelapp-sintniklaas.herokuapp.com/api/";
	private const string BASE_URL = "http://localhost:8090/api/";

	private const string GAME_SUFFIX = "games";
	private const string COLOR_SUFFIX = "colors";
	private const string ACCOUNT_SUFFIX = "accounts";
	private const string LOCATION_SUFFIX = "locations";

	//######################### BASE METHODS
	private static int Get(string urlSuffix, out string serverResponse)
	{
		ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
		// Create a request using a URL that can receive a Post.
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BASE_URL + urlSuffix);

		// Set the Method property of the request to POST.
		request.Method = "GET";
		request.AutomaticDecompression = DecompressionMethods.None;//todo add decompression method for lzString

		// Get the response.
		WebResponse response = request.GetResponse();
		// Get the stream containing content returned by the server.
		Stream dataStream = response.GetResponseStream();
		// Open the stream using a StreamReader for easy access.
		StreamReader reader = new StreamReader(dataStream);
		// Read the content.
		string responseFromServer = reader.ReadToEnd();
		// Display the content.
		serverResponse = responseFromServer;
		// Clean up the streams.
		reader.Close();
		dataStream.Close();
		response.Close();

		//return the http status
		return (int)((HttpWebResponse)response).StatusCode;
	}

	private static int Put(string urlSuffix, string data, out string serverResponse)
	{
		ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
		// Create a request using a URL that can receive a Post.
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BASE_URL + urlSuffix);
		// Set the Method property of the request to POST.  
		request.Method = "PUT";
		request.AutomaticDecompression = DecompressionMethods.None;//todo add decompression method for lzString
																   // Create POST data and convert it to a byte array.
		byte[] byteArray = Encoding.UTF8.GetBytes(data);
		// Set the ContentType property of the WebRequest.  
		//request.ContentType = "text/plain";
		request.ContentType = "application/json";
		request.Headers.Add("charset", "UTF-8");
		// Set the ContentLength property of the WebRequest.  
		request.ContentLength = byteArray.Length;
		// Get the request stream.  
		Stream dataStream = request.GetRequestStream();
		// Write the data to the request stream.  
		dataStream.Write(byteArray, 0, byteArray.Length);
		// Close the Stream object.  
		dataStream.Close();
		// Get the response.
		Debug.Log("1");
		WebResponse response = request.GetResponse();
		Debug.Log("2");
		// Get the stream containing content returned by the server.  
		dataStream = response.GetResponseStream();
		// Open the stream using a StreamReader for easy access.  
		StreamReader reader = new StreamReader(dataStream);
		Debug.Log("3");
		// Read the content.  
		string responseFromServer = reader.ReadToEnd();
		// Display the content.
		serverResponse = responseFromServer;
		Debug.Log("4");
		// Clean up the streams.  
		reader.Close();
		dataStream.Close();
		response.Close();

		//return the http status
		return (int)((HttpWebResponse)response).StatusCode;
	}

	private static int Post(string urlSuffix, string data, out string serverResponse)
	{
		ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
		// Create a request using a URL that can receive a Post.
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BASE_URL + urlSuffix);
		// Set the Method property of the request to POST.  
		request.Method = "POST";
		request.AutomaticDecompression = DecompressionMethods.None;//todo add decompression method for lzString
																   // Create POST data and convert it to a byte array.
		byte[] byteArray = Encoding.UTF8.GetBytes(data);
		// Set the ContentType property of the WebRequest.
		//request.ContentType = "text/plain";
		request.ContentType = "application/json";
		request.Headers.Add("charset", "UTF-8");
		// Set the ContentLength property of the WebRequest.  
		request.ContentLength = byteArray.Length;
		// Get the request stream.  
		Stream dataStream = request.GetRequestStream();
		// Write the data to the request stream.  
		dataStream.Write(byteArray, 0, byteArray.Length);
		// Close the Stream object.  
		dataStream.Close();
		// Get the response.  
		try
		{
			WebResponse response = request.GetResponse();
			// Get the stream containing content returned by the server.  
			dataStream = response.GetResponseStream();
			// Open the stream using a StreamReader for easy access.  
			StreamReader reader = new StreamReader(dataStream);
			// Read the content.  
			string responseFromServer = reader.ReadToEnd();
			// Display the content.
			serverResponse = responseFromServer;
			// Clean up the streams.  
			reader.Close();
			dataStream.Close();
			response.Close();

			//return the http status
			return (int)((HttpWebResponse)response).StatusCode;
		}
		catch (WebException e)
		{
			serverResponse = "";
			return (int)((HttpWebResponse)e.Response).StatusCode;
		}
	}

	private static void HandleReturnCode(int code)
	{
		int majorCode = (code / 100) % 10;
		if (majorCode != 2)
		{
			if (majorCode == 4)
			{
				throw new RestException(code);
				//error in request parameters or body
			}
			else if (majorCode == 5)
			{
				throw new RestException(code);
				//server error
			}
			else
			{
				throw new RestException(code);
				//unknown other return
			}
		}
	}



	/// <summary>
	/// Returns wether certificate is ok
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="certificate"></param>
	/// <param name="chain"></param>
	/// <param name="sslPolicyErrors"></param>
	/// <returns></returns>
	public static bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
	{
		bool isOk = true;
		// If there are errors in the certificate chain, look at each error to determine the cause.
		if (sslPolicyErrors != SslPolicyErrors.None)
		{
			for (int i = 0; i < chain.ChainStatus.Length; i++)
			{
				if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown)
				{
					chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
					chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
					chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
					chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
					bool chainIsValid = chain.Build((X509Certificate2)certificate);
					if (!chainIsValid)
					{
						isOk = false;
					}
				}
			}
		}
		return isOk;
	}


	//######################### GAME METHODS
	public static string NewGame(GameResource resource)
	{
		string response;
		string urlSuffix = GAME_SUFFIX;
		string data = JsonUtility.ToJson(resource);
		int code = Post(urlSuffix, data, out response);
		HandleReturnCode(code);
		return response;
	}

	public static string SaveGameSettings(string data, string token)
	{
		string response;
		string urlSuffix = GAME_SUFFIX + "/updategame/" + token;
		int code = Put(urlSuffix, data, out response);
		HandleReturnCode(code);
		return response;
	}

	public static string GetGameById(string gameId)
	{
		string response;
		string urlSuffix = GAME_SUFFIX + "/" + gameId;
		int code = Get(urlSuffix, out response);
		HandleReturnCode(code);
		return response;
	}

	public static List<GameListResource> GetStagedGames()
	{
		string reply;
		const string urlSuffix = GAME_SUFFIX + "/staged";
		var code = Get(urlSuffix, out reply);
		HandleReturnCode(code);
		Debug.Log(reply);
		return JsonArrayHelper.getJsonList<GameListResource>(reply);
	}

	public static string GetRunningGames()
	{
		string response;
		string urlSuffix = GAME_SUFFIX + "/running";
		int code = Get(urlSuffix, out response);
		HandleReturnCode(code);
		return response;
	}

	public static string GetAllGames()
	{
		string response;
		string urlSuffix = GAME_SUFFIX + "/";
		int code = Get(urlSuffix, out response);
		HandleReturnCode(code);
		return response;
	}

	public static string StartGame(string gameId, string token)
	{
		string response;
		string urlSuffix = GAME_SUFFIX + "/" + gameId + "/startGame/" + token;
		int code = Post(urlSuffix, "", out response);
		HandleReturnCode(code);
		return response;
	}

	public static string StopGame(string gameId, string token)
	{
		string response;
		string urlSuffix = GAME_SUFFIX + "/" + gameId + "/stopGame/" + token;
		int code = Post(urlSuffix, "", out response);
		HandleReturnCode(code);
		return response;
	}

	public static string ChangeDuration(string gameId, string token,int minutes)
	{
		string response;
		string urlSuffix = GAME_SUFFIX + "/" + gameId + "/duration/" + token;
		int code = Post(urlSuffix, minutes+"", out response);
		HandleReturnCode(code);
		return response;
	}

	public static string KickPlayer(string gameId, string token,string playerId)
	{
		string response;
		string urlSuffix = GAME_SUFFIX + "/" + gameId + "/kick/" + token;
		int code = Post(urlSuffix, playerId, out response);
		HandleReturnCode(code);
		return response;
	}

	public static string GetPlayersByClientId(string gameId, string clientId)
	{
		string response;
		string urlSuffix = GAME_SUFFIX + "/" + gameId + "/players/" + clientId;
		int code = Get(urlSuffix, out response);
		HandleReturnCode(code);
		return response;
	}

	public static string RegisterPlayer(string clientId, string name, string password, string gameId)
	{
		string response;
		string urlSuffix = GAME_SUFFIX + "/" + gameId + "/register";
		RegisterUserResource rur = new RegisterUserResource
		{
			name = name,
			clientID = clientId,
			password = password
		};
		int code = Post(urlSuffix, JsonUtility.ToJson(rur), out response);
		HandleReturnCode(code);
		return response;
	}

	public static string UnregisterPlayer(string clientId, string gameId)
	{
		string response;
		string urlSuffix = GAME_SUFFIX + "/" + gameId + "/unregister/" + clientId;
		int code = Post(urlSuffix, "", out response);
		HandleReturnCode(code);
		return response;
	}

	//######################### LOCATION METHODS
	public static string GetAreaLocations()
	{
		string response;
		string urlSuffix = LOCATION_SUFFIX + "/arealocations";
		int code = Get(urlSuffix, out response);
		HandleReturnCode(code);
		return response;
	}

	public static string GetAreaLocations(string type)
	{
		string response;
		string urlSuffix = LOCATION_SUFFIX + "/arealocations?type=" + type;
		int code = Get(urlSuffix, out response);
		HandleReturnCode(code);
		return response;
	}

	public static string GetPointLocations()
	{
		string response;
		string urlSuffix = LOCATION_SUFFIX + "/pointlocations";
		int code = Get(urlSuffix, out response);
		HandleReturnCode(code);
		return response;
	}

	public static string GetPointLocations(string type)
	{
		string response;
		string urlSuffix = LOCATION_SUFFIX + "/pointlocations?type=" + type;
		int code = Get(urlSuffix, out response);
		HandleReturnCode(code);
		return response;
	}

	//######################### ACCOUNT METHODS
	public static string DeviceLogin(string deviceId)
	{
		string response;
		string urlSuffix = ACCOUNT_SUFFIX + "/login/" + deviceId;
		int code = Post(urlSuffix, "", out response);
		HandleReturnCode(code);
		return response;
	}

	public static string Login(string data)
	{
		string response;
		string urlSuffix = ACCOUNT_SUFFIX + "/login";
		int code = Post(urlSuffix, data, out response);
		HandleReturnCode(code);
		return response;
	}

	public static string Register(string data)
	{
		string response;
		string urlSuffix = ACCOUNT_SUFFIX;
		int code = Post(urlSuffix, data, out response);
		HandleReturnCode(code);
		return response;
	}

	public static string GetAccountInfo(string emailAddress)
	{
		string response;
		string urlSuffix = ACCOUNT_SUFFIX + "/" + emailAddress;
		int code = Get(urlSuffix, out response);
		HandleReturnCode(code);
		return response;
	}

	//######################### COLOR METHODS
	public static string GetColors()
	{
		string response;
		string urlSuffix = COLOR_SUFFIX + "/";
		int code = Get(urlSuffix, out response);
		HandleReturnCode(code);
		return response;
	}

}

internal class RestException : Exception
{
	public RestException(int code) : base("ERROR CODE: " + code) { }
}

[Serializable]
public class ConnectionResource
{
	public string clientToken;

	public ConnectionResource(string clientToken)
	{
		this.clientToken = clientToken;
	}
}

[Serializable]
public class GameResource
{
	public string token;
	public string roomName;
	public int maxTeams;
	public int maxPlayersPerTeam;
	public string password;

	public GameResource(string token, string roomName, int maxTeams, int maxPlayersPerTeam, string password)
	{
		this.token = token;
		this.roomName = roomName;
		this.maxTeams = maxTeams;
		this.maxPlayersPerTeam = maxPlayersPerTeam;
		this.password = password;
	}
}

[Serializable]
public class GameListResource
{
	public string id;
	public string name;
	public bool hasPassword;
}

[Serializable]
public class LoginResource
{
	public string emailaddress;
	public string password;
}

[Serializable]
public class RegisterUserResource
{
	public string clientID;
	public string name;
	public string password;
}

/// <summary>
/// Source: http://answers.unity3d.com/answers/1328660/view.html
/// </summary>
public class JsonArrayHelper
{
	//Usage:
	//YouObject[] objects = JsonHelper.getJsonArray<YouObject> (jsonString);
	public static T[] getJsonArray<T>(string json)
	{
		string newJson = "{ \"array\": " + json + "}";
		Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
		return wrapper.array;
	}

	//Usage:
	//string jsonString = JsonHelper.arrayToJson<YouObject>(objects);
	public static string arrayToJson<T>(T[] array)
	{
		Wrapper<T> wrapper = new Wrapper<T>();
		wrapper.array = array;
		return JsonUtility.ToJson(wrapper);
	}


	public static List<T> getJsonList<T>(string json)
	{
		string newJson = "{ \"array\": " + json + "}";
		Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
		List<T> list = new List<T>();
		foreach (T obj in wrapper.array)
		{
			list.Add(obj);
		}
		return list;
	}

	public static string ListToJson<T>(List<T> list)
	{
		Wrapper<T> wrapper = new Wrapper<T>();
		T[] array = new T[list.Count];
		for (int i = 0; i < list.Count; i++)
		{
			array[i] = list[i];
		}
		wrapper.array = array;
		return JsonUtility.ToJson(wrapper);
	}

	[Serializable]
	private class Wrapper<T>
	{
		public T[] array;
	}
}
