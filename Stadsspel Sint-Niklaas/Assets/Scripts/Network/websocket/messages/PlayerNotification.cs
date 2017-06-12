using System;
using System.Collections.Generic;
public class PlayerNotification
{
	//not serialized
	private IDictionary<string, int> _legalItems;
	private IDictionary<string, int> _illegalItems;
	public IDictionary<string, int> LegalItems
	{
		get
		{
			if (_legalItems == null)
			{
				_legalItems = new Dictionary<string, int>();
				foreach (PlayerItemCount playerItemCount in legalItems)
				{
					_legalItems.Add(playerItemCount.item,playerItemCount.count);
				}
			}
			return _legalItems;
		}
	}
	public IDictionary<string, int> IllegalItems
	{
		get {
			if (_illegalItems == null)
			{
				_illegalItems = new Dictionary<string, int>();
				foreach (PlayerItemCount playerItemCount in illegalItems)
				{
					_illegalItems.Add(playerItemCount.item, playerItemCount.count);
				}
			}
			return _illegalItems;
		}
	}

	//serialized
	public List<PlayerItemCount> illegalItems;
	public List<PlayerItemCount> legalItems;
	public double money;

	[Serializable]
	public class PlayerItemCount
	{
		public string item;
		public int count;

		public PlayerItemCount(string item, int count)
		{
			this.item = item;
			this.count = count;
		}
	}
}