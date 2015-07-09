using UnityEngine;
using System.Collections;

public enum CarriedState { NOT_CARRIED, CARRIED }
public enum ThrownState { NOT_THROWN, THROWN }

public class ThrowableMono : MonoBehaviour {

    public Collider collider;
    public IThrowable throwableScript;
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
        Debug.Log("transform.parent: " + transform.parent);
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
            if (rb.velocity.sqrMagnitude == 0f)
            {
                beenThrown = ThrownState.NOT_THROWN;
            }
        }
	}

    void CheckGrounded()
    {

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
            transform.parent = pos;
            Debug.Log("transform.parent: " + transform.parent);
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
            Debug.Log("transform.parent: " + transform.parent);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (beenThrown == ThrownState.THROWN)
        {

        }
    }
}
