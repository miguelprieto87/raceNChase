using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;
using Photon.Realtime;

using ExitGames.Client.Photon;

public class UIPanelManager : MonoBehaviour
{
    public static UIPanelManager Instance { get; private set; }

    [Header("Panels")]
    public GameObject loginUIPanel;
    public GameObject connectingInfoPanel;

    public GameObject createRoomUIPanel;
    public GameObject creatingRoomInfoPanel;

    public GameObject gameOptionsUIPanel;

    public GameObject joinRandomUIPanel;
    public GameObject insideRoomUIPanel;

    [Header("Text Fields")]
    [SerializeField] private Text creatingGameUIMessage;

    [Header("Inside Room Panel")]
    [SerializeField] private Text gameTypeUIText;
    [SerializeField] private TMP_Text roomInfoUIText;
    [SerializeField] private GameObject playerListUIContainer;
    [SerializeField] private GameObject playerEntryPrefab;
    [SerializeField] private GameObject startGameButton;
    [SerializeField] private Image roomPanelBackground;
    [SerializeField] private Sprite standardBackground;
    [SerializeField] private Sprite deathRaceBackground;

    [Header("Vehicle Selection Panel")]
    [SerializeField] private GameObject[] vehicleSelectionUIObjects;
    [SerializeField] private StandardRaceVehicleSO[] standardVehicles;
    [SerializeField] private DeathRaceVehicleSO[] deathRaceVehicles;

    private Dictionary<int, GameObject> playerDictionary = new Dictionary<int, GameObject>();

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

    public void ActivatePanel(string panelName)
    {
        loginUIPanel.SetActive(loginUIPanel.name.Equals(panelName));
        connectingInfoPanel.SetActive(connectingInfoPanel.name.Equals(panelName));
        createRoomUIPanel.SetActive(createRoomUIPanel.name.Equals(panelName));
        creatingRoomInfoPanel.SetActive(creatingRoomInfoPanel.name.Equals(panelName));
        gameOptionsUIPanel.SetActive(gameOptionsUIPanel.name.Equals(panelName));
        joinRandomUIPanel.SetActive(joinRandomUIPanel.name.Equals(panelName));
        insideRoomUIPanel.SetActive(insideRoomUIPanel.name.Equals(panelName));
    }

    public void UpdateCreatingRoomUIMessage(string message)
    {
        creatingGameUIMessage.text = message;
    }

    public void UpdateRoomInfoUIText(string text)
    {
        roomInfoUIText.text = text;
    }

    public void UpdateGameTypeInsideRoom(string mode)
    {
        if (mode == GameMode.Standard.ToString())
        {
            roomPanelBackground.sprite = standardBackground;
            gameTypeUIText.text = "Let's Race!";

            for (int i = 0; i < vehicleSelectionUIObjects.Length; i++)
            {
                GameObject go = vehicleSelectionUIObjects[i];
                StandardRaceVehicleSO srv = standardVehicles[i];

                SetVehiclePanelProperties(go, srv.vehicleSprite, srv.vehicleName, string.Empty);
            }

        }
        else if (mode == GameMode.DeathRace.ToString())
        {
            roomPanelBackground.sprite = deathRaceBackground;
            gameTypeUIText.text = "Let's Chase!";

            for (int i = 0; i < vehicleSelectionUIObjects.Length; i++)
            {
                GameObject go = vehicleSelectionUIObjects[i];
                DeathRaceVehicleSO drv = deathRaceVehicles[i];

                string vehicleProperty = $"{drv.weaponName}\r\nDamage: {drv.weaponDamage} - Rate of Fire: {drv.rateOfFire}s";
                SetVehiclePanelProperties(go, drv.vehicleSprite, drv.vehicleName, vehicleProperty);
            }
        }
    }

    private static void SetVehiclePanelProperties(GameObject go, Sprite sprite, string name, string property)
    {
        go.GetComponent<Image>().sprite = sprite;
        go.transform.Find("PlayerName").GetComponent<Text>().text = name;
        go.transform.Find("PlayerProperty").GetComponent<Text>().text = property;
    }

    public void SetStartButtonActiveState(bool state)
    {
        startGameButton.SetActive(state);
    }

    public void AddPlayerToLobbyList(Player newPlayer)
    {
        GameObject entry = Instantiate(playerEntryPrefab);

        entry.transform.SetParent(playerListUIContainer.transform);
        entry.transform.localScale = Vector3.one;
        entry.GetComponent<PlayerEntryInitializer>().Initialize(newPlayer.ActorNumber, newPlayer.NickName);
    
        playerDictionary.Add(newPlayer.ActorNumber, entry);
    }

    public void RemovePlayerFromLobbyList(int actorNumber)
    {
        Destroy(playerDictionary[actorNumber]);
        playerDictionary.Remove(actorNumber);
    }

    public void ClearPlayerLobbyList()
    {
        foreach (GameObject gameObject in playerDictionary.Values)
        {
            Destroy(gameObject);
        }
        playerDictionary.Clear();
    }

    public void UpdatePlayerReadyStatus(Player targetPlayer, Hashtable changedProps)
    {
        if (playerDictionary.TryGetValue(targetPlayer.ActorNumber, out GameObject playerEntryObject))
        {
            if (changedProps.TryGetValue(RaceNChaseConstants.PLAYER_READY_KEY, out object isPlayerReady))
            {
                playerEntryObject.GetComponent<PlayerEntryInitializer>().SetPlayerReady((bool)isPlayerReady);
            }
        }
    }
}