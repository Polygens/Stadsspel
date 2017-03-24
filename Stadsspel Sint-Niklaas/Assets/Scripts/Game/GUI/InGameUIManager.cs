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

	[SerializeField]
	private RectTransform m_LogNotifications;

	[SerializeField]
	private LogUI m_LogUI;

	[SerializeField]
	private FinalScoreUI m_FinalScoreUI;

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

	public RectTransform LogNotifications {
		get {
			return m_LogNotifications;
		}
	}

	public LogUI LogUI {
		get {
			return m_LogUI;
		}
	}

	public InstructionsUI InstructionsUI {
		get {
			return m_InstructionsUI;
		}
	}

	public FinalScoreUI FinalScoreUI {
		get { return m_FinalScoreUI; }
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
