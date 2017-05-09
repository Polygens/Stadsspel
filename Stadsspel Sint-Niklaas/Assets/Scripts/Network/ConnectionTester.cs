using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Random = UnityEngine.Random;

[RequireComponent(typeof(WebsocketImpl))]
public class ConnectionTester : MonoBehaviour {
  const string socketAddress = "ws://localhost:8090/user";
  //var socketAddress = "ws://stniklaas-stadsspel.herokuapp.com/user";
  //var api = "stniklaas-stadsspel.herokuapp.com";
  const string api = "localhost:8090";
  const string pw = "PW";
  const string pwhost = "PWHOST";

  private string clientID;
  private string gameID;
  private string tokenClient;
  private string tokenWeb;
  private WebsocketImpl ws;
  //var wsWeb;
  private string hostToken;
  private static bool stop = false;

  private Rest rest = new Rest();
  private const string TEST_URL = "https://postman-echo.com/";


  // Use this for initialization
  void Start() {
    ws = gameObject.GetComponent<WebsocketImpl>();
  }

  //########################################################################

  public void TestSout() {
    Debug.Log("TEST");
  }

  public void TestGet() {
    Debug.Log("TESTING GET");
    ServicePointManager.ServerCertificateValidationCallback = Rest.MyRemoteCertificateValidationCallback;
    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(TEST_URL + "Get?test=123");
    // Set the Method property of the request to POST.  
    request.Method = "GET";
    request.AutomaticDecompression = DecompressionMethods.None;//todo add decompression method for lzString 
                                                               // Get the response.  
    WebResponse response = request.GetResponse();
    // Display the status.
    Debug.Log((int)((HttpWebResponse)response).StatusCode);
    Debug.Log(((HttpWebResponse)response).StatusDescription);
    // Get the stream containing content returned by the server.  
    Stream dataStream = response.GetResponseStream();
    // Open the stream using a StreamReader for easy access.  
    StreamReader reader = new StreamReader(dataStream);
    // Read the content.  
    string responseFromServer = reader.ReadToEnd();
    // Display the content.  
    Debug.Log(responseFromServer);
    // Clean up the streams.  
    reader.Close();
    dataStream.Close();
    response.Close();
  }

  public void TestPost() {
    Debug.Log("TESTING POST");
    ServicePointManager.ServerCertificateValidationCallback = Rest.MyRemoteCertificateValidationCallback;
    // Create a request using a URL that can receive a Post.   
    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(TEST_URL + "Post");
    // Set the Method property of the request to POST.  
    request.Method = "POST";
    request.AutomaticDecompression = DecompressionMethods.None;//todo add decompression method for lzString
                                                               // Create POST data and convert it to a byte array.  
    string postData = "This is a test that posts this string to a Web server.";
    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
    // Set the ContentType property of the WebRequest.  
    request.ContentType = "text/plain";
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
    // Display the status.
    Debug.Log((int)((HttpWebResponse)response).StatusCode);
    Debug.Log(((HttpWebResponse)response).StatusDescription);
    // Get the stream containing content returned by the server.  
    dataStream = response.GetResponseStream();
    // Open the stream using a StreamReader for easy access.  
    StreamReader reader = new StreamReader(dataStream);
    // Read the content.  
    string responseFromServer = reader.ReadToEnd();
    // Display the content.  
    Debug.Log(responseFromServer);
    // Clean up the streams.  
    reader.Close();
    dataStream.Close();
    response.Close();
  }

