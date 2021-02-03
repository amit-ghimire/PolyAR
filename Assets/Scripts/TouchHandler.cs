using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchHandler : MonoBehaviour
{
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) 
        {
            Plane follower = new Plane(Camera.main.transform.forward * -1, this.transform.position);

            Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayDistance;
            if (follower.Raycast(mRay, out rayDistance))
                this.transform.position = mRay.GetPoint(rayDistance);
        }
    }
}
