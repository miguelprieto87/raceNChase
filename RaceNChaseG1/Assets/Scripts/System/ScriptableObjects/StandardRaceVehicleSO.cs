using UnityEngine;

[CreateAssetMenu(fileName = "SRVehicle_", menuName = "Standard Race Vehicle")]
public class StandardRaceVehicleSO : ScriptableObject
{
    [Header("Vehicle Properties")]
    public string vehicleName;
    public Sprite vehicleSprite;
}
