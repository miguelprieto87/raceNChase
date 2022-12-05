using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    public float accelForce = 30f;
    public float rotationTorque = 30f;

    private Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            body.AddRelativeForce(new(0f, 0f, accelForce));
        }
        else if (Input.GetKey(KeyCode.S))
        {
            body.AddRelativeForce(new(0f, 0f, -accelForce));
        }

        if (Input.GetKey(KeyCode.D))
        {
            body.AddRelativeTorque(new(0f, rotationTorque, 0f));
        }
        if (Input.GetKey(KeyCode.A))
        {
            body.AddRelativeTorque(new(0f, -rotationTorque, 0f));
        }
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
