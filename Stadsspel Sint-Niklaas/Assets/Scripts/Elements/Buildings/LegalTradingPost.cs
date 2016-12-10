using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class LegalTradingPost : TradingPost
{
	public Item Pizza = new Item("Pizza", 10, 15, true);
	public Item IceCream = new Item("Ijs", 2.5f, 4f, true);
	public Item Cookies = new Item("Koekjes", 6f, 10f, true);

  /*[SyncVar]
  private TeamID[] usedByTeams;*/    // -> Cannot be an array

  public void AddGoodsToPlayer()
  {
    throw new System.NotImplementedException();
  }

  //For drop down or inputField
  public void UpdateNumberOfGoods(int number)
  {
    throw new System.NotImplementedException();
  }

  

	public LegalTradingPost()
	{
		throw new System.NotImplementedException();
	}
}