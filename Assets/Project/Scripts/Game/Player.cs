using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Visuals")]
    public Camera playerCamera;
    public GameObject bulletPrefab;

    [Header("Gameplay")]
    public int initialHealth = 100;
    public int initialAmmo=12;
    public float knockbackForce = 10;
    public float hurtDuration = 0.5f;

    private bool isHurt;

    private int health;
    public int Health { get { return health; } }
    private int ammo;
    public int Ammo { get { return ammo; } }
    private bool killed;
    public bool Killed { get { return killed; } }



    // Start is called before the first frame update
    void Start(){
        ammo = initialAmmo;
        health = initialHealth;
    }

    // Update is called once per frame
    //shooting bullet
    void Update()
    {
        if (Input.GetMouseButtonDown(0))//#0 for left mouse 1 right 2 middle
        {

            if (ammo > 0 && Killed==false){
                ammo--;

                GameObject bulletObject = ObjectPoolingManager.Instance.GetBullet(true); //creating bulletprefab
                bulletObject.transform.position = playerCamera.transform.position + playerCamera.transform.forward;//bullet front of me
                bulletObject.transform.forward = playerCamera.transform.forward;//no need calculate, from camera moove forward
            }
        }
    }

    //check for collisions
    private void OnTriggerEnter(Collider OterCollider)
    {
        if (OterCollider.GetComponent<AmmoCrate>() != null){
            //collect ammo, colliod with the crates
            AmmoCrate ammoCrate = OterCollider.GetComponent<AmmoCrate>();
            ammo += ammoCrate.ammo;

            Destroy(ammoCrate.gameObject);
        }
        //healt crate
        else if (OterCollider.GetComponent<HealthCrate>() != null)
        {
            //collect ammo, colliod with the crates
            HealthCrate healthCrate = OterCollider.GetComponent<HealthCrate>();
            health += healthCrate.health;

            Destroy(healthCrate.gameObject);
        }


        if (isHurt == false) {
            GameObject hazard = null;
            if (OterCollider.GetComponent<Enemy>()!=null){
            //touching enemies.
                Enemy enemy = OterCollider.GetComponent<Enemy>();
                if (enemy.Killed == false)
                {
                    hazard = enemy.gameObject;
                    health -= enemy.damage;
                }
            }

            else if (OterCollider.GetComponent<Bullet>() != null)
            {
                Bullet bullet = OterCollider.GetComponent<Bullet>();
                if (bullet.ShotByPlayer == false)
                {
                    hazard = bullet.gameObject;
                    health -= bullet.damage;
                    bullet.gameObject.SetActive(false);
                }
            }
            if (hazard != null) {
                isHurt = true;
                //perform the knockback effect.
                Vector3 hurtDirection = (transform.position - hazard.transform.position).normalized;
                Vector3 knockbackDirection = (hurtDirection + Vector3.up).normalized;
                GetComponent<ForceReceiver>().AddForce(knockbackDirection, knockbackForce);

                StartCoroutine(HurtRoutine());
            }

            //player dies
            if (health <= 0)
            {
                if (killed == false)
                {
                    killed = true;

                    Onkill();
                }
            }
        }
    }

    IEnumerator HurtRoutine()
    {
        yield return new WaitForSeconds(hurtDuration);

        isHurt = false;
    }

    //disabling 
    private void Onkill()
    {
        GetComponent<CharacterController>().enabled = false;
        GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
    }
}
