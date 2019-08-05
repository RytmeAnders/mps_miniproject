using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flight : MonoBehaviour
{
    /* TODO:
        - Introduce proper bounce in the plane (right now it just stops on the ground)
        - Create controls for pitch (for aoa)
        - Allow plane to rotate
    */

    // Public variables
    public float g = 9.8f;
    public float aircraftHeight = 1;
    public float mass = 1;

    [Range(0,1)]
    public float acceleration;

    [Range(0,1)]
    public float deceleration;

    // Private variables
    Vector3 F_drag, F_thrust, F_gravity, F_lift, F_final; //(F = force)
    Rigidbody rb;
    float speed;

    void Start()
    {
        speed = 0;
        rb = GetComponent<Rigidbody>();
    }

    // No need for "+=" when using rigidbody velocity instead of transform.position
    void Update()
    {
        // Get final vector3 combining all 4 forces
        F_final = CalculateThrust() + CalculateDrag() + CalculateGravity() + CalculateLift();

        // Add this final vector3 to the plane
        // TODO Scale forces by 1/m to get acceleration (Verth and Bishop, 2008: p.606)
        rb.velocity = F_final;
        print(speed);
    }

    //Forward force
    /// <summary>
    /// Seems to be an arbitrary vector given to the aircraft.
    /// Thrust == "velocity" of aircraft
    /// </summary>
    /// <returns></returns>
    Vector3 CalculateThrust()
    {
        // Speed up if key is down
        if(Input.GetKey(KeyCode.W))
        {
            speed += acceleration;
        }

        // Slow down if no button is held
        if(!Input.GetKey(KeyCode.W))
        {
            if(speed > 0)
            {
                speed -= deceleration;
            }
        }

        // At the moment our plane can only fly straight ahead
        F_thrust = Vector3.forward * speed;
        return F_thrust;
    }

    //Backwards force
    /// <summary>
    /// Fdrag = 1/2 * rho * Cd * v^2
    /// rho = air density,
    /// Cd = drag coefficient (exponential aoa),
    /// v = velocity of aircraft (thrust).
    /// - Parallel to velocity
    /// </summary>
    /// <returns></returns>
    Vector3 CalculateDrag()
    {
        return F_drag;
    }

    // Downwards force
    /// <summary>
    /// Gravity force = mg. Acts as an acceleration downwards.
    /// </summary>
    /// <returns></returns>
    Vector3 CalculateGravity()
    {
        if(transform.position.y >= aircraftHeight)
        {
            F_gravity.y -= mass * g * Time.deltaTime;
            return F_gravity;
        }

        F_gravity.y = 0;
        return F_gravity;
    }

    //Upwards force
    /// <summary>
    /// L = 1/2 * rho * Cl * v^2. Depends on thrust and angle of attack.
    /// rho = air density,
    /// Cl = lift coefficient (linear aoa between 0-15 degrees),
    /// v^2 = speed (velocity?) of aircraft (thrust).
    /// - Perpendicular to velocity.
    /// </summary>
    /// <returns></returns>
    Vector3 CalculateLift()
    {
        return F_lift;
    }
}
