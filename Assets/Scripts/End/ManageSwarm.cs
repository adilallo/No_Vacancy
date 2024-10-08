using System.Collections.Generic;
using UnityEngine;

public class ManageSwarm : MonoBehaviour
{
    public List<GameObject> agent_prefabs;
    public int spread;
    public int amount;
    public GameObject cam_rig;
    public List<Agent> allAgents;
    public float rotationSpeed = 30f;

    void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject selectedPrefab = agent_prefabs[i % agent_prefabs.Count];
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

        UpdateControlsWithMouse();

        RotateAgents();
    }

    void UpdateControlsWithMouse()
    {
        float normalizedMouseX = Mathf.Clamp01(Input.mousePosition.x / Screen.width);
        float normalizedMouseY = Mathf.Clamp01(Input.mousePosition.y / Screen.height);

        updateCohesionRadius(normalizedMouseX);
        updateCohesionStrength(normalizedMouseY);
        updateSeparationRadius(normalizedMouseX);
        updateSeparationStrength(normalizedMouseY);
        updateAllignmentRadius(normalizedMouseX);
        updateAllignmentStrength(normalizedMouseY);
    }

    void RotateAgents()
    {
        foreach (Agent agent in allAgents)
        {
            agent.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
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