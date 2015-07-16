using UnityEngine;
using System.Collections;
using PlayerScripts;

public enum CarriedState { NOT_CARRIED, CARRIED }
public enum ThrownState { NOT_THROWN, THROWN }

public class Throwable : MonoBehaviour {

    public Collider myCollider;
    GameObject holder;
    CarriedState beingCarried;
    ThrownState beenThrown;
    Rigidbody rb;
    float height;

    public Transform leftIKHandle;
    public Transform rightIKHandle;

    string thrower;

	// Use this for initialization
	void Start () {
        beingCarried = CarriedState.NOT_CARRIED;
        rb = GetComponent<Rigidbody>();

        if (leftIKHandle == null || rightIKHandle == null)
        {
            Debug.LogWarning("IK handles are not set for " + gameObject.name);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (beenThrown == ThrownState.THROWN)
        {
            CheckGrounded();
        }
	}

    void CheckGrounded()
    {
        if (rb.velocity.sqrMagnitude < 0.0001f)
        {
            beenThrown = ThrownState.NOT_THROWN;
        }
    }

    public void BeInteractedWith(GameObject actor, Transform pos)
    {
        if (beingCarried == CarriedState.NOT_CARRIED)
        {
            // Being picked up
            beingCarried = CarriedState.CARRIED;
            rb.isKinematic = true;
            holder = actor;
            transform.position = pos.position;
            transform.rotation = actor.transform.rotation;
            transform.parent = pos;
        }
    }

    public void GetThrown(Vector3 direction)
    {
        if (beingCarried == CarriedState.CARRIED)
        {
            beenThrown = ThrownState.THROWN;
            beingCarried = CarriedState.NOT_CARRIED;
            transform.parent = null;
            rb.isKinematic = false;
            rb.AddForce(direction, ForceMode.VelocityChange);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (beenThrown == ThrownState.THROWN)
        {
            if (other.collider.gameObject.tag == "Player")
            {
                PlayerStatus otherPlayer = other.collider.gameObject.GetComponent<PlayerStatus>();
                otherPlayer.TakeDamage(new Damage(10, DamageType.PHYSICAL));
            }
        }
    }
}
