using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Weapon : MonoBehaviour
{
    private bool m_Shooting = false;
    float m_Timer = 0;
    public GameObject m_PlayerObj;
    public float gunForce = 5;                                           // Set the number of hitpoints that this gun will take away from shot objects with a health script
    public float fireRate = 0.25f;                                      // Number in seconds which controls how often the player can fire
    public float weaponRange = 50f;                                     // Distance in Unity units over which the player can fire
    public float hitForce = 100f;                                       // Amount of force which will be added to objects with a rigidbody shot by the player
    public Transform gunEnd;                                            // Holds a reference to the gun end object, marking the muzzle location of the gun

    public Camera fpsCam;                                              // Holds a reference to the first person camera
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);    // WaitForSeconds object used by our ShotEffect coroutine, determines time laser line will remain visible
                                                                        // Reference to the audio source which will play our shooting sound effect

    private float nextFire;                                             // Float to store the time the player will be allowed to fire again, after firing


    private FirstPersonController _player;
    private FirstPersonController m_Player
    {
        get
        {
            if (_player == null)
                _player = m_PlayerObj.GetComponent<FirstPersonController>();
            return _player;
        }
    }

    private Animator _animator;
    private Animator m_Animator
    {
        get
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();
            return _animator;
        }
    }

    private void DamageEnemy(RaycastHit xHit, float xIncForce)
    {
        if (xHit.collider.tag == "Enemy")
        {
            Zombie m_Zombie = xHit.collider.gameObject.GetComponent<Zombie>();
            m_Zombie.Damage(-xHit.normal * (hitForce * (1 + xIncForce)));
        }
    }

    private void Fire(float xIncForce)
    {
        // Update the time when our player can fire next
        nextFire = Time.time + fireRate;

        // Start our ShotEffect coroutine to turn our laser line on and off
        StartCoroutine(ShotEffect());

        // Create a vector at the center of our camera's viewport
        Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

        if (xIncForce > 0)
        {
            RaycastHit[] sHitAll;

            float thickness = 1f; //<-- Desired thickness here.
            Vector3 origin = rayOrigin;
            Vector3 direction = fpsCam.transform.forward;
            sHitAll = Physics.SphereCastAll(origin, thickness, direction);
            foreach (RaycastHit sHit in sHitAll)
            {
                DamageEnemy(sHit, xIncForce);
            }
        }else
        {
            RaycastHit sHit;

            // Check if our raycast has hit anything
            if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out sHit, weaponRange))
            {
                DamageEnemy(sHit, xIncForce);
            }
        }
    }

    void Update()
    {
        // Check if the player has pressed the fire button and if enough time has elapsed since they last fired
        if (!m_Shooting)
        {
            if (Input.GetButton("Fire1"))
            {
                m_Timer += Time.deltaTime;
                if (m_Timer > .29f)
                {
                    m_Animator.SetInteger("Shoot", 2);
                }
            }
            if (Input.GetButtonUp("Fire1"))
            {
                m_Animator.SetInteger("Shoot", 1);
                if (Time.time > nextFire)
                {
                    if (m_Timer > .3f)
                    {
                        Fire(m_Timer * gunForce);
                    }
                    else
                    {
                        Fire(0);
                    }
                    m_Timer = 0;
                }
            }
        }

        if (Input.GetButton("Fire2"))
        {
            if(m_Animator.GetInteger("Chain") != 2)
                m_Animator.SetInteger("Chain", 1);
        }
        if (Input.GetButtonUp("Fire2"))
        {
            m_Animator.SetInteger("Chain", 0);
        }
    }


    private IEnumerator ShotEffect()
    {
        m_Shooting = true;
        // Play the shooting sound effect
        m_Player.PlayShootSound();

        //Wait for .07 seconds
        yield return shotDuration;

        // Deactivate our line renderer after waiting
        m_Animator.SetInteger("Shoot", 0);
        m_Shooting = false;
    }
}
