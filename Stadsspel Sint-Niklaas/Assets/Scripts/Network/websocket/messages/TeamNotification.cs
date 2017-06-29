using System.Collections.Generic;

public class TeamNotification
{
	private IDictionary<string, long> _visitedTradeposts;

	public List<AreaLocation> districts;
	public double treasury;
	public double bankAccount;
	public double totalPlayerMoney;
	public List<string> tradeposts;

	public List<TradepostLock> visitedTradeposts;
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
}