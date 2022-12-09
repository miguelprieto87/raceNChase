using TMPro;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class RaceUIManager : MonoBehaviourPunCallbacks
{
    public TMP_Text countdownUIText;
    public GameObject orderPanel;
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
        orderPanel.SetActive(true);
        return finishOrderUIObjects[which];
    }

    public void SetCountdownUIText(string text)
    {
        countdownUIText.text = text;
    }

    public void OnQuitClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        SceneManager.LoadScene("LobbyScene");
    }
}
