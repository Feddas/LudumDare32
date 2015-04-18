using UnityEngine;
using System.Collections;

/// <summary>
/// based off of: http://forum.unity3d.com/threads/simple-weapon-recoil-script.70271/
/// </summary>
public class Recoil : MonoBehaviour
{
    public string FireButton = "Fire1";
    public GameObject ExplosionOrigin;
    public ParticleSystem GunBurst;
    public float force = 100000.0f;
    public float radius = 6.0f;
    public float up = 0.0f;
    public ForceMode forceMode;

    private float recoil = 0.0f;
    private float maxRecoil_x;
    private float maxRecoil_y;
    private float recoilSpeed = 2f;

    void Start()
    {
    }

    public void StartRecoil(float recoilParam, float maxRecoil_xParam, float recoilSpeedParam)
    {
        // in seconds
        recoil = recoilParam;
        maxRecoil_x = maxRecoil_xParam;
        recoilSpeed = recoilSpeedParam;
        maxRecoil_y = Random.Range(-maxRecoil_xParam, maxRecoil_xParam);
    }

    void recoiling()
    {
        if (recoil > float.Epsilon)
        {
            Quaternion maxRecoil = Quaternion.Euler(maxRecoil_x, maxRecoil_y, 0f);
            // Dampen towards the target rotation
            transform.localRotation = Quaternion.Slerp(transform.localRotation, maxRecoil, Time.deltaTime * recoilSpeed);
            recoil -= Time.deltaTime;

            //Debug.Log("Recoil:" + recoil + "rot" + transform.localRotation.eulerAngles);
        }
        else
        {
            recoil = 0f;
            // Dampen towards the target rotation
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, Time.deltaTime * recoilSpeed / 2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        recoiling();

        if (Input.GetButton("Fire1"))
        {
            // StartRecoil(0.1f, 10f, 10f);
            GunBurst.Stop();
            GunBurst.Play();
            foreach (var col in Physics.OverlapSphere(ExplosionOrigin.transform.position, radius))
            {
                var rigidbody = col.GetComponent<Rigidbody>();
                //Debug.Log("collided with " + col.name + " rigid?" + (rigidbody != null));
                if (rigidbody == null)
                    continue;

                rigidbody.AddExplosionForce(force, ExplosionOrigin.transform.position, radius, up, forceMode);
            }

            // car doesn't have a collider on it, so it's rigid body isn't found in Physics.OverlapSphere
            this.GetComponent<Rigidbody>().AddExplosionForce(force, ExplosionOrigin.transform.position, radius, up, forceMode);
        }
    }
}