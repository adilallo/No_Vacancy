using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class agent : MonoBehaviour
{
    Color myColor;
    Vector3 velocity;
    Vector3 acceleration;
    int maxSpeed = 3;
    List<agent> allAgents;

    public float cohesion_radius=0.5f;
    public float cohesion_strength = 0.5f;
    public float separation_radius = 0.5f;
    public float separation_strength = 0.5f;
    public float allignment_radius = 0.5f;
    public float allignment_strength = 0.5f;
    public float agility = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        myColor = new Color(Random.value, Random.value, Random.value);
        //this.GetComponent<MeshRenderer>().material.color = myColor;
        //this.GetComponent<TrailRenderer>().material.color = myColor;

        allAgents = FindObjectOfType<manage_swarm>().allAgents;

        velocity = Vector3.zero;// Random.insideUnitSphere;
        this.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        acceleration = (cohesion(10*cohesion_radius, cohesion_strength)
            + separation(10*separation_radius, separation_strength)
            + allignment(10*allignment_radius,allignment_strength))/3 ;//compose the acceleration vector
        
        velocity += acceleration*agility;//adjust how much inertia the agents will have
        if (velocity.magnitude > maxSpeed)//limit the velocity to maxSpeed
        {
            velocity = velocity.normalized * maxSpeed;
        }
        this.transform.position += velocity*Time.deltaTime;
    }


    Vector3 cohesion(float radius, float amount)
    {
        Vector3 average = Vector3.zero;
        int within_radius = 1;
        foreach (agent a in allAgents)
        {
            if (a != this)
            {
                Vector3 diff = a.transform.position - this.transform.position;
                if (diff.magnitude > radius)
                {
                    average += diff;
                    within_radius++;
                }
            }
        }
        average /= within_radius;
        average.Normalize();
        average *= amount;
        return average;
    }

    Vector3 separation(float radius, float amount)
    {
        Vector3 average = Vector3.zero;
        int within_radius = 1;
        foreach (agent a in allAgents)
        {
            if (a != this)
            {
                Vector3 diff = a.transform.position - this.transform.position;
                if (diff.magnitude < radius)
                {
                    average += diff;
                    within_radius++;
                }
            }
        }
        average /= within_radius;
        average.Normalize();
        average *= amount*-1;
        return average;
    }

    Vector3 allignment(float radius, float amount)
    {
        Vector3 average = Vector3.zero;
        int within_radius = 1;
        foreach (agent a in allAgents)
        {
            if (a != this)
            {
                Vector3 diff = a.transform.position - this.transform.position;
                if (diff.magnitude < radius)
                {
                    average += a.velocity;//.normalized;
                    within_radius++;
                }
            }
        }
        average /= within_radius;
        average.Normalize();
        average *= amount;
        return average;
    }
}
