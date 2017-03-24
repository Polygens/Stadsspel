using UnityEngine;

namespace Stadsspel.Districts
{
	public class HeadDistrict : District
	{
		[SerializeField]
		private SpriteRenderer m_Icon;

		[SerializeField]
		private Sprite m_ChestIcon;

		private new void Awake()
		{
			m_DistrictType = DistrictType.HeadDistrict;
			base.Awake();

		}

		private void Start()
		{
			m_Icon.sprite = m_ChestIcon;
			transform.GetChild(0).tag = "Treasure";
		}

		protected override void OnTeamChanged()
		{
			base.OnTeamChanged();
			Color newColor = gameObject.GetComponent<Renderer>().material.color;
			newColor.a = 0.4f;
			gameObject.GetComponent<Renderer>().material.color = newColor;
		}
	}
}