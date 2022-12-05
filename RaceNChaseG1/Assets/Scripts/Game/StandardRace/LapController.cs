using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

using ExitGames.Client.Photon;
using TMPro;

[RequireComponent(typeof(CarController)), RequireComponent(typeof(PlayerInitializer))]
public class LapController : MonoBehaviourPun
{
    public enum EventCode
    {
        WhoFinished = 0
    }

    private List<Collider> lapTriggers = new List<Collider>();

    private byte finishOrder = 0;

    // Start is called before the first frame update
    void Start()
    {
        lapTriggers = StandardRaceManager.Instance.lapTriggers;
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEventReceived;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEventReceived;
    }

    private void OnEventReceived(EventData eventData)
    {
        if (eventData.Code == (byte)EventCode.WhoFinished)
        {
            object[] data = eventData.CustomData as object[];

            if (data != null)
            {
                string nickName = (string)data[0];
                finishOrder = (byte)data[1];
                int viewID = (int)data[2];

                GameObject finishOrderUIObject = RaceUIManager.Instance.GetFinishOrderUIObject(finishOrder - 1);
                finishOrderUIObject.SetActive(true);

                if (viewID == photonView.ViewID)
                {
                    finishOrderUIObject.GetComponent<TMP_Text>().color = Color.red;
                }
                finishOrderUIObject.GetComponent<TMP_Text>().text = $"{finishOrder}. {nickName}";

                Debug.Log($"{nickName} finished in position {finishOrder}.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (lapTriggers.Contains(other))
        {
            int triggerIndex = lapTriggers.IndexOf(other);
            lapTriggers[triggerIndex].gameObject.SetActive(false);

            if (other.name == "FinishTrigger")
            {
                GameFinished();
            }
        }
    }

    private void GameFinished()
    {
        finishOrder += 1;

        GetComponent<PlayerInitializer>().playerCamera.transform.parent = null;
        GetComponent<CarController>().enabled = false;

        int viewID = photonView.ViewID;

        object[] data = new object[] { photonView.Owner.NickName, finishOrder, viewID };

        RaiseEventOptions eventOptions = new RaiseEventOptions
        { 
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCache
        };

        SendOptions sendOptions = new SendOptions
        {
            Reliability = false
        };

        PhotonNetwork.RaiseEvent((byte)EventCode.WhoFinished, data, eventOptions, sendOptions);
    }
}
