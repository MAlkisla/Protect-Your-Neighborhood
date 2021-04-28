using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AK : MonoBehaviour
{
    Animator animator;

    public bool canShoot;
    float inFrequencyOfShoot;
    public float outFrequencyOfShoot;
    public float range;

    [Header("Sounds")]
    public AudioSource fireSound;
    public AudioSource magazineChargerSound;
    public AudioSource bulletOutSound;

    [Header("Effects")]
    public ParticleSystem fireEffect;
    public ParticleSystem bulletTrace;
    public ParticleSystem bloodEffect;

    [Header("Others")]
    public Camera myCam;

    [Header("Gun Settings")]
    public int TotalBulletQuantity;
    public int MagazineCapacity;
    public int RemainingBullets;
    public TextMeshProUGUI TotalBullet_Text;
    public TextMeshProUGUI RemainingBullets_Text;


    void Start()
    {
        RemainingBullets = MagazineCapacity;
        MagazineCharging("Normal");
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (canShoot && Time.time > inFrequencyOfShoot && RemainingBullets != 0)
            {
                Shoot();
                inFrequencyOfShoot = Time.time + outFrequencyOfShoot;
            }
            if (RemainingBullets == 0)
                bulletOutSound.Play();

        }
        if (Input.GetKey(KeyCode.R))
        {
            if (RemainingBullets < MagazineCapacity && TotalBulletQuantity != 0)
            {
                if (RemainingBullets != 0)
                    MagazineCharging("BulletsAvailable");
                else
                    MagazineCharging("BulletsIsNotAvailable");

                animator.Play("magazinecharger");
            }
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

        RemainingBullets--;
        RemainingBullets_Text.text = RemainingBullets.ToString();

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

    private void MagazineCharging(string stage)
    {
        switch (stage)
        {
            case "BulletsAvailable":
                if (TotalBulletQuantity <= MagazineCapacity)
                {
                    int currentBullets = RemainingBullets;
                    RemainingBullets += TotalBulletQuantity;
                }
                else
                {
                    TotalBulletQuantity -= MagazineCapacity - RemainingBullets;
                    RemainingBullets = MagazineCapacity;
                }
                TotalBullet_Text.text = TotalBulletQuantity.ToString();
                RemainingBullets_Text.text = RemainingBullets.ToString();
                break;

            case "BulletsIsNotAvailable":
                if (TotalBulletQuantity <= MagazineCapacity)
                {
                    RemainingBullets = TotalBulletQuantity;
                    TotalBulletQuantity = 0;
                }
                else
                {
                    TotalBulletQuantity -= MagazineCapacity;
                    RemainingBullets = MagazineCapacity;
                }
                TotalBullet_Text.text = TotalBulletQuantity.ToString();
                RemainingBullets_Text.text = RemainingBullets.ToString();
                break;

            case "Normal":
                TotalBullet_Text.text = TotalBulletQuantity.ToString();
                RemainingBullets_Text.text = RemainingBullets.ToString();
                break;
        }
    }
}
