using UnityEngine;

[CreateAssetMenu(fileName = "DRVehicle_", menuName = "Death Race Vehicle")]
public class DeathRaceVehicleSO : ScriptableObject
{
    [Header("Vehicle Properties")]
    public string vehicleName;
    public Sprite vehicleSprite;

    [Header("Weapon Properties")]
    public string weaponName;
    public float weaponDamage;
    public float rateOfFire;
    public float projectileSpeed;
}
