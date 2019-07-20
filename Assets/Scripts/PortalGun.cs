using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalGun : MonoBehaviour
{
    // Player camera reference
    public Camera cam;

    // Left/Right portal references
    public GameObject leftPortal;
    public GameObject rightPortal;

    private void Update() {
        
        if(Input.GetButtonDown("Fire1"))
            ThrowPortal(leftPortal);
        else if(Input.GetButtonDown("Fire2"))
            ThrowPortal(rightPortal);
    }

    private void ThrowPortal(GameObject portal) {

        portal.GetComponent<Portal>().portalOn = true;
        RaycastHit hit;
        LayerMask portalable = 1 << 8;

        // Casts a ray on main camera center that checks if the object hit is valid to portal
        // (at the moment just the player isn't valid) and throws the portal
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, portalable)) {
            if(hit.transform.tag != "Player") {
                portal.transform.position = hit.point;
                portal.transform.rotation = Quaternion.LookRotation(hit.normal);
            }
        }
    }
}
