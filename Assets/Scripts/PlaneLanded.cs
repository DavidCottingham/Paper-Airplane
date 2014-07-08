using UnityEngine;
using System.Collections;

public class PlaneLanded : MonoBehaviour {

    public const float landedTimeLimit = 1.0f;
    private float timeEntered;

	void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Environment") {
            timeEntered = Time.time;
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if (other.tag == "Environment") {
            if (Time.time - timeEntered > landedTimeLimit) {
                transform.parent.gameObject.GetComponent<PlayerHazard>().Die();
            }
        }
    }
}
