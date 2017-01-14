using UnityEngine;

public class Building : Element
{
	protected new void Start()
	{
		base.Start();
		ActionRadius = 30;

		Renderer Renderer = GetComponent<Renderer>();
		Renderer.material.SetFloat("Opacity", 1f);
		Renderer.material.SetFloat("Thickness", 0f);
	}
}
