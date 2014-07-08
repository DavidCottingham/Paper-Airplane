using UnityEngine;
using System.Collections;

public class PlayerHazard : MonoBehaviour {

    void OnEnable() {
        FlameScript.FlameEntered += FlameEntered;
    }
    
    void OnDisable() {
        FlameScript.FlameEntered -= FlameEntered;
    }

    void FlameEntered() {
        Invoke("Die", 1.5f);
    }
    
    public void Die() {
        Destroy(gameObject);
    }
}
