using System.Collections;
using UnityEngine;

using Photon.Pun;

[RequireComponent(typeof(CarController)), RequireComponent(typeof(PlayerInitializer))]
public class CountdownTimer : MonoBehaviourPunCallbacks
{
    private byte countdownTimer;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            countdownTimer = GameManager.Instance.settings.timeToRaceStart;
            photonView.RPC("SetTime", RpcTarget.AllBuffered, countdownTimer);
            StartCoroutine(Tick());
        }
    }

    [PunRPC]
    public void SetTime(byte time)
    {
        if (time > 0)
        {
            RaceUIManager.Instance.SetCountdownUIText(time.ToString());
        }
        else
        {
            RaceUIManager.Instance.SetCountdownUIText("GO!");
            GetComponent<CarController>().EnableControls();
            StartCoroutine(ClearCountdownText());
        }
    }

    private IEnumerator Tick()
    {
        while(countdownTimer > 0)
        {
            countdownTimer--;
            yield return new WaitForSeconds(1);
            photonView.RPC("SetTime", RpcTarget.AllBuffered, countdownTimer);
        }
    }

    private IEnumerator ClearCountdownText()
    {
        yield return new WaitForSeconds(3);
        RaceUIManager.Instance.SetCountdownUIText(string.Empty);
        enabled = false;
    }
}
