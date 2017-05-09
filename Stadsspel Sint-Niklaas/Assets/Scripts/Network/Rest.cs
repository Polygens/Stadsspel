using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

public class Rest {
  //private const string BASE_URL = "https://stniklaas-stadsspel.herokuapp.com/api/";
  private const string BASE_URL = "http://localhost:8090/api/";
  private const string GAME_SUFFIX = "games";
  private const string COLOR_SUFFIX = "colors";
  private const string ACCOUNT_SUFFIX = "accounts";
  private const string LOCATION_SUFFIX = "locations";

  //######################### BASE METHODS
  private static int Get(string urlSuffix, out string serverResponse) {
    ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
    // Create a request using a URL that can receive a Post.
    Debug.Log("" + BASE_URL + urlSuffix);
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

  private static int Put(string urlSuffix, string data, out string serverResponse) {
    ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
    // Create a request using a URL that can receive a Post.   
    Debug.Log("" + BASE_URL + urlSuffix);
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

  private static int Post(string urlSuffix, string data, out string serverResponse) {
    ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
    // Create a request using a URL that can receive a Post.   
    Debug.Log("" + BASE_URL + urlSuffix);
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

  private static void HandleReturnCode(int code) {
    int majorCode = (code / 100) % 10;
    if (majorCode != 2) {
      if (majorCode == 4) {
        //error in request parameters or body
      } else if (majorCode == 5) {
        //server error
      } else {
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
  public static bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
    bool isOk = true;
    // If there are errors in the certificate chain, look at each error to determine the cause.
    if (sslPolicyErrors != SslPolicyErrors.None) {
      for (int i = 0; i < chain.ChainStatus.Length; i++) {
        if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown) {
          chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
          chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
          chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
          chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
          bool chainIsValid = chain.Build((X509Certificate2)certificate);
          if (!chainIsValid) {
            isOk = false;
          }
        }
      }
    }
    return isOk;
  }


  //######################### GAME METHODS
  public string NewGame(string data) {
    string response;
    string urlSuffix = GAME_SUFFIX;
    int code = Post(urlSuffix, data, out response);
    HandleReturnCode(code);
    return response;
  }

  public string SaveGameSettings(string data, string token) {
    string response;
    string urlSuffix = GAME_SUFFIX + "/updategame/" + token;
    int code = Put(urlSuffix, data, out response);
    HandleReturnCode(code);
    return response;
  }

  public string GetGameById(string gameId) {
    string response;
    string urlSuffix = GAME_SUFFIX + "/" + gameId;
    int code = Get(urlSuffix, out response);
    HandleReturnCode(code);
    return response;
  }

  public string GetStagedGames() {
    string response;
    string urlSuffix = GAME_SUFFIX + "/staged";
    int code = Get(urlSuffix, out response);
    HandleReturnCode(code);
    return response;
  }

  public string GetRunningGames() {
    string response;
    string urlSuffix = GAME_SUFFIX + "/running";
    int code = Get(urlSuffix, out response);
    HandleReturnCode(code);
    return response;
  }

  public string GetAllGames() {
    string response;
    string urlSuffix = GAME_SUFFIX + "/";
    int code = Get(urlSuffix, out response);
    HandleReturnCode(code);
    return response;
  }

  public string StartGame(string gameId, string token) {
    string response;
    string urlSuffix = GAME_SUFFIX + "/" + gameId + "/startGame/" + token;
    int code = Post(urlSuffix, "", out response);
    HandleReturnCode(code);
    return response;
  }

  public string StopGame(string gameId, string token) {
    string response;
    string urlSuffix = GAME_SUFFIX + "/" + gameId + "/stopGame/" + token;
    int code = Post(urlSuffix, "", out response);
    HandleReturnCode(code);
    return response;
  }

  public string GetPlayersByClientId(string gameId, string clientId) {
    string response;
    string urlSuffix = GAME_SUFFIX + "/" + gameId + "/players/" + clientId;
    int code = Get(urlSuffix, out response);
    HandleReturnCode(code);
    return response;
  }

  public string RegisterPlayer(string data, string gameId) {
    string response;
    string urlSuffix = GAME_SUFFIX + "/" + gameId + "/register";
    int code = Post(urlSuffix, data, out response);
    HandleReturnCode(code);
    return response;
  }

  public string UnregisterPlayer(string clientId, string gameId) {
    string response;
    string urlSuffix = GAME_SUFFIX + "/" + gameId + "/unregister/" + clientId;
    int code = Post(urlSuffix, "", out response);
    HandleReturnCode(code);
    return response;
  }

  //######################### LOCATION METHODS
  public string GetAreaLocations() {
    string response;
    string urlSuffix = LOCATION_SUFFIX + "/arealocations";
    int code = Get(urlSuffix, out response);
    HandleReturnCode(code);
    return response;
  }

  public string GetAreaLocations(string type) {
    string response;
    string urlSuffix = LOCATION_SUFFIX + "/arealocations?type=" + type;
    int code = Get(urlSuffix, out response);
    HandleReturnCode(code);
    return response;
  }

  public string GetPointLocations() {
    string response;
    string urlSuffix = LOCATION_SUFFIX + "/pointlocations";
    int code = Get(urlSuffix, out response);
    HandleReturnCode(code);
    return response;
  }

  public string GetPointLocations(string type) {
    string response;
    string urlSuffix = LOCATION_SUFFIX + "/pointlocations?type=" + type;
    int code = Get(urlSuffix, out response);
    HandleReturnCode(code);
    return response;
  }

  //######################### ACCOUNT METHODS
  public string DeviceLogin(string deviceId) {
    string response;
    string urlSuffix = ACCOUNT_SUFFIX + "/login/" + deviceId;
    int code = Post(urlSuffix, "", out response);
    HandleReturnCode(code);
    return response;
  }

  public string Login(string data) {
    string response;
    string urlSuffix = ACCOUNT_SUFFIX + "/login";
    int code = Post(urlSuffix, data, out response);
    HandleReturnCode(code);
    return response;
  }

  public string Register(string data) {
    string response;
    string urlSuffix = ACCOUNT_SUFFIX;
    int code = Post(urlSuffix, data, out response);
    HandleReturnCode(code);
    return response;
  }

  public string GetAccountInfo(string emailAddress) {
    string response;
    string urlSuffix = ACCOUNT_SUFFIX + "/" + emailAddress;
    int code = Get(urlSuffix, out response);
    HandleReturnCode(code);
    return response;
  }

  //######################### COLOR METHODS
  public string GetColors() {
    string response;
    string urlSuffix = COLOR_SUFFIX + "/";
    int code = Get(urlSuffix, out response);
    HandleReturnCode(code);
    return response;
  }

}

[Serializable]
public class ConnectionResource {
  public string clientToken;

  public ConnectionResource(string clientToken) {
    this.clientToken = clientToken;
  }
}

[Serializable]
public class GameResource {
  public string token;
  public string roomName;
  public int maxTeams;
  public int maxPlayersPerTeam;
  public string hostPassword;

  public GameResource(string token, string roomName, int maxTeams, int maxPlayersPerTeam, string hostPassword) {
    this.token = token;
    this.roomName = roomName;
    this.maxTeams = maxTeams;
    this.maxPlayersPerTeam = maxPlayersPerTeam;
    this.hostPassword = hostPassword;
  }
}

[Serializable]
public class GameListResource {
  public string id;
  public string name;
}

[Serializable]
public class LoginResource {
  public string emailaddress;
  public string password;
}

[Serializable]
public class RegisterUserResource {
  public string clientID;
  public string name;
  public string password;
}