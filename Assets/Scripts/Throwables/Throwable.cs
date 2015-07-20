using UnityEngine;
using System.Collections;
using PlayerScripts;

public enum CarriedState { NOT_CARRIED, CARRIED }
public enum ThrownState { NOT_THROWN, THROWN }

public class Throwable : MonoBehaviour {

    public AudioClip impactClip;
    AudioSource audioSource;
    public Collider myCollider;
    GameObject holder;
    CarriedState beingCarried;
    ThrownState beenThrown;
    Rigidbody rb;
    float height;

    public Transform leftIKHandle;
    public Transform rightIKHandle;

    public int damageAmount;

    string thrower;

	// Use this for initialization
	void Start () {
        beingCarried = CarriedState.NOT_CARRIED;
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

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

    /// <summary>
    /// Check if the Throwable is grounded
    /// TODO: implement a damage cooldown timer (sometimes players get collided with multiple times very quickly)
    /// </summary>
    void CheckGrounded()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f))
        {
            if (hit.collider.gameObject.tag == "LevelGeometry" && rb.velocity == Vector3.zero)
            {
                beenThrown = ThrownState.NOT_THROWN;
                holder = null;
            }
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
            if (other.collider.gameObject.tag == "Player" && other.collider.gameObject != holder)
            {
                audioSource.PlayOneShot(impactClip);
                PlayerStatus otherPlayer = other.collider.gameObject.GetComponent<PlayerStatus>();
                otherPlayer.TakeDamage(new Damage(damageAmount, DamageType.PHYSICAL));
            }
        }
    }

    void OnAnimatorIK()
    {

    }
}
