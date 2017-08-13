using System.Collections.Generic;

public enum Items : byte
{
	pizza,
	ijs,
	koekjes,
	orgaan,
	diploma,
	drugs
}

public class Item
{
	private int m_BuyPrice;
	private int m_SellPrice;
	private bool m_IsLegal;
	private string m_ItemName;

	/// <summary>
	/// Constructor, initialises the item.
	/// </summary>
	public Item(string ItemName, int BuyPrice, int SellPrice,bool IsLegal)
	{
		m_ItemName = ItemName;
		m_BuyPrice = BuyPrice;
		m_SellPrice = SellPrice;
		m_IsLegal = IsLegal;
	}

	static public List<Item> LegalShopItems {
		get {
			List<Item> shopItems = new List<Item>();
			shopItems.Add(new Item("Dozijn bloemen", 5, 6,true));
			shopItems.Add(new Item("Vat bier", 8, 10, true));
			shopItems.Add(new Item("Kg ijs", 10, 13, true));

			shopItems.Add(new Item("Rol textiel", 15, 20, true));
			shopItems.Add(new Item("Bakstenen", 20, 28, true));
			shopItems.Add(new Item("Art Deco Kunst", 29, 40, true));
			return shopItems;
		}
	}


	static public List<Item> IllegalShopItems
	{
		get
		{
			List<Item> shopItems = new List<Item>();
			shopItems.Add(new Item("Dozijn bloemen", 4, 5, false));
			shopItems.Add(new Item("Vat bier", 7, 9, false));
			shopItems.Add(new Item("Kg ijs", 8, 11, false));

			shopItems.Add(new Item("Rol textiel", 12, 17, false));
			shopItems.Add(new Item("Bakstenen", 17, 25, false));
			shopItems.Add(new Item("Art Deco Kunst", 25, 36, false));
			return shopItems;
		}
	}

	public string ItemName {
		get { return m_ItemName; }
		set { m_ItemName = value; }
	}

	public int BuyPrice {
		get { return m_BuyPrice; }
		set { m_BuyPrice = value; }
	}

	public bool IsLegal {
		get { return m_IsLegal; }
		set { m_IsLegal = value; }
	}

	public int SellPrice {
		get { return m_SellPrice; }
		set { m_SellPrice = value; }
	}
}


