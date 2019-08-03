using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flight : MonoBehaviour
{

    Vector3 drag, thrust, gravity, lift, final;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //get final vector3 combining all 4 forces
        final = Drag() + Thrust() + Gravity() + Lift();

        //TODO add this final vector3 to the plane

    }

    Vector3 Drag() //Backwards force
    {
        return drag;
    }

    Vector3 Thrust() //Forward force
    {
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
