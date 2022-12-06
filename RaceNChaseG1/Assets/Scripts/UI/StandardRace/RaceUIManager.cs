using TMPro;
using UnityEngine;

public class RaceUIManager : MonoBehaviour
{
    public TMP_Text countdownUIText;

    public GameObject[] finishOrderUIObjects;

    public static RaceUIManager Instance { get; private set; }

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
        foreach(GameObject go in finishOrderUIObjects) 
        { 
            go.SetActive(false);
        }
    }

    public GameObject GetFinishOrderUIObject(int which)
    {
        return finishOrderUIObjects[which];
    }

    public void SetCountdownUIText(string text)
    {
        countdownUIText.text = text;
    }
}