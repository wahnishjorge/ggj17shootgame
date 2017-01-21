using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chainsaw : MonoBehaviour
{
    public Animator m_Animator;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (Input.GetButton("Fire2"))
            {
                Zombie sZombie = other.gameObject.GetComponent<Zombie>();
                sZombie.Cutted();
                m_Animator.SetInteger("Chain", 2);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (Input.GetButton("Fire2"))
                m_Animator.SetInteger("Chain", 1);
            else
                m_Animator.SetInteger("Chain", 0);
        }
    }
}
