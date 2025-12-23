using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class BoidMovement : MonoBehaviour
{
    [SerializeField] private ListBoidVariable boids;
    private float radius = 2f;
    private float forwardSpeed = 5f;
    private float visionAngle = 270f;
    private float turnSpeed = 10f;
    public Vector3 velocity { get; private set; }
    private void FixedUpdate()
    {
        velocity = Vector2.Lerp(velocity, CalculateVelocity(), turnSpeed / 2 * Time.fixedDeltaTime);
        transform.position += velocity * Time.fixedDeltaTime;
        LookRotation();
    }
    private Vector2 CalculateVelocity()
    {
        var boidsInRange = BoidsInRange();
        Vector2 velocity = ((Vector2)transform.forward 
            + 1.7f * Separation(boidsInRange)
            + 0.1f * Alignment(boidsInRange)
            + Cohesion(boidsInRange)).normalized * forwardSpeed;
        return velocity;
    }
    private void LookRotation()
    {
        transform.rotation = Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation(velocity), turnSpeed * Time.fixedDeltaTime);
    }    
    private List<BoidMovement> BoidsInRange()
    {
        var listBoid = boids.boidMovements.FindAll(boid => boid != this 
        && (boid.transform.position - transform.position).magnitude <= radius 
        && InVisionCone(boid.transform.position));
        return listBoid;
    }    
    private bool InVisionCone(Vector2 position)
    {
        Vector2 directionToPosition = position - (Vector2)transform.position;
        float dotProduct = Vector2.Dot(transform.forward, directionToPosition);
        float cosHalfVisionAngle = Mathf.Cos(visionAngle * 0.5f * Mathf.Deg2Rad);
        return dotProduct >= cosHalfVisionAngle;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);

        var boidsInRange = BoidsInRange();
        foreach(var boid in boidsInRange)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, boid.transform.position);
        }    
    }
    private Vector2 Separation(List<BoidMovement> boidMovements)
    {
        Vector2 direction = Vector2.zero;
        foreach(var boid in boidMovements)
        {
            float ratio = Mathf.Clamp01((boid.transform.position - transform.position).magnitude / radius);
            direction -= ratio * (Vector2)(boid.transform.position - transform.position);
        }
        return direction.normalized;
    }
    private Vector2 Alignment(List<BoidMovement> boidMovements)
    {
        Vector2 direction = Vector2.zero;
        foreach (var boid in boidMovements)
        {
            direction += (Vector2)boid.velocity;
        }
        if (boidMovements.Count > 0) direction /= boidMovements.Count;
        else direction = velocity;
        return direction.normalized;
    }
    private Vector2 Cohesion(List<BoidMovement> boidMovements)
    {
        Vector2 direction;
        Vector2 center = Vector2.zero;
        foreach (var boid in boidMovements)
        {
            center += (Vector2)boid.transform.position;
        }
        if (boidMovements.Count > 0) center /= boidMovements.Count;
        else center = transform.position;
        direction = center - (Vector2)transform.position;
        return direction.normalized;
    }
}
