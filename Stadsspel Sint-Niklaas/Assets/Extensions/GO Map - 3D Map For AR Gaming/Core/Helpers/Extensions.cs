using UnityEngine;

namespace Assets.Helpers
{
	public static class Extensions
	{
		public static Vector2 ToVector2xz(this Vector3 v)
		{
			return new Vector2(v.x, v.z);
		}
	}
}
