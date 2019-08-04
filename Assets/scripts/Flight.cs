using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flight : MonoBehaviour
{
    // Public variables
    public float gravityCoeff = 9.8f;
    public float aircraftHeight = 1;

    // Private variables
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
        // Get final vector3 combining all 4 forces
        final = Drag() + Thrust() + CalculateGravity() + Lift();

        // Add this final vector3 to the plane
        rb.velocity = final;
        print(rb.velocity);
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

    // Downwards force
    /// <summary>
    /// Subtract gravity coefficient (9.8 m/s^2) every frame if above ground.
    /// This will accelerate downwards force over time.
    /// </summary>
    /// <returns></returns>
    Vector3 CalculateGravity()
    {
        if(transform.position.y > aircraftHeight)
        {
            gravity.y -= gravityCoeff * Time.deltaTime;
            return gravity;
        }

        gravity.y = 0;
        return gravity;
    }

    Vector3 Lift() //Upwards force
    {
        return lift;
    }
}
