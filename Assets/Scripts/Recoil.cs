using UnityEngine;
using System.Collections;

/// <summary>
/// based off of: http://forum.unity3d.com/threads/simple-weapon-recoil-script.70271/
/// </summary>
public class Recoil : MonoBehaviour
{
    public static float CarRecoil;
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
    private AudioSource gunAudio;

    void Start()
    {
        gunAudio = ExplosionOrigin.GetComponent<AudioSource>();
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

        if (CarRecoil > float.Epsilon)
        {
            CarRecoil -= Time.deltaTime;
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

            if (gunAudio.isPlaying == false)
                gunAudio.Play();

            foreach (var col in Physics.OverlapSphere(ExplosionOrigin.transform.position, radius))
            {
                var rigidbody = col.GetComponent<Rigidbody>();
                //Debug.Log("collided with " + col.name + " rigid?" + (rigidbody != null));
                if (rigidbody == null)
                    continue;

                rigidbody.AddExplosionForce(force, ExplosionOrigin.transform.position, radius, up, forceMode);
            }

            // AddExplosionForce and AddForce make the car frustrating to drive. If you want to try it, uncomment this line.
            // this.GetComponent<Rigidbody>().AddForce(Vector3.forward * force * 100);

            CarRecoil = 1; // HACK: SimpleCarController uses this global variable to add wheel torque
        }
    }
}