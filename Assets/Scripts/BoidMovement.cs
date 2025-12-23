using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidMovement : MonoBehaviour
{
    private float forwardSpeed = 5f;
    public Vector3 velocity { get; private set; }
    private void FixedUpdate()
    {
        velocity = Vector2.Lerp(velocity, transform.forward.normalized * forwardSpeed, Time.fixedDeltaTime);
        transform.position += velocity * Time.fixedDeltaTime;
        LookRotation();
    }
    private void LookRotation()
    {
        transform.rotation = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation(velocity), Time.fixedDeltaTime);
    }    
}
