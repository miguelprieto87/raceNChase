using UnityEngine;

using Photon.Pun;

using TMPro;

[RequireComponent(typeof(CarController))]
public class PlayerInitializer : MonoBehaviourPunCallbacks
{
    public Camera playerCamera;
    [SerializeField] private TMP_Text playerName;
    
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue(GameMode.Standard.ToString()))
        {
            if (photonView.IsMine)
            {
                GetComponent<LapController>().enabled = true;
            }
            else
            {
                GetComponent<LapController>().enabled = false;
            }
        }

        if (photonView.IsMine)
        {
            GetComponent<CarController>().enabled = true;
            playerCamera.gameObject.SetActive(true);
        }
        else
        {
            GetComponent<CarController>().enabled = false;
            playerCamera.gameObject.SetActive(false);
        }

        playerName.text = photonView.Owner.NickName;
        if (photonView.IsMine) 
        {
            playerName.gameObject.SetActive(false);
        }
    }
}