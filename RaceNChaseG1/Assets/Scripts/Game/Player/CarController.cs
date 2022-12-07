using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviourPun
{
    public float baseSpeed = 1f;
    public float accelForce = 30f;
    public float rotationTorque = 30f;

    public float resistance = 1f;

    private Rigidbody body;
    private TMP_Text speedometer;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();

        if (photonView.IsMine)
        {
            speedometer = GameObject.Find("TXT_Speed").GetComponent<TMP_Text>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        speedometer.text = currentSpeed();

        float speedReduction = Random.Range(0, resistance);
        float carVelocity = ((baseSpeed * (accelForce - speedReduction)) + 1);

        if (Input.GetKey(KeyCode.W))
        {
            body.AddRelativeForce(new(0f, 0f, carVelocity));
        }
        else if (Input.GetKey(KeyCode.S))
        {
            body.AddRelativeForce(new(0f, 0f, -carVelocity / 2));
        }

        if (Input.GetKey(KeyCode.D))
        {
            body.AddRelativeTorque(new(0f, rotationTorque, baseSpeed));
        }
        if (Input.GetKey(KeyCode.A))
        {
            body.AddRelativeTorque(new(0f, -rotationTorque, baseSpeed));
        }
    }
    string currentSpeed()
    {
        float currentSpeed = body.velocity.magnitude;
        return currentSpeed.ToString("00") + "km/h";
    }
    public void EnableControls()
    {
        body.isKinematic = false;
    }

    public void DisableControls()
    {
        body.isKinematic = true;
    }
}