  public void TestPut() {
    Debug.Log("TESTING PUT");
    ServicePointManager.ServerCertificateValidationCallback = Rest.MyRemoteCertificateValidationCallback;
    // Create a request using a URL that can receive a Post.   
    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(TEST_URL + "Put");
    // Set the Method property of the request to POST.  
    request.Method = "PUT";
    request.AutomaticDecompression = DecompressionMethods.None;//todo add decompression method for lzString
                                                               // Create POST data and convert it to a byte array.  
    string postData = "This is a test that posts this string to a Web server.";
    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
    // Set the ContentType property of the WebRequest.  
    request.ContentType = "text/plain";
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
    // Display the status.
    Debug.Log((int)((HttpWebResponse)response).StatusCode);
    Debug.Log(((HttpWebResponse)response).StatusDescription);
    // Get the stream containing content returned by the server.  
    dataStream = response.GetResponseStream();
    // Open the stream using a StreamReader for easy access.  
    StreamReader reader = new StreamReader(dataStream);
    // Read the content.  
    string responseFromServer = reader.ReadToEnd();
    // Display the content.  
    Debug.Log(responseFromServer);
    // Clean up the streams.  
    reader.Close();
    dataStream.Close();
    response.Close();
  }

  public void SetupGame() {
    stop = false;
    StartCoroutine(coSetupGame());
  }

  public void RunClient() {
    StartCoroutine(coRunClient());
  }

  public void StartGame()
  {
    StartCoroutine(coStartGame());
  }

  public void StopGame()
  {
    stop = true;
    StartCoroutine(coStopGame());
  }

  //########################################################################

  IEnumerator coSetupGame() {
    LoginHost();
    yield return new WaitForSeconds(1);
    CreateGame();
    yield return 0;
  }

  IEnumerator coRunClient() {
    RegisterPlayer();
    yield return new WaitForSeconds(1);
    yield return StartCoroutine(OpenWebsocketPlayer());
    yield return new WaitForSeconds(1);
    while (!stop) {
      SendLocationMessage();
      yield return new WaitForSeconds(5);
    }
  }

  IEnumerator coStartGame()
  {
    Debug.Log("###################### START GAME ############################");
    string data = rest.StartGame(gameID, hostToken);
    Debug.Log(data);
    yield return 0;
  }

  IEnumerator coStopGame()
  {
    Debug.Log("###################### STOP GAME ############################");
    string data = rest.StopGame(gameID,hostToken);
    Debug.Log(data);
    yield return 0;
  }

  private void SendLocationMessage() {
    Debug.Log("###################### SENDING LOCATION ############################");
    LocationMessage lm = new LocationMessage(51.0 + Random.Range(0.0f, 1.0f), 4.0 + Random.Range(0.0f, 1.0f));
    var submessage = JsonUtility.ToJson(lm);
    Debug.Log(submessage);
    Debug.Log(lm.latitude + " " + lm.longitude);
    ws.send(GameMessageType.LOCATION ,submessage);
  }

  private IEnumerator OpenWebsocketPlayer() {
    Debug.Log("###################### OPEN SOCKET ############################");
    Debug.Log(ws);
    yield return StartCoroutine(ws.Connect(socketAddress,gameID,clientID));
    yield return 0;
  }

  private void RegisterPlayer() {
    Debug.Log("###################### REGISTER PLAYER ############################");
    clientID = "" + Math.Floor(Random.Range(0.0f, 1.0f) * 1000000);
    var obj = "{\"clientID\": \"" + clientID + "\",\"name\": \"tim\",\"password\":\"" + pw + "\"}";
    Debug.Log(obj);
    string data = rest.RegisterPlayer(obj, gameID);
    ConnectionResource connectionResource = JsonUtility.FromJson<ConnectionResource>(data);
    tokenClient = connectionResource.clientToken;
  }

  private void CreateGame() {
    Debug.Log("###################### NEW GAME ############################");
    GameResource gameResource = new GameResource(hostToken, "room of rooms", 3, 5, pw);
    var obj = JsonUtility.ToJson(gameResource);
    Debug.Log(obj);
    Debug.Log(gameResource);
    gameID = rest.NewGame(obj);
    Debug.Log(gameID);
  }

  private void LoginHost() {
    Debug.Log("###################### LOGIN HOST ############################");
    var obj = "{\"emailaddress\":\"default@host.com\",\"password\":\"test\"}";
    Debug.Log(obj);
    string data = rest.Login(obj);
    Debug.Log(data);
    hostToken = data;
  }
}
