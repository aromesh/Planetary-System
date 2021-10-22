using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class CelestialBody : MonoBehaviour
{
    public static List<CelestialBody> CelestialBodies;
    public Rigidbody rb;
    const float universal_G = 1f;
    const float physicsTimeStep = 0.01f;

    public Vector3 velocity {get; private set;}

    void FixedUpdate()
    {
        foreach (CelestialBody body in CelestialBodies)
        {
            if (body != this)
            {
                Attract(body);
            }
        }
    }

    private void OnEnable()
    {
        if (CelestialBodies == null)
        {
            CelestialBodies = new List<CelestialBody>();
        }
        CelestialBodies.Add(this);
        //intial velocity
        rb.velocity = new Vector3(0,0,0);
    }

    private void OnDisable()
    {
        CelestialBodies.Remove(this);
    }
    void Attract(CelestialBody celestial_body)
    {
        Rigidbody other_body = celestial_body.rb;

        Vector3 direction = rb.position - other_body.position;
        float dist_sqr = direction.sqrMagnitude;

        if (dist_sqr == 0f)
        {
            return;
        }

        float force_magnitude = universal_G * (rb.mass * other_body.mass)/dist_sqr;
        Vector3 force = force_magnitude * direction.normalized;
        other_body.AddForce(force);

        Vector3 acceleration = force/rb.mass;
        velocity += acceleration * physicsTimeStep;
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        rb.MovePosition(rb.position + velocity*physicsTimeStep);
    }

}
