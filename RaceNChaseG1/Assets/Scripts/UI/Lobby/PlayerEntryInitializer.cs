using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

using ExitGames.Client.Photon;

public class PlayerEntryInitializer : MonoBehaviour
{
    [Header("UI References")]
    public Text playerNameText;
    public Button playerReadyButton;
    public Image playerReadyCheckmark;

    private bool isReady = false;

    public void Initialize(int playerID, string nickname)
    {
        playerNameText.text = nickname;

        if (PhotonNetwork.LocalPlayer.ActorNumber != playerID)
        {
            playerReadyButton.gameObject.SetActive(false);
        }
        else
        {
            Hashtable initialProperties = new Hashtable() { { RaceNChaseConstants.PLAYER_READY_KEY, isReady} };
            PhotonNetwork.LocalPlayer.SetCustomProperties(initialProperties);

            playerReadyButton.onClick.AddListener(() =>
            {
                isReady = !isReady;
                SetPlayerReady(isReady);

                initialProperties[RaceNChaseConstants.PLAYER_READY_KEY] = isReady;
                PhotonNetwork.LocalPlayer.SetCustomProperties(initialProperties);
            });
        }
    }

    public void SetPlayerReady(bool ready)
    {
        playerReadyCheckmark.enabled = ready;

        if (ready) playerReadyButton.GetComponentInChildren<Text>().text = "Ready!";
        else playerReadyButton.GetComponentInChildren<Text>().text = "Ready?";
    }
}
