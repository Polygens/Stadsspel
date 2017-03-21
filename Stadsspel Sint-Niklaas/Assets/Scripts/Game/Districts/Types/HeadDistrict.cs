using UnityEngine;

namespace Stadsspel.Districts
{
	public class HeadDistrict : District
	{
        public SpriteRenderer icon;
        public Sprite chestIcon;

		private new void Awake()
		{
			m_DistrictType = DistrictType.HeadDistrict;
			base.Awake();
            
		}

        private void Start()
        {
            icon.sprite = chestIcon;
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