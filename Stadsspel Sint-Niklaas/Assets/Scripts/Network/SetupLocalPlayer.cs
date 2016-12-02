using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SetupLocalPlayer : NetworkBehaviour
{

	[SyncVar] //From server to client
	public string pName = "player";

	[SyncVar]
	public Color playerColor = Color.white;

    [SyncVar]
    public TeamID teamID = 0;

	[Command] //Tells Unity function will be called on the server
	public void CmdUpdateData(string newName, TeamID newID)
	{
		pName = newName;
        teamID = newID;
	}

	void Start()
	{
		if (isLocalPlayer) {
			GetComponent<MoveAvatar>().enabled = true;
            CmdUpdateData(pName, teamID);
        }

		Renderer[] rends = GetComponentsInChildren<Renderer>();
		foreach (Renderer r in rends) {
			r.material.color = playerColor;
		}
        GetComponentInChildren<TextMesh>().text = pName;
    }

}
