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
	public Item(string ItemName, int BuyPrice, int SellPrice, bool IsLegal)
	{
		m_ItemName = ItemName;
		m_BuyPrice = BuyPrice;
		m_SellPrice = SellPrice;
		m_IsLegal = IsLegal;
	}

	static public List<Item> ShopItems {
		get {
			List<Item> shopItems = new List<Item>();
			shopItems.Add(new Item("Ijs", 3, 4, true));
			shopItems.Add(new Item("Drugs", 30, 50, false));
			shopItems.Add(new Item("Koekjes", 6, 10, true));

			shopItems.Add(new Item("Diploma", 100, 120, false));
			shopItems.Add(new Item("Pizza", 10, 15, true));
			shopItems.Add(new Item("Orgaan", 50, 70, false));
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


