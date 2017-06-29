using System;
using System.Collections.Generic;
public class PlayerNotification
{
	//not serialized
	private IDictionary<string, int> _legalItems;
	private IDictionary<string, int> _illegalItems;
	private IDictionary<string, long> _visitedTradeposts;
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
	public IDictionary<string, int> IllegalItems {
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
	public IDictionary<string, long> VisitedTradeposts {
		get {
			if (_visitedTradeposts == null)
			{
				_visitedTradeposts = new Dictionary<string, long>();
				foreach (TradepostLock postLock in visitedTradeposts)
				{
					_visitedTradeposts.Add(postLock.tradepostId, postLock.unlockMoment);
				}
			}
			return _visitedTradeposts;
		}
	}

	//serialized
	public List<PlayerItemCount> illegalItems;
	public List<PlayerItemCount> legalItems;
	public List<TradepostLock> visitedTradeposts;
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