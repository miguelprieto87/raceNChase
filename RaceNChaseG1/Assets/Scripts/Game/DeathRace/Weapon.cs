using Photon.Pun;
using System.Collections;

using UnityEngine;

public class Weapon : MonoBehaviourPun
{
    public GameObject projectilePrefab;
    public Transform projectileSpawn;
    public DeathRaceVehicleSO playerProperties;

    private bool firing = false;
    private bool canFire = true;

    private bool isLaser = false;

    private LineRenderer lineRenderer;

    public AudioSource Source;
    public AudioClip Pewpew;

    private void Start()
    {
        isLaser = playerProperties.weaponName == "Laser Gun";
        if (isLaser)
        {
            lineRenderer = GetComponentInChildren<LineRenderer>();
            lineRenderer.startWidth = 0.1f;
        }
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        if (Input.GetKey(KeyCode.Space))
        {
            photonView.RPC("NotifyFire", RpcTarget.AllBuffered);

            Source.PlayOneShot(Pewpew, 0.7F);
        }
        else
        {
            photonView.RPC("CeaseFire", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void NotifyFire()
    {
        if (!firing && canFire)
        {
            firing = true;
            canFire = false;
            StartCoroutine(Fire());
        }
    }

    [PunRPC]
    public void CeaseFire()
    {
        firing = false;
    }

    private IEnumerator Fire()
    {
        while (firing)
        {
            if (isLaser)
            {
                Ray laser = new Ray(projectileSpawn.position, transform.forward * 200f);
                if (Physics.Raycast(laser, out RaycastHit hit))
                {
                    lineRenderer.enabled = true;
                    lineRenderer.SetPosition(0, projectileSpawn.position);
                    lineRenderer.SetPosition(1, hit.point);

                    StartCoroutine(DisableLaser());

                    GameObject go = hit.collider.gameObject;
                    if (go.CompareTag("Player") && go.GetComponent<PhotonView>().IsMine)
                    {
                        go.GetComponent<PhotonView>().RPC("ApplyDamage", RpcTarget.AllBuffered, playerProperties.weaponDamage);
                    }
                }
            }
            else
            {
                GameObject go = Instantiate(projectilePrefab, projectileSpawn);
                go.GetComponent<Projectile>().Initialize(transform.forward, playerProperties.projectileSpeed, playerProperties.weaponDamage);
            }

            yield return new WaitForSeconds(playerProperties.rateOfFire);
        }

        canFire = true;
    }

    private IEnumerator DisableLaser()
    {
        yield return new WaitForSeconds(playerProperties.rateOfFire * 0.5f);
        lineRenderer.enabled = false;
    }
}
