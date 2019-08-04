using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flight : MonoBehaviour
{

    Vector3 drag, thrust, gravity, lift, final;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // No need for "+=" when using rigidbody velocity instead of transform.position
    void Update()
    {
        //get final vector3 combining all 4 forces
        final = Drag() + Thrust() + Gravity() + Lift();

        //add this final vector3 to the plane
        rb.velocity = final;
    }

    Vector3 Drag() //Backwards force
    {
        return drag;
    }

    Vector3 Thrust() //Forward force
    {
        thrust = Vector3.forward; //Testing forward motion
        return thrust;
    }

    Vector3 Gravity() //Downwards force
    {
        return gravity;
    }

    Vector3 Lift() //Upwards force
    {
        return lift;
    }
}
