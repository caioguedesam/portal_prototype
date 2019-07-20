using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    // Player Character controller reference
    public CharacterController controller;

    // Opposing portal reference
    public GameObject opposingPortal;
    public bool portalOn = false;

    private void OnTriggerEnter(Collider other) {
        if(other.tag != "Level" && opposingPortal.GetComponent<Portal>().portalOn == true) {
            controller.enabled = false;

            // This only moves portaled player to a position forward of the opposing portal
            // and makes it look forward to the portal.
            // Still have to do smooth movement (like in the actual game)
            controller.transform.position = opposingPortal.transform.position + opposingPortal.transform.forward * 2;
            controller.transform.rotation = Quaternion.LookRotation(opposingPortal.transform.forward);

            controller.enabled = true;
        }
    }

}
