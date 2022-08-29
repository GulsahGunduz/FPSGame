using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{

    public bool isShoot;
    float _shootRange;
    public float shootRange;
    public float distance;
    public Camera fpsCam;


    int bulletsTotal = 100;
    public int bulletMag;
    int remainBullet;


    public AudioSource[] sounds;
    public ParticleSystem[] effects;

    Animator animator;

    int mag = 2;

    float damage = 25;

    [Header("TEXT")]
    public TextMeshProUGUI BulletsText;
    public TextMeshProUGUI ReBulletText;


    private void Start()
    {
        remainBullet = mag;
        bulletsTotal = PlayerPrefs.GetInt("gun", bulletsTotal);

        BulletsText.text = bulletsTotal.ToString();
        ReBulletText.text = remainBullet.ToString();

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
      

        if(Input.GetKeyDown(KeyCode.Mouse0) && isShoot && Time.time > _shootRange && remainBullet != 0)
        {
            Shoot();
            _shootRange = Time.time + shootRange;
        }
        if(remainBullet == 0)
        {
            sounds[1].Play();
            animator.Play("ChangeMag");

            remainBullet = bulletsTotal;
        }
        

    }

    void Shoot()
    {

        remainBullet--;
        ReBulletText.text = remainBullet.ToString();

        animator.Play("Shoot");
        sounds[0].Play();
        effects[0].Play();

        RaycastHit hit;


        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, distance))
        {
            

            if (hit.transform.gameObject.CompareTag("Swat"))
            {
                Instantiate(effects[2], hit.point, Quaternion.LookRotation(hit.normal));
            }

            else if (hit.transform.gameObject.CompareTag("EnviromentObj"))
            {
                Instantiate(effects[1], hit.point, Quaternion.LookRotation(hit.normal));
            }

        }


    }



}
