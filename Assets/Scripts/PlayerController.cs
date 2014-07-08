using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

    private enum Directions {Left, Right, None};

    //important to note that this boolean shows intention to move while transitioning (as well as current move direction).
    //it is changed by player controls or automatically when being pushed by opposing force. to get an time-accurate move direction, use ForceDirection()
    private bool moveRight = true;
    private bool crashed = false;

	public float forwardMotionForce = 12f;
    private float opposingForceThreshold = 0.3f;

	private bool beingPushed = false;
    private Vector2 pushingForce = Vector2.zero;

    private Animator animator;

    //fForce in class scope so GC doesn't have to clear a bunch of unused vars since it would be created and destroyed every physics update. dunno if neccessary though
    private float fForce;

    private float flipCooldown = 1f;
    private float lastFlip;

	void OnEnable() {
		PushPlane.Push += PlanePushed;
		PushPlane.StopPush += ExitPush;
	}

	void OnDisable() {
		PushPlane.Push -= PlanePushed;
		PushPlane.StopPush -= ExitPush;
	}

    void Start() {
        animator = GetComponent<Animator>();
        //lastFlip = Time.time;
    }

	void FixedUpdate() {
        if (!crashed) {
    		fForce = forwardMotionForce;
            animator.SetFloat("Speed",Mathf.Abs(rigidbody2D.velocity.x));
            animator.SetBool("Facing Right", (ForceXDirection() == Directions.Right ? true : false));
            //if being pushed and being pushed horizonally (don't need to try to flip when pushed by candle) and within threshold
            if (beingPushed && !OnFlipCooldown() && (ForceXDirection(pushingForce) != Directions.None) && (Mathf.Abs(rigidbody2D.velocity.x) <= opposingForceThreshold)) {
                //AND currently moving opposite direction of force (not yet flipped already)
                //this last check important so while transitioning (e.g. from -force through 0 to +force), boolean doesn't flip constantly and possibly (and likely) end up flipped wrong
                if ((ForceXDirection() == Directions.Right && !moveRight) || (ForceXDirection() == Directions.Left && moveRight) || ForceXDirection() == Directions.None) {
                    Flip();
                }
            }

    		//limit minimum velocity in X
    		rigidbody2D.AddForce(new Vector2(moveRight ? fForce : -fForce, 0.0f));
        }
	}

	void Update() {
        if (Input.GetKeyDown("escape")) {
            Application.Quit();
        }
        if (!crashed) {
            if (!beingPushed || ForceXDirection(pushingForce) == Directions.None) {
        		if ((Input.GetKey("right") && !moveRight) || (Input.GetKey("left") && moveRight)){
        			Flip();
        		}
            }
        }
        GUIupdate();
	}

	void Flip() {
        moveRight = !moveRight; //reversing force handled by fixed update constant adding force
        if (moveRight) { animator.SetTrigger("Turn Right"); }
        else if (!moveRight) { animator.SetTrigger("Turn Left"); }
        lastFlip = Time.time;
	}

	void PlanePushed(Vector2 force) {
        pushingForce = force;
		beingPushed = true;
	}

	void ExitPush() {
        pushingForce = Vector2.zero;
		beingPushed = false;
	}

    Directions ForceXDirection(float xVel) {
        if (xVel > 0.25f ) return Directions.Right;
        else if (xVel < -0.25f) return Directions.Left;
        else return Directions.None;
    }

    Directions ForceXDirection(Vector2 force) {
        return ForceXDirection(force.x);
    }
    
    Directions ForceXDirection() {
        return ForceXDirection(rigidbody2D.velocity.x);
    }

    void GUIupdate() {
        DebugGUI.UpdateMessage(1, rigidbody2D.velocity.ToString());
        DebugGUI.UpdateMessage(2, Mathf.Abs(rigidbody2D.velocity.x).ToString());
        DebugGUI.UpdateMessage(3, beingPushed ? "Being pushed" : "Not being pushed");
        DebugGUI.UpdateMessage(4, ForceXDirection().ToString());
        DebugGUI.UpdateMessage(5, ForceXDirection(pushingForce).ToString());
        DebugGUI.UpdateMessage(6, moveRight ? "right" : "left");
    }

    bool OnFlipCooldown(){
        if (Time.time - lastFlip > flipCooldown) return false;
        return true;
    }

    void OnCollisionEnter2D(Collision2D coll) {
        if (coll.collider.tag == "Obstacle") {
            crashed = true;
            rigidbody2D.fixedAngle = false;
            Invoke("Die", 1.5f);
        }
    }

    void Die() {
        Destroy(gameObject);
    }
}