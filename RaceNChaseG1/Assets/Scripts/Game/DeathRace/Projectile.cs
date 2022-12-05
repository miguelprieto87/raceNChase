using Photon.Pun;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    private float damage;

    private void Start()
    {
        StartCoroutine(Kill());
    }

    private IEnumerator Kill()
    {
        while (true) 
        { 
            if (transform.position.sqrMagnitude > 2500)
            {
                Destroy(gameObject);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public void Initialize(Vector3 dir, float speed, float damage)
    {
        transform.forward = dir;

        this.damage = damage;

        GetComponent<Rigidbody>().velocity = dir * speed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);

        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PhotonView>().IsMine)
        {
            other.gameObject.GetComponent<PhotonView>().RPC("ApplyDamage", RpcTarget.AllBuffered, damage);
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
