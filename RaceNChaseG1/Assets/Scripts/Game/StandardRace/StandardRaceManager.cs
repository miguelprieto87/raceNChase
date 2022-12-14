using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StandardRaceManager : MonoBehaviour
{
    public GameObject lapContainer;
    public GameObject initialLine;
    public GameObject finalLine;
    public byte numberOfLaps;
    [HideInInspector]
    public List<Collider> lapTriggers;

    public static StandardRaceManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        lapTriggers = lapContainer.GetComponentsInChildren<Collider>().ToList();
        numberOfLaps = GameManager.Instance.settings.numberOfLaps;
    }
}
