﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flight : MonoBehaviour
{
    /* TODO:
        - Introduce proper bounce in the plane (right now it just stops on the ground)
        - Determine the correct zero lift angle
    */

    // Public variables
    [Header("Aircraft parameters")]
    public float aircraftHeight = 1;
    public float maxSpeed;
    public float stall;

    [Header("Aircraft controls")]
    [Range(0,1)]
    public float pitchStrength;
    [Range(0,1)]
    public float rollStrength;
    [Range(0,1)]
    public float acceleration;
    [Range(0,1)]
    public float deceleration;

    [Header("Velocities")]
    public Vector3 F_drag;
    public Vector3 F_thrust;
    public Vector3 F_gravity;
    public Vector3 F_lift;
    public Vector3 F_final;

    // Private variables
    Rigidbody rb;
    float speed;
    float Cl;
    float pitch;
    float mass;
    float m;
    float roll;

    // Constants
    const float rho = 1.225f; //Standard air pressure at sea level
    const float g = 9.8f; //Gravity in newton on earth

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pitch = -1;
        speed = 0;
        roll = 0;
        mass = rb.mass;
        m = 0.05f;
    }

    // No need for "+=" when using rigidbody velocity instead of transform.position
    void Update()
    {
        ControlPitch();
        ControlRoll();
        CalculateAngleOfAttack();
        
        // Get final vector3 combining all 4 forces
        F_final = CalculateThrust() + CalculateDrag() + CalculateGravity() + CalculateLift();

        // Add this final vector3 to the plane
        // TODO Scale forces by 1/m to get acceleration (Verth and Bishop, 2008: p.606)
        rb.velocity = F_final;
    }

    #region Forces
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
        if(!Input.GetKey(KeyCode.W) && speed > 0)
        {
            speed -= deceleration;
        }

        // At the moment our plane can only fly straight ahead
        F_thrust = transform.forward * speed;
        return F_thrust;
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

        //Stop gravity if on ground
        F_gravity = new Vector3(0,0,0);
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
        F_lift = new Vector3(0,1,0);
        float L = 0.5f * rho * CalculateAngleOfAttack() * Mathf.Pow(speed,2);
        F_lift.y += L * Time.deltaTime;

        if(pitch <= -stall)
        {
            F_lift = new Vector3(0,0,0);
        }
        return F_lift;
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
        F_drag = new Vector3(0,0,1);
        float D = 0.5f * rho * CalculateAngleOfAttack() * Mathf.Pow(speed,2);
        F_drag = -transform.forward * D * Time.deltaTime;

        if(-F_drag.z > F_thrust.z)
        {
            F_drag.z = -F_thrust.z;
        }

        return F_drag;
    }

    #endregion

    #region Flight Control
    void ControlPitch()
    {
        // Pitch up
        if(Input.GetKey(KeyCode.E))
        {
            pitch -= pitchStrength;
        }

        // Pitch down
        if(Input.GetKey(KeyCode.Q))
        {
            pitch += pitchStrength;
        }
        transform.eulerAngles = new Vector3(pitch,-roll,roll*2);
    }

    void ControlRoll()
    {
        // Pitch up
        if(Input.GetKey(KeyCode.D))
        {
            roll -= rollStrength;
        }

        // Pitch down
        if(Input.GetKey(KeyCode.A))
        {
            roll += rollStrength;
        }
        transform.eulerAngles = new Vector3(pitch,-roll,roll*2);
    }

    float CalculateAngleOfAttack()
    {
        Vector3 normalVelocity = F_final.normalized;
        Vector3 flatForward = transform.forward;
        flatForward.y = 0;
        float aoa = Mathf.Acos(Vector3.Dot(flatForward,transform.forward)) * Mathf.Rad2Deg - 1;

        Debug.DrawRay(transform.position,flatForward*100,Color.red);
        Debug.DrawRay(transform.position,transform.forward*100,Color.green);
        
        // I propose that there is zero lift parallel to the ground
        Cl = 2 * m * (aoa-Vector3.forward.z);
        print(aoa);
        return Cl;
    }
    #endregion
}
