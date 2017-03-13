using UnityEngine;

public class InGameUIManager : MonoBehaviour
{
	static public InGameUIManager s_Singleton;

	[SerializeField]
	private RectTransform m_PriorityButtons;

	[SerializeField]
	private UserPanelUI m_UserPanelUI;

	[SerializeField]
	private TradingPostUI m_TradingPostUI;

	[SerializeField]
	private TreasureUI m_TreasureUI;

	[SerializeField]
	private TreasureEnemyUI m_TreasureEnemyUI;

	[SerializeField]
	private GrandMarketUI m_GrandMarketUI;

	[SerializeField]
	private GoodsUI m_GoodsUI;

	[SerializeField]
	private BankUI m_BankUI;

	[SerializeField]
	private RectTransform m_Panels;

	[SerializeField]
	private InstructionsUI m_InstructionsUI;

	public RectTransform PriorityButtons {
		get {
			return m_PriorityButtons;
		}
	}

	public UserPanelUI UserPanelUI {
		get {
			return m_UserPanelUI;
		}
	}

	public TradingPostUI TradingPostUI {
		get {
			return m_TradingPostUI;
		}
	}

	public TreasureUI TreasureUI {
		get {
			return m_TreasureUI;
		}
	}

	public TreasureEnemyUI TreasureEnemyUI {
		get {
			return m_TreasureEnemyUI;
		}
	}

	public GrandMarketUI GrandMarketUI {
		get {
			return m_GrandMarketUI;
		}
	}

	public GoodsUI GoodsUI {
		get {
			return m_GoodsUI;
		}
	}

	public BankUI BankUI {
		get {
			return m_BankUI;
		}
	}

	public RectTransform Panels {
		get {
			return m_Panels;
		}
	}

	public InstructionsUI InstructionsUI {
		get {
			return m_InstructionsUI;
		}
	}

	private void Start()
	{
		if(s_Singleton != null) {
			Destroy(this);
			return;
		}
		s_Singleton = this;
	}
}
