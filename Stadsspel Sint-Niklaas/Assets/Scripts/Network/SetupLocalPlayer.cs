using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SetupLocalPlayer : NetworkBehaviour {

    [SyncVar] //From server to client
    public string pName = "player";

    [SyncVar]
    public Color playerColor = Color.white;

    void OnGUI()
    {
        if(isLocalPlayer)
        {
            pName = GUI.TextField(new Rect(25, Screen.height - 40, 100, 30), pName);
            if (GUI.Button(new Rect(130, Screen.height - 40, 80, 30), "Change"))
            {
                CmdChangeName(pName);
            }
        }       
    }

    [Command] //Tells Unity function will be called on the server
    public void CmdChangeName(string newName)
    {
        pName = newName;
    }
	
	void Start ()
    {
	    if(isLocalPlayer)
        {
            GetComponent<MovePlayer>().enabled = true;
        }

        Renderer[] rends = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rends)
        {
            r.material.color = playerColor;
        }

    }

    void Update()
    {
        GetComponentInChildren<TextMesh>().text = pName;
    }
}
