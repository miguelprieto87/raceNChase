using UnityEngine;

using TMPro;

using Photon.Pun;
using Photon.Realtime;

using ExitGames.Client.Photon;

public class LobbyEvents : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    public TMP_InputField roomNameInput;

    private GameManager gameManagerRef;
    private UIPanelManager uiManagerRef;

    private void Start()
    {
        gameManagerRef = GameManager.Instance;
        uiManagerRef = UIPanelManager.Instance;
    }

    public void OnLoginButtonClicked()
    {
        string playerName = playerNameInput.text;

        if (!string.IsNullOrEmpty(playerName))
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PhotonNetwork.ConnectUsingSettings();

            string connectingInfoName = uiManagerRef.connectingInfoPanel.name;
            uiManagerRef.ActivatePanel(connectingInfoName);
        }
        else
        {
            Debug.LogError("<color=red>Empty player name.</color>");
        }
    }

    public void OnCreateGameClicked()
    {
        string createRoomName = uiManagerRef.createRoomUIPanel.name;
        uiManagerRef.ActivatePanel(createRoomName);
    }

    public void OnCreateRoomClicked()
    {
        string roomName = roomNameInput.text;
        
        if (string.IsNullOrEmpty(roomName))
        {
            roomName = "Room" + Random.Range(1, 10000);
        }

        string[] roomPropsInLobby = { RaceNChaseConstants.GAME_MODE_KEY };
        Hashtable customProperties = new Hashtable()
        {
            { RaceNChaseConstants.GAME_MODE_KEY, gameManagerRef.settings.gameMode.ToString() }
        };

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = gameManagerRef.settings.maxPlayers;
        roomOptions.CustomRoomPropertiesForLobby = roomPropsInLobby;
        roomOptions.CustomRoomProperties = customProperties;

        PhotonNetwork.CreateRoom(roomName, roomOptions);

        string creatingRoomName = uiManagerRef.creatingRoomInfoPanel.name;
        uiManagerRef.ActivatePanel(creatingRoomName);
    }

    public void OnJoinRandomRoomClicked(GameModeSO gameMode)
    {
        gameManagerRef.settings.gameMode = gameMode.mode;

        Hashtable expectedRoomProperties = new Hashtable { { RaceNChaseConstants.GAME_MODE_KEY, gameManagerRef.settings.gameMode.ToString() } };

        PhotonNetwork.JoinRandomRoom(expectedRoomProperties, 0);
    }

    public void OnLeaveRoomClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnStartGameClicked()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(RaceNChaseConstants.GAME_MODE_KEY))
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue(GameMode.Standard.ToString()))
            {
                PhotonNetwork.LoadLevel("RacingScene");
            }
            else if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue(GameMode.DeathRace.ToString()))
            {
                PhotonNetwork.LoadLevel("DeathRaceScene");
            }
        }
    }

    public void SetGameMode(GameModeSO gameMode)
    {
        gameManagerRef.settings.gameMode = gameMode.mode;
    }
}
