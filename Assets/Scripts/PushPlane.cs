using UnityEngine;
using System.Collections;
using System;

[RequireComponent (typeof(BoxCollider2D))]
public class PushPlane : MonoBehaviour {

    //default push direction
    public Vector2 pushDirection = Vector2.up;
    //force is speed * direction, calc'ed in Start
    private Vector2 pushForce;
    public float minPush = 1f;
    //default push speed
    public float pushSpeed = 3.0f;
    public float maxPlayerDistance = 5;

    //Events
	public static event Action<Vector2> Push;
	public static event Action StopPush;

	void Start() {
		pushForce = pushDirection * pushSpeed;
        maxPlayerDistance = GetSingleVector(GetComponent<BoxCollider2D>().size);
	}

	void Update() {
        //DEBUG_OUT
		if (GetComponent<TextMesh>() != null) {
            GetComponent<TextMesh>().text = pushForce.ToString();
        }
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			if (Push != null) {
				Push(pushForce);
			}
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Player") {
			if (StopPush != null) {
				StopPush();
			}
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.tag == "Player") {
            //direction to target = target - me
            Vector2 directionToPlayer = other.transform.position - transform.position;
            ScalePushForce(directionToPlayer);
			other.attachedRigidbody.AddForce(pushForce);
            DebugGUI.UpdateMessage(20, pushForce.ToString());
		}
	}

    float GetSingleVector(Vector2 v2) {
        if (pushDirection == Vector2.up || pushDirection == -Vector2.up) {
            return v2.y;
        } else if (pushDirection == Vector2.right || pushDirection == -Vector2.right) {
            return v2.x;
        }
        return 0.0f;
    }

    float GetSingleVector() {
        return GetSingleVector(pushForce);
    }

    void ScalePushForce(Vector2 playerDirection) {
        float playerDirMagnitude = GetSingleVector(playerDirection);
        DebugGUI.UpdateMessage(21, playerDirMagnitude.ToString());
        DebugGUI.UpdateMessage(10, maxPlayerDistance.ToString());
        float t = Mathf.Abs(playerDirMagnitude) / maxPlayerDistance;
        pushForce = Mathf.Lerp(pushSpeed, minPush, t) * pushDirection;
    }
}