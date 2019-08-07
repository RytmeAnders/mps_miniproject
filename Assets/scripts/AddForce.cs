using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForce : MonoBehaviour
{
    /* NOTES:
        - We only apply Angle Of Attack when applying thrust, otherwise when the plane
        starts stalling, its velocity goes nearly straight down, making the "angle of attack"
        extremely large because the forward vector is still pointing up.
     */

    // Public
    public int speed;
    public int torque;
    public float m;

    public Vector3 F_Lift;
    public Vector3 F_Drag;
    public float LiftMag;
    public float DragMag;

    // Private
    Rigidbody rb;
    float attack;

    // Constant
    const float g = 9.82f;
    const float rho = 1.225f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Pitch();
        Roll();
        ApplyGravity();

        ApplyThrust();
        ApplyLift();
        ApplyDrag();
    }

    void ApplyThrust()
    {
        if(Input.GetKey(KeyCode.W))
        {
            rb.AddForce(transform.forward * speed);
            attack = CalculateAngleOfAttack();
        }

        if(!Input.GetKey(KeyCode.W))
        {
            attack = 0.5f;
        }
    }

    void ApplyGravity()
    {
        rb.AddForce(-transform.up * rb.mass * g);
    }

    void ApplyLift()
    {
        float L = 0.5f * rho * attack * Mathf.Pow(rb.velocity.magnitude,2);
        Vector3 lift = transform.up * L;
        rb.AddForce(lift);
        //F_Lift = L;
        LiftMag = L;
    }

    void ApplyDrag()
    {
        float D = 0.5f * rho * attack * Mathf.Pow(rb.velocity.magnitude,2);
        Vector3 drag = -transform.forward * D;
        rb.AddForce(drag);
        //F_Drag = D;
        DragMag = D;
    }

    float CalculateAngleOfAttack()
    {
        //Normalize velocity so we only consider the cosine of the angle in the dot-product
        Vector3 normalVelocity = rb.velocity.normalized;

        //The -1cos of the dot-product provides the angle in radians
        float aoa = Mathf.Acos(Vector3.Dot(normalVelocity,transform.forward)) * Mathf.Rad2Deg;

        // I propose that there is zero lift parallel to the ground
        float Cl = 2 * m * (aoa - 1);

        // Draw the forward vector of the plane and its velocity for comparison
        Debug.DrawRay(transform.position,rb.velocity,Color.red);
        Debug.DrawRay(transform.position,transform.forward,Color.green);
    
        Cl = Mathf.Abs(Cl);
        return Cl;
    }

    void Pitch()
    {
        // Pitch up
        if(Input.GetKey(KeyCode.E))
        {
            rb.AddTorque(-transform.right * torque);
        }

        // Pitch down
        if(Input.GetKey(KeyCode.Q))
        {
            rb.AddTorque(transform.right * torque);
        }
    }

    void Roll()
    {
        // Roll right
        if(Input.GetKey(KeyCode.D))
        {
            rb.AddTorque(-transform.forward * torque);
        }

        // Pitch left
        if(Input.GetKey(KeyCode.A))
        {
            rb.AddTorque(transform.forward * torque);
        }
    }

    Vector3 squareVector(Vector3 v)
    {
        Vector3 squared = new Vector3(Mathf.Pow(v.x,2),Mathf.Pow(v.y,2),Mathf.Pow(v.z,2));
        return squared;
    }
}
