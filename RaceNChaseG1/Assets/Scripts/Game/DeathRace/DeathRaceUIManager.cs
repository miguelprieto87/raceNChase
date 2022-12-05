using UnityEngine;

using Photon.Pun;
using UnityEngine.SceneManagement;

public class DeathRaceUIManager : MonoBehaviourPunCallbacks
{
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
