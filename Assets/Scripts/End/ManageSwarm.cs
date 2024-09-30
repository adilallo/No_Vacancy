using System.Collections.Generic;
using UnityEngine;

public class ManageSwarm : MonoBehaviour
{
    public List<GameObject> agent_prefabs;
    public int spread;
    public int amount;
    public GameObject cam_rig;
    public List<Agent> allAgents;

    void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject selectedPrefab = agent_prefabs[Random.Range(0, agent_prefabs.Count)];
            GameObject newAgent = Instantiate(selectedPrefab, Random.insideUnitSphere * spread, Quaternion.identity);

            Agent agentScript = newAgent.GetComponent<Agent>();
            allAgents.Add(agentScript);

            agentScript.SetAllAgents(allAgents);
        }

        Debug.Log("Total Agents: " + allAgents.Count);
    }

    void Update()
    {
        Vector3 average = Vector3.zero;
        foreach (Agent a in allAgents)
        {
            average += a.transform.position;
        }
        average /= allAgents.Count + 1;

        cam_rig.transform.position = Vector3.MoveTowards(cam_rig.transform.position, average, 0.1f);

        RandomizeControls();
    }

    void RandomizeControls()
    {
        // Generate random values between 0 and 1 for each property
        float randomCohesionRadius = Random.Range(0f, 1f);
        float randomCohesionStrength = Random.Range(0f, 1f);
        float randomSeparationRadius = Random.Range(0f, 1f);
        float randomSeparationStrength = Random.Range(0f, 1f);
        float randomAllignmentRadius = Random.Range(0f, 1f);
        float randomAllignmentStrength = Random.Range(0f, 1f);

        // Apply the random values to all agents
        updateCohesionRadius(randomCohesionRadius);
        updateCohesionStrength(randomCohesionStrength);
        updateSeparationRadius(randomSeparationRadius);
        updateSeparationStrength(randomSeparationStrength);
        updateAllignmentRadius(randomAllignmentRadius);
        updateAllignmentStrength(randomAllignmentStrength);
    }

    public void updateCohesionRadius(float val)
    {
        foreach (Agent a in allAgents)
        {
            a.cohesion_radius = val;
        }
    }
    public void updateCohesionStrength(float val)
    {
        foreach (Agent a in allAgents)
        {
            a.cohesion_strength = val;
        }
    }
    public void updateSeparationRadius(float val)
    {
        foreach (Agent a in allAgents)
        {
            a.separation_radius = val;
        }
    }
    public void updateSeparationStrength(float val)
    {
        foreach (Agent a in allAgents)
        {
            a.separation_strength = val;
        }
    }
    public void updateAllignmentRadius(float val)
    {
        foreach (Agent a in allAgents)
        {
            a.allignment_radius = val;
        }
    }
    public void updateAllignmentStrength(float val)
    {
        foreach (Agent a in allAgents)
        {
            a.allignment_strength = val;
        }
    }
}
