﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class Zombie : MonoBehaviour
{
    private float m_DestroySeconds = 1;
    private bool m_Die = false;
	public GameObject _blood;
	public SkinnedMeshRenderer _renderer;
    public bool m_Move = true;
    public float m_MinSpeed = 2f;
    public float m_MaxSpeed = 3.5f;
    public int m_Life = 3;
    public GameObject m_explotionGuy;
    private NavMeshAgent _nav;
    private NavMeshAgent m_Nav
    {
        get
        {
            if (_nav == null)
                _nav = GetComponent<NavMeshAgent>();
            return _nav;
        }
    }

    private FPSController _playerObj;
    private FPSController m_PlayerObj
    {
        get
        {
            if (_playerObj == null)
                _playerObj = GameObject.FindGameObjectWithTag("Player").GetComponent<FPSController>();
            return _playerObj;
        }
    }

    private Rigidbody _Rigidbody;
    private Rigidbody m_Rigidbody
    {
        get
        {
            if (_Rigidbody == null)
                _Rigidbody = GetComponent<Rigidbody>();
            return _Rigidbody;
        }
    }

    private CapsuleCollider _collider;
    private CapsuleCollider m_Collider
    {
        get
        {
            if (_collider == null)
                _collider = GetComponent<CapsuleCollider>();
            return _collider;
        }
    }

    private Animator _anim;
    private Animator m_Anim
    {
        get
        {
            if (_anim == null)
                _anim = GetComponent<Animator>();
            return _anim;
        }
    }

    void Start()
    {
        m_Nav.speed = Random.Range(m_MinSpeed, m_MaxSpeed);
    }

    // Update is called once per frame
    void Update() {
        if (m_Life > 0)
            MoveToPlayer();
        else
        {
            if (!m_Die)
            {
                m_Die = true;
                Destroy(gameObject, m_DestroySeconds);
                WaveManager.ZombieDie();
            }
        }
    }

    void MoveToPlayer()
    {
        if (m_Life > 0 && m_Move && m_Nav.enabled && m_PlayerObj.m_Life > 0)
        {
            m_Anim.SetInteger("Move", 1);
			gameObject.transform.LookAt( new Vector3 (m_PlayerObj.transform.position.x, gameObject.transform.position.y, m_PlayerObj.transform.position.z));
            m_Nav.SetDestination(m_PlayerObj.transform.position);
            if (!m_MakeDamage && Vector3.Distance(transform.position, m_PlayerObj.transform.position) < 1.8f)
            {
                StartCoroutine(SetDamage());
            }
        } else
        {
            if (m_Life > 0)
            {
                m_Anim.SetInteger("Move", 0);
                m_Nav.enabled = false;
            }
        }
    }

    private bool m_MakeDamage = false;
    IEnumerator SetDamage()
    {
        m_MakeDamage = true;
        yield return new WaitForSeconds(0.6f);
        if (m_Life > 0)
        {
            m_PlayerObj.GetDamaged();
        }
        m_MakeDamage = false;
    }

    IEnumerator GetDamage(Vector3 xForce, bool sExplote)
    {
        if (!sExplote)
            m_Life--;
        else
        {
            m_Life = 0;
            Explotion();
        }
        
        m_Nav.enabled = false;
        m_Rigidbody.isKinematic = false;
        m_Rigidbody.AddForce(xForce);
        yield return new WaitForSeconds(1.5f);
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 100, 1))
        {
            if (Vector3.Distance(transform.position, hit.position) < 1.3f)
            {
                if (m_Life > 0)
                {
                    m_Nav.enabled = true;
                    m_Rigidbody.isKinematic = true;
                }
                else
                {
                    m_Nav.enabled = false;
                    m_Collider.isTrigger = true;
                }
            }else
            {
                m_Life = 0;
            }
        }
    }

	void Explotion()
	{
		m_Nav.enabled = false;
		_renderer.enabled = false;
		m_explotionGuy.gameObject.SetActive(true);
		Instantiate(_blood, transform.position + Vector3.up , Quaternion.identity);
	}


    IEnumerator GetCutted(bool sExplode)
    {
        if (m_Nav.enabled)
        {
            m_Nav.enabled = false;
            m_Rigidbody.isKinematic = false;
            m_Rigidbody.AddForce(m_PlayerObj.transform.forward * 10);

            yield return new WaitForSeconds(3f);

            if (!sExplode)
            {
                if (m_Life - 2 > 0)
                    m_Life -= 2;
                else
                {
                    m_Life = 0;
                }
            }
            else
            {
                m_Life = 0;
                Explotion();
            }


            if (m_Life <= 0)
            {
                m_Collider.isTrigger = true;
                m_DestroySeconds = 3f;
            }
            else
            {
                m_Nav.enabled = true;
                m_Rigidbody.isKinematic = true;
            }
        }
    }

    public void Cutted(bool sExplode)
    {
        if (m_Life > 0)
        {
            StartCoroutine(GetCutted(sExplode));
        }
    }

    public void Damage(Vector3 xForce, bool sExplote = false)
    {
        if(m_Life > 0)
            StartCoroutine(GetDamage(xForce, sExplote));
    }
}
