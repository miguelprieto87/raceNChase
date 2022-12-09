using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviourPun
{
    public float topSpeed = 30f;
    public float accelForce = 30f;
    public float rotationTorque = 30f;

    float currentAccel = 0f;

    public float resistance = 1f;

    private Rigidbody body;
    private TMP_Text speedometer;

    public AudioSource Source;
    public AudioClip Vroom;

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
        currentAccel = Mathf.Clamp(currentAccel, -topSpeed, topSpeed);

        if (Input.GetKey(KeyCode.W))
        {
            currentAccel += accelForce;
            Source.PlayOneShot(Vroom, 0.7F);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            currentAccel -= accelForce;
        }
        else
        {
            if (currentAccel > 0f)
                currentAccel -= resistance;
            else if (currentAccel < 0f)
                currentAccel = 0f;
        }
        body.AddRelativeForce(new(0f, 0f, currentAccel));

        if (Input.GetKey(KeyCode.D))
        {
            body.AddRelativeTorque(new(0f, rotationTorque, 0f));
        }
        if (Input.GetKey(KeyCode.A))
        {
            body.AddRelativeTorque(new(0f, -rotationTorque, 0f));
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
