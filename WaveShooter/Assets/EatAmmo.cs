using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class EatAmmo : MonoBehaviour
{
    public int m_Ammo = 5;
    private bool m_Used = false;
    private FPSController _player;
    private FPSController m_Player
    {
        get
        {
            if (_player == null)
                _player = GameObject.FindGameObjectWithTag("Player").GetComponent<FPSController>();
            return _player;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !m_Used && m_Player.m_Ammo < m_Player.m_MaxAmmo)
        {
            m_Used = true;
            if (m_Player.m_Ammo + m_Ammo >= m_Player.m_MaxAmmo)
                m_Player.m_Ammo = m_Player.m_MaxAmmo;
            else
                m_Player.m_Ammo += m_Ammo;
			LevelManager.instance.SetAmmo(m_Player.m_Ammo.ToString() + "/" + m_Player.m_MaxAmmo.ToString());
            Destroy(gameObject);
        }
    }
}
