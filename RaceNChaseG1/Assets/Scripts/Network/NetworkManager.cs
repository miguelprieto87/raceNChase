using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager Instance { get; private set; }

    [SerializeField]
    private LobbyEvents lobbyEvents;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnConnected()
    {
        base.OnConnected();

        string gameOptionsName = UIPanelManager.Instance.gameOptionsUIPanel.name;
        UIPanelManager.Instance.ActivatePanel(gameOptionsName);

        Debug.Log("<color=green>Successfully connected to the Photon Cloud</color>");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        Debug.LogFormat($"<color=cyan>{PhotonNetwork.LocalPlayer.NickName} connected to the Master Server.</color>");
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        Debug.LogFormat($"<color=orange>{PhotonNetwork.CurrentRoom.Name} was created.</color>");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        if (UIPanelManager.Instance == null) return;

        Debug.LogFormat($"<color=orange>{PhotonNetwork.LocalPlayer.NickName} joined the room.</color>");

        string insideRoomPanelName = UIPanelManager.Instance.insideRoomUIPanel.name;
        UIPanelManager.Instance.ActivatePanel(insideRoomPanelName);

        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(RaceNChaseConstants.GAME_MODE_KEY, out object gameMode))
        {
            UpdateRoomInfoText();
            UIPanelManager.Instance.UpdateGameTypeInsideRoom((string)gameMode);

            foreach (Player player in PhotonNetwork.PlayerList)
            {
                UIPanelManager.Instance.AddPlayerToLobbyList(player);
            }
        }

        UIPanelManager.Instance.SetStartButtonActiveState(false);
    }

    private void UpdateRoomInfoText()
    {
        string roomMessage = $"Room Name: {PhotonNetwork.CurrentRoom.Name}  -  Players: " +
            $"{PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
        UIPanelManager.Instance.UpdateRoomInfoUIText(roomMessage);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);

        Debug.LogWarningFormat($"JoinRandomRoom failed! Error code {returnCode}: {message}");

        UIPanelManager.Instance.UpdateCreatingRoomUIMessage("No match found!\r\nCreating new room...");

        lobbyEvents.OnCreateRoomClicked();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        if (UIPanelManager.Instance == null) return;

        UIPanelManager.Instance.AddPlayerToLobbyList(newPlayer);

        UpdateRoomInfoText();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        if (UIPanelManager.Instance == null) return;

        UIPanelManager.Instance.RemovePlayerFromLobbyList(otherPlayer.ActorNumber);

        UpdateRoomInfoText();

        UIPanelManager.Instance.SetStartButtonActiveState(CheckPlayerReady());
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        if (UIPanelManager.Instance == null) return;

        UIPanelManager.Instance.ClearPlayerLobbyList();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);

        if (UIPanelManager.Instance == null) return;

        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            UIPanelManager.Instance.SetStartButtonActiveState(CheckPlayerReady());
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        UIPanelManager.Instance.UpdatePlayerReadyStatus(targetPlayer, changedProps);

        UIPanelManager.Instance.SetStartButtonActiveState(CheckPlayerReady());
    }

    private bool CheckPlayerReady()
    {
        if (!PhotonNetwork.IsMasterClient) return false;

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.TryGetValue(RaceNChaseConstants.PLAYER_READY_KEY, out object isReady))
            {
                if (!(bool)isReady) return false;
            }
            else return false;
        }

        return true;
    }
}
