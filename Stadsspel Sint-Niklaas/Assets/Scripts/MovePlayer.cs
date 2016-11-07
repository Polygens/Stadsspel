using UnityEngine;
using System.Collections;

public class MovePlayer : MonoBehaviour {

    public float speed = 10.0f;
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        float translationV = Input.GetAxis("Vertical") * speed;
        float translationH = Input.GetAxis("Horizontal") * speed;

        translationV *= Time.deltaTime;
        translationH *= Time.deltaTime;

        GetComponent<Rigidbody2D>().velocity = new Vector2(translationH, translationV);
    }
}
