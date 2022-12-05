using System;
using UnityEngine;

using ExitGames.Client.Photon;
using Photon.Pun;

public class PlayerSelection : MonoBehaviour
{
    public GameObject[] selectablePlayers;

    private int activePlayer = 0;
    public int ActivePlayer
    {
        get { return activePlayer; }
        set
        {
            activePlayer = Math.Clamp(value, 0, selectablePlayers.Length - 1);
        }
    }

    private void Start()
    {
        ActivatePlayer(activePlayer);
    }

    private void ActivatePlayer(int thisPlayer)
    {
        foreach(GameObject player in selectablePlayers)
        {
            player.SetActive(false);
        }

        selectablePlayers[thisPlayer].SetActive(true);

        Hashtable playerSelectionProp = new() { { RaceNChaseConstants.PLAYER_SELECTION, thisPlayer } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerSelectionProp);
    }

    public void ChangeActivePlayer(int direction)
    {
        activePlayer += direction;
        if (activePlayer >= selectablePlayers.Length)
        {
            activePlayer = 0;
        }
        else if (activePlayer == -1)
        {
            activePlayer = selectablePlayers.Length - 1;
        }
        ActivatePlayer(activePlayer);
    }
}
