using UnityEngine;

public class AutoOrient : MonoBehaviour
{

	private void LateUpdate()
	{
		transform.rotation = Camera.main.transform.rotation;
	}
}
