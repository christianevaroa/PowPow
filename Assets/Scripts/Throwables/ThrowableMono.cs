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

    string thrower;

	// Use this for initialization
	void Start () {
        beingCarried = CarriedState.NOT_CARRIED;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (beingCarried == CarriedState.CARRIED)
        {
            // TODO: change position to be based upon size of object
        }
	}

    void CheckGrounded()
    {

    }

    public void BeInteractedWith(GameObject actor, Transform pos)
    {
        if (beingCarried == CarriedState.NOT_CARRIED)
        {
            beingCarried = CarriedState.CARRIED;
            holder = actor;
            transform.position = pos.position;
            transform.parent = pos;
        }
    }
}
