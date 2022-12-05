using UnityEngine;

using Photon.Pun;

public class VehicleSpawner : MonoBehaviour
{
    public GameObject[] vehiclePrefabs;
    public Transform[] startPositions;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(RaceNChaseConstants.PLAYER_SELECTION, out object playerSelection))
            {
                if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue(GameMode.Standard.ToString()))
                {
                    int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
                    PhotonNetwork.Instantiate(vehiclePrefabs[(int)playerSelection].name, startPositions[actorNumber - 1].position, Quaternion.identity);
                }
                else if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue(GameMode.DeathRace.ToString()))
                {
                    int randomPosition = Random.Range(-35, 35);
                    PhotonNetwork.Instantiate(vehiclePrefabs[(int)playerSelection].name, new Vector3(randomPosition, 0f, randomPosition), Quaternion.identity);
                }
            }
        }
    }
}
