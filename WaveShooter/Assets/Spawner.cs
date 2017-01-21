using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour {
    public List<EnemyList> m_Enemys = new List<EnemyList>();
	// Use this for initialization
	void Start ()
    {
        for (int i = 0; i < m_Enemys.Count; i++)
        {
            for (int j = 0; j < m_Enemys[i].m_Count; j++)
                Instantiate(m_Enemys[i].m_EnemyObj,Spawn(),Quaternion.identity);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    Vector3 Spawn()
    {
        Vector3 randomDirection = GetRandomPosition();
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, 100, 1);
        return  hit.position;

    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color32(0, 255, 0, 50);
        Gizmos.DrawCube(transform.position, transform.localScale);
    }

    Vector3 GetRandomPosition()
    {
        float sStartX = transform.position.x - transform.localScale.x / 2f;
        float sEndX = transform.position.x + transform.localScale.x / 2f;
        if (sEndX < sStartX)
        {
            float sTmp = sStartX;
            sStartX = sEndX;
            sEndX = sTmp;
        }

        float sStartZ = transform.position.z - transform.localScale.z / 2f;
        float sEndZ = transform.position.z + transform.localScale.z / 2f;
        if (sEndZ < sStartZ)
        {
            float sTmp = sStartZ;
            sStartZ = sEndZ;
            sEndZ = sTmp;
        }

        return new Vector3(Random.Range(sStartX, sEndX), transform.position.y + transform.localScale.y, Random.Range(sStartZ, sEndZ));
    }
}

[System.Serializable]
public class EnemyList
{
    public GameObject m_EnemyObj;
    public int m_Count;
}