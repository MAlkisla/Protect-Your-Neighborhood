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
    public AudioSource magazineChargerSound;
    public ParticleSystem fireEffect;
    public ParticleSystem bulletTrace;
    public ParticleSystem bloodEffect;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && canShoot && Time.time > inFrequencyOfShoot)
        {
            Shoot();
            inFrequencyOfShoot = Time.time + outFrequencyOfShoot;
        }
        if (Input.GetKey(KeyCode.R))
        {
            animator.Play("magazinecharger");
        }
    }

    void MagazineCharger()
    {
        magazineChargerSound.Play();
    }

    private void Shoot()
    {
        fireSound.Play();
        fireEffect.Play();
        animator.Play("fire");

        RaycastHit hit;

        if (Physics.Raycast(myCam.transform.position, myCam.transform.forward, out hit, range))
        {
            if (hit.transform.gameObject.CompareTag("Enemy"))
                Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
            else if (hit.transform.gameObject.CompareTag("TipOverObject"))
            {

                Rigidbody rg = hit.transform.gameObject.GetComponent<Rigidbody>();
                //rg.AddForce(transform.forward * 500f);
                rg.AddForce(-hit.normal * 500f);
                Instantiate(bulletTrace, hit.point, Quaternion.LookRotation(hit.normal));
            }

            else
                Instantiate(bulletTrace, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }
}
