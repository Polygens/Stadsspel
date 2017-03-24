using UnityEngine;

namespace Stadsspel.Districts
{
	public class HeadDistrict : District
	{
		[SerializeField]
		private SpriteRenderer m_Icon;

		[SerializeField]
		private Sprite m_ChestIcon;

		/// <summary>
		/// Initialises the class before Start.
		/// </summary>
		private new void Awake()
		{
			m_DistrictType = DistrictType.HeadDistrict;
			base.Awake();

		}

		/// <summary>
		/// Initialises the class.
		/// </summary>
		private void Start()
		{
			m_Icon.sprite = m_ChestIcon;
			transform.GetChild(0).tag = "Treasure";
		}

		/// <summary>
		/// Handles the change of team.
		/// </summary>
		protected override void OnTeamChanged()
		{
			base.OnTeamChanged();
			Color newColor = gameObject.GetComponent<Renderer>().material.color;
			newColor.a = 0.4f;
			gameObject.GetComponent<Renderer>().material.color = newColor;
		}
	}
}