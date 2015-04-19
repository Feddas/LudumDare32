using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}

/// <summary>
/// copied from: http://docs.unity3d.com/Manual/WheelColliderTutorial.html
/// </summary>
public class SimpleCarController : MonoBehaviour
{
    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;

    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    public void FixedUpdate()
    {
        float motor = maxMotorTorque * Recoil.CarRecoil;// Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            //if (axleInfo.motor)
            //{
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            //}
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Collectable")
        {
            //Debug.Log("Picked up " + collision.gameObject.name);
            var clip = collision.gameObject.GetComponent<AudioSource>().clip;
            var audio = this.GetComponent<AudioSource>();

            audio.Stop();
            audio.clip = clip;
            audio.Play();

            GameManager.Instance.CoinCount--;
            Destroy(collision.gameObject);
        }
        else if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            //Debug.Log("Hit wall " + collision.gameObject.name);
            this.transform.position = new Vector3(0, .8f, 0);
            this.transform.rotation = Quaternion.identity;
            GameManager.Instance.CoinCount += 20;
            GameManager.Instance.OverlayText("You hit a wall, you have to collect 20 more cubes.");
        }
        else
        {
            //Debug.Log("Hit other " + collision.gameObject.name);
        }
    }
}