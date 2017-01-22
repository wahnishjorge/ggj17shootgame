using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chainsaw : MonoBehaviour
{
    public Weapon m_Weapon;
    public Animator m_Animator;
    [Range(0, 100)]
    public int m_MakeExplotionPercent = 50;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (Input.GetButton("Fire2"))
            {
				if (m_Animator.GetInteger("Chain") != 2)
					CameraShake.instance.Shake(0.5f,0.1f);

				Zombie sZombie = other.gameObject.GetComponent<Zombie>();
                bool sExplode = false;
                int sExplotePercent = Random.Range(1, 100);
                if (sExplotePercent <= m_MakeExplotionPercent)
                    sExplode = true;

                sZombie.Cutted(sExplode);
                m_Animator.SetInteger("Chain", 2);
                m_Weapon.PlayCutSound();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (Input.GetButton("Fire2"))
            {
                m_Animator.SetInteger("Chain", 1);
                m_Weapon.PlayChainSound();
            }
            else
            {
                m_Animator.SetInteger("Chain", 0);
                m_Weapon.StopSound();
            }
        }
    }
}
