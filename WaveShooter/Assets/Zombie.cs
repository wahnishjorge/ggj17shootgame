using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Zombie : MonoBehaviour
{
    public bool m_Move = true;
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


    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update () {
        MoveToPlayer();


    }

    void MoveToPlayer()
    {
        if (m_Move)
        {
            m_Nav.SetDestination(m_PlayerObj.transform.position);
        }
    }
}
