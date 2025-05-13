using UnityEngine;
using System.Collections.Generic;

public class TrollerSpawnManager : MonoBehaviour
{
    List<NavManager> _trollers = new List<NavManager>();
    public List<NavManager> Trollers => _trollers;

    public static TrollerSpawnManager Instance;
    void Awake()
    { 
        if (Instance == null) Instance = this;

        foreach (NavManager nav in GetComponentsInChildren<NavManager>())
        { _trollers.Add(nav); }
        foreach (NavManager nav in GetComponentsInChildren<NavManager>())
        { nav.SetAgents(Trollers); }
    }
}
