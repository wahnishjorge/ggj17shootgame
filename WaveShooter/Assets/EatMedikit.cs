using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class EatMedikit : MonoBehaviour {
    public int m_Life = 3;
    private bool m_Used = false;
    private FirstPersonController _player;
    private FirstPersonController m_Player
    {
        get
        {
            if (_player == null)
                _player = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
            return _player;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !m_Used && m_Player.m_Life < m_Player.m_MaxLife)
        {
            m_Used = true;
            if (m_Player.m_Life + m_Life >= m_Player.m_MaxLife)
                m_Player.m_Life = m_Player.m_MaxLife;
            else
                m_Player.m_Life += m_Life;
            Destroy(gameObject);
        }
    }
}
