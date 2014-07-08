using UnityEngine;
using System.Collections;
using System;

public class FlameScript : MonoBehaviour {

    public static event Action FlameEntered;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
            if (FlameEntered!=null) {
                FlameEntered();
            }
		}
	}
}