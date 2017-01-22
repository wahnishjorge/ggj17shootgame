using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomSpawn : MonoBehaviour
{
    public List<Invokation> m_Creator = new List<Invokation>();
    public WaveManager m_WaveManager;
    public int m_MinWaves = 2;
    public int m_MaxWaves = 5;
    public List<SpawnList> m_Spawns = new List<SpawnList>();

    public Light m_DirectionalLight;
    public float m_DirectionalLightXMinRotation;
    public float m_DirectionalLightXMaxRotation;
    public float m_DirectionalLightYMinRotation;
    public float m_DirectionalLightYMaxRotation;
    public float m_DirectionalLightIntencityMin = 0.3f;
    public float m_DirectionalLightIntencityMax = 1f;

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

    void Awake()
    {
        m_WaveManager.m_WaveExist = false;
    }

    // Use this for initialization
    void Start()
    {
        Create();
        int sWaveCount = Random.Range(m_MinWaves, m_MaxWaves);
        for (int i=0;i< sWaveCount;i++)
        {
            Waves sWave = new Waves();
            sWave.m_Objects = m_Spawns;
            m_WaveManager.m_Wave.Add(sWave);
        }
        m_WaveManager.m_WaveExist = true;
        m_WaveManager.m_IsRandomLevel = true;
        m_DirectionalLight.color = new Color(Random.value,Random.value,Random.value,Random.value);
        m_DirectionalLight.intensity = Random.Range(m_DirectionalLightIntencityMin, m_DirectionalLightIntencityMax);
        LightRotation();
    }

    void LightRotation()
    {
        Vector3 sRotation = m_DirectionalLight.transform.rotation.eulerAngles;
        if (m_DirectionalLightXMinRotation != 0 || m_DirectionalLightXMaxRotation != 0)
            sRotation = new Vector3(Random.Range(m_DirectionalLightXMinRotation, m_DirectionalLightXMaxRotation), sRotation.y, sRotation.z);
        if (m_DirectionalLightYMinRotation != 0 || m_DirectionalLightYMaxRotation != 0)
            sRotation = new Vector3(sRotation.x, Random.Range(m_DirectionalLightYMinRotation, m_DirectionalLightYMaxRotation), sRotation.z);

        m_DirectionalLight.transform.rotation = Quaternion.Euler(sRotation);
    }

    Vector3 SpawnPosition()
    {
        bool sAgain = true;
        Vector3 sPosition = Vector3.zero;
        while(sAgain)
        {
            sAgain = false;
            Vector3 randomDirection = GetRandomPosition();
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, 100, 1);
            sPosition = hit.position;

            if (Vector3.Distance(m_PlayerObj.transform.position, hit.position) < 3)
                sAgain = true;

            if (!sAgain)
            {
                foreach (Invokation sCreate in m_Creator)
                {
                    foreach (GameObject sGO in sCreate.m_Instances)
                    {
                        if (Vector3.Distance(sGO.transform.position, hit.position) < sCreate.m_Distance)
                        {
                            sAgain = true;
                            break;
                        }
                    }
                    if (sAgain)
                        break;
                }
            }
        }
        return sPosition;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color32(0, 0, 255, 50);
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

    public void Spawn(List<SpawnList> xObjects)
    {
        for (int i = 0; i < xObjects.Count; i++)
        {
            xObjects[i].m_Count = Random.Range(xObjects[i].m_CountMin, xObjects[i].m_CountMax);
            for (int j = 0; j < xObjects[i].m_Count; j++)
                Instantiate(xObjects[i].m_Obj, SpawnPosition(), Quaternion.identity);
        }
    }
    
    public void Create()
    {
        foreach(Invokation sCreate in m_Creator)
        {
            int sCount = Random.Range(sCreate.m_MinCount, sCreate.m_MaxCount);
            for(int i=0;i<sCount;i++)
            {
                Vector3 sRotation = Quaternion.identity.eulerAngles;

                if (sCreate.m_XRotationMin != 0 || sCreate.m_XRotationMax != 0)
                    sRotation = new Vector3(Random.Range(sCreate.m_XRotationMin, sCreate.m_XRotationMax), sRotation.y, sRotation.z);
                if (sCreate.m_YRotationMin != 0 || sCreate.m_YRotationMax != 0)
                    sRotation = new Vector3(sRotation.x, Random.Range(sCreate.m_YRotationMin, sCreate.m_YRotationMax), sRotation.z);
                if (sCreate.m_ZRotationMin != 0 || sCreate.m_ZRotationMax != 0)
                    sRotation = new Vector3(sRotation.x, sRotation.y, Random.Range(sCreate.m_ZRotationMin, sCreate.m_ZRotationMax));

                sCreate.m_Instances.Add(Instantiate(sCreate.m_Object, SpawnPosition(), Quaternion.Euler(sRotation)));
                if (sCreate.m_IsCarvingNavMesh)
                {
                    sCreate.m_Instances[sCreate.m_Instances.Count - 1].AddComponent<NavMeshObstacle>();
                    NavMeshObstacle sNavObs = sCreate.m_Instances[sCreate.m_Instances.Count - 1].GetComponent<NavMeshObstacle>();
                    sNavObs.carving = true;
                    sCreate.m_Instances[sCreate.m_Instances.Count - 1].AddComponent<BoxCollider>();
                }
            }
        }
    }
}

[System.Serializable]
public class Invokation
{
    public GameObject m_Object;
    public List<GameObject> m_Instances = new List<GameObject>();
    public int m_MinCount = 2;
    public int m_MaxCount = 4;
    public float m_Distance;

    public float m_XRotationMin = 0;
    public float m_XRotationMax = 0;
    public float m_YRotationMin = 0;
    public float m_YRotationMax = 0;
    public float m_ZRotationMin = 0;
    public float m_ZRotationMax = 0;

    public bool m_IsCarvingNavMesh = true;
}