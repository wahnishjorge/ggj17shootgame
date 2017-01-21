using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Zombie : MonoBehaviour
{
    public bool m_Move = true;
    public int m_Life = 3;
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

    private GameObject _playerObj;
    private GameObject m_PlayerObj
    {
        get
        {
            if (_playerObj == null)
                _playerObj = GameObject.FindGameObjectWithTag("Player");
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
    
	
	// Update is called once per frame
	void Update () {
        if (m_Life > 0)
            MoveToPlayer();
    }

    void MoveToPlayer()
    {
        if (m_Life > 0 && m_Move && m_Nav.enabled)
        {
            m_Nav.SetDestination(m_PlayerObj.transform.position);
        }
    }

    IEnumerator GetDamage(Vector3 xForce)
    {
        m_Life--;
        bool sRespawn = false;
        m_Nav.enabled = false;
        m_Rigidbody.isKinematic = false;
        m_Rigidbody.AddForce(xForce);
        yield return new WaitForSeconds(1.5f);
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 100, 1))
        {
            if (Vector3.Distance(transform.position, hit.position) < 1.3f)
            {
                sRespawn = true;
                if (m_Life > 0)
                {
                    m_Nav.enabled = true;
                    m_Rigidbody.isKinematic = true;
                }
                else
                {
                    m_Nav.enabled = false;
                    m_Collider.isTrigger = true;
                    Destroy(gameObject, 2);
                }
            }
        }
        if(!sRespawn)
        {
            Destroy(gameObject);
        }
    }


    IEnumerator GetCutted()
    {
        if (m_Nav.enabled)
        {
            m_Nav.enabled = false;
            m_Rigidbody.isKinematic = false;
            m_Rigidbody.AddForce(transform.forward * -1);

            yield return new WaitForSeconds(2f);

            if (m_Life - 2 > 0)
                m_Life -= 2;
            else
                m_Life = 0;

            if (m_Life <= 0)
            {
                m_Collider.isTrigger = true;
                Destroy(gameObject, 3);
            }
            else
            {
                m_Nav.enabled = true;
                m_Rigidbody.isKinematic = true;
            }
        }
    }

    public void Cutted()
    {
        if (m_Life > 0)
        {
            StartCoroutine(GetCutted());
        }
    }

    public void Damage(Vector3 xForce)
    {
        if(m_Life > 0)
            StartCoroutine(GetDamage(xForce));
    }
}
