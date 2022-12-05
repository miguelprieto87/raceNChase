using System.Collections;

using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
public class HealthManager : MonoBehaviourPun
{
    [Header("Health")]
    public float startHealth = 100f;
    public Image healthBar;

    [Header("Respawn")]
    public GameObject weaponObject;
    public GameObject playerUIObject;
    public int respawnTimeInSeconds = 9;

    private float currentHealth;
    private GameObject deathUIPanel;
    private Text respawnTimerUIText;

    private void Start()
    {
        currentHealth = startHealth;
        healthBar.fillAmount = 1f;

        if (photonView.IsMine)
        {
            deathUIPanel = GameObject.Find("DeathPanel");
            respawnTimerUIText = deathUIPanel.transform.Find("RespawnTimeText").GetComponent<Text>();

            deathUIPanel.SetActive(false);
        }
    }

    [PunRPC]
    public void ApplyDamage(float damage)
    {
        if (currentHealth <= 0f) return;

        currentHealth -= damage;

        healthBar.fillAmount = currentHealth / startHealth;

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        ChangeLivingState(false);

        if (photonView.IsMine)
        {
            respawnTimerUIText.text = respawnTimeInSeconds.ToString();
            StartCoroutine(Respawn(respawnTimeInSeconds));
        }
    }

    private void ChangeLivingState(bool alive)
    {
        weaponObject.SetActive(alive);
        playerUIObject.SetActive(alive);

        if (photonView.IsMine)
        {
            deathUIPanel.SetActive(!alive);
        }
    }

    private IEnumerator Respawn(int timeToRespawn)
    {
        GetComponent<CarController>().enabled = false;
        GetComponent<Weapon>().enabled = false;

        while (timeToRespawn > 0)
        {
            yield return new WaitForSeconds(1f);
            timeToRespawn--;

            respawnTimerUIText.text = timeToRespawn.ToString();
        }

        GetComponent<CarController>().enabled = true;
        GetComponent<Weapon>().enabled = true;

        float randomPos = Random.Range(-35, 36);
        transform.position = new Vector3(randomPos, 0, randomPos);

        photonView.RPC("RestoreVehicle", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void RestoreVehicle()
    {
        currentHealth = startHealth;
        healthBar.fillAmount = 1f;

        ChangeLivingState(true);
    }
}
