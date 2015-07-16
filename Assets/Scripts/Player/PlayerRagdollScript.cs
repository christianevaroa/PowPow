using UnityEngine;
using System.Collections;

/// <summary>
/// This script is (going to be...) used for switching the ragdoll rigidbodies kinematic on/off
/// TODO: disable Animator, disable main character capsule rigidbody
/// </summary>
public class PlayerRagdollScript : MonoBehaviour {

    Rigidbody[] rbs;

	// Use this for initialization
	void Start () {
        rbs = GetComponentsInChildren<Rigidbody>();
        SetAllKinematic(false);
	}

    public void SetAllKinematic(bool b)
    {
        foreach (Rigidbody rb in rbs)
        {
            rb.isKinematic = b;
        }
    }
}
