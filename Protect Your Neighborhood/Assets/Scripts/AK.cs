using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AK : MonoBehaviour
{
    public bool canShoot;
    float inFrequencyOfShoot;
    public float outFrequencyOfShoot;
    public float range;
    public Camera myCam;
    public AudioSource fireSound;
    public ParticleSystem fireEffect;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && canShoot && Time.time > inFrequencyOfShoot)
        {
            Shoot();
            inFrequencyOfShoot = Time.time + outFrequencyOfShoot;
        }
    }

    private void Shoot()
    {
        fireSound.Play();
        fireEffect.Play();

        RaycastHit hit;

        if (Physics.Raycast(myCam.transform.position,myCam.transform.forward,out hit, range))
        {
            Debug.Log(hit.transform.name);
        }
    }
}
