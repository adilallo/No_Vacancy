using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manage_swarm : MonoBehaviour
{
    public GameObject agent_pre;
    public int spread;
    public int amount;
    public GameObject cam_rig;
    public List<agent> allAgents;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < amount; i++)
        {
          allAgents.Add(
              Instantiate(agent_pre,
              Random.insideUnitSphere * spread,
              Quaternion.identity).
              GetComponent<agent>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 average = Vector3.zero;
        foreach(agent a in allAgents)
        {
            average += a.transform.position;            
        }
        average /= allAgents.Count+1;
        cam_rig.transform.position =Vector3.MoveTowards(cam_rig.transform.position, average,0.1f);
    }

    //functions attached to the sliders:
    public void updateCohesionRadius(float val)
    {
        foreach (agent a in allAgents)
        {
            a.cohesion_radius = val;
        }
    }
    public void updateCohesionStrength(float val)
    {
        foreach (agent a in allAgents)
        {
            a.cohesion_strength = val;
        }
    }
    public void updateSeparationRadius(float val)
    {
        foreach (agent a in allAgents)
        {
            a.separation_radius = val;
        }
    }
    public void updateSeparationStrength(float val)
    {
        foreach (agent a in allAgents)
        {
            a.separation_strength = val;
        }
    }
    public void updateAllignmentRadius(float val)
    {
        foreach (agent a in allAgents)
        {
            a.allignment_radius = val;
        }
    }
    public void updateAllignmentStrength(float val)
    {
        foreach (agent a in allAgents)
        {
            a.allignment_strength = val;
        }
    }
}
