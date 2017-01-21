using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{

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

    public List<SpawnList> m_Objects = new List<SpawnList>();
	// Use this for initialization
	void Start ()
    {
        for (int i = 0; i < m_Objects.Count; i++)
        {
            for (int j = 0; j < m_Objects[i].m_Count; j++)
                Instantiate(m_Objects[i].m_Obj, Spawn(),Quaternion.identity);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    Vector3 Spawn()
    {
        Vector3 sPosition = Vector3.zero;
        for(int i=0;i<3;i++)
        { 
            Vector3 randomDirection = GetRandomPosition();
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, 100, 1);
            sPosition = hit.position;
            if (Vector3.Distance(m_PlayerObj.transform.position, hit.position) > 3)
                break;
        }
        return sPosition;
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
public class SpawnList
{
    public GameObject m_Obj;
    public int m_Count;
}