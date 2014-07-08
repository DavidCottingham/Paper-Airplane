using UnityEngine;
using System.Collections;

public class PusherAnimation : MonoBehaviour {

    public float opacity = 1f;
    private SpriteRenderer sr;
    private Color tempColor;

    void Start() {
        sr = GetComponent<SpriteRenderer>();
        tempColor = sr.color;
    }

	void LateUpdate() {
        tempColor = sr.color;
        tempColor.a = opacity;
        sr.color = tempColor;
    }
}
