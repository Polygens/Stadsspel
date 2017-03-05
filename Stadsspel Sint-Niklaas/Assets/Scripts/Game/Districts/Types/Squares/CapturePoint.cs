using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CapturePoint : Square
{
	private new void Start()
	{
		base.Start();
		Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
		rigidbody.isKinematic = true;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		// TO DO
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		// TO DO
	}
}
