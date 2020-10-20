using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct RespawnCount
{
    public int Gobiln_Count;
    public int HopGoblin_Count;
    public int Wolf_Count;
    public int Troll_Count;
}

public class EnemyRespawn : MonoBehaviour
{
    CSVReader Reader;

    [SerializeField]
    RespawnCount[] respawnCounts;
    GameObject[] obj;

    List<GameObject> EnemyList;
    EnemyPooling Pooling;

    int StageCount = 0;
    public int GetStageCount() { return StageCount; }

    int Goblin_Count = 0;
    int HopGoblin_Count = 0;
    int Wolf_Count = 0;
    int Troll_Count = 0;

    int TotalCount = 0;

    Transform TempParent;

    float SpendTime = 0.0f;

    bool IsStagePlaying = false;
    public bool GetIsStagePlaying() { return IsStagePlaying; }
    public bool SetIsStagePlaying(bool value) { return IsStagePlaying = value; }

    bool IsSpawn = true;
    public bool SetIsSpawn(bool value) { return IsSpawn = value; }

    void Start()
    {
        Pooling = GameObject.Find("EnemyPooling").GetComponent<EnemyPooling>();

        EnemyList = Pooling.GetEnemyList();

        List<Dictionary<string,object>> data = CSVReader.Read("RespawnCount");
        respawnCounts = new RespawnCount[data.Count];

        for(int i = 0; i < data.Count; i++)
        {
            respawnCounts[i].Gobiln_Count = (int)data[i]["Goblin"];
            respawnCounts[i].HopGoblin_Count = (int)data[i]["Hopgoblin"];
            respawnCounts[i].Wolf_Count = (int)data[i]["Wolf"];
            respawnCounts[i].Troll_Count = (int)data[i]["Troll"];
        }

        obj = GameObject.FindGameObjectsWithTag("RespawnSpot");

    }

    // Update is called once per frame
    void Update()
    {
        if (IsStagePlaying)
        {
            if (IsSpawn)
            {
                SpendTime += Time.deltaTime;

                int Count = respawnCounts[StageCount].Gobiln_Count +
                            respawnCounts[StageCount].HopGoblin_Count +
                            respawnCounts[StageCount].Wolf_Count +
                            respawnCounts[StageCount].Troll_Count;

                if (SpendTime >= 1.0f && TotalCount < Count)
                {
                    if (respawnCounts[StageCount].Gobiln_Count != 0 ||
                        respawnCounts[StageCount].HopGoblin_Count != 0 ||
                        respawnCounts[StageCount].Wolf_Count != 0 ||
                        respawnCounts[StageCount].Troll_Count != 0)
                    {
                        int RandomCount = Random.Range(0, obj.Length);

                        if (Goblin_Count < respawnCounts[StageCount].Gobiln_Count)
                        {
                            SpawnEnemy(EnemyList[Goblin_Count], RandomCount);
                            Goblin_Count++;
                        }
                        else if (HopGoblin_Count < respawnCounts[StageCount].HopGoblin_Count)
                        {
                            SpawnEnemy(EnemyList[HopGoblin_Count + 20], RandomCount);
                            HopGoblin_Count++;
                        }
                        else if (Troll_Count < respawnCounts[StageCount].Troll_Count)
                        {
                            SpawnEnemy(EnemyList[Troll_Count + 40], RandomCount);
                            Troll_Count++;
                        }
                        else if (Wolf_Count < respawnCounts[StageCount].Wolf_Count)
                        {
                            SpawnEnemy(EnemyList[Wolf_Count + 60], RandomCount);
                            Wolf_Count++;
                        }
                        SpendTime = 0.0f;
                    }
                    TotalCount = Goblin_Count + HopGoblin_Count + Wolf_Count + Troll_Count;
                }
                if (TotalCount == Count)
                    IsSpawn = false;
            }
        }
    }

    void SpawnEnemy(GameObject Enemy, int random)
    {
        TempParent = Enemy.transform.parent;

        Enemy.transform.SetParent(obj[random].transform);
        Enemy.transform.SetPositionAndRotation(obj[random].transform.position, obj[random].transform.rotation);
        Enemy.SetActive(true);
        Enemy.GetComponent<EnemyController>().SetEnemyActivate();
    }

    public void  EnemyDeathCheck()
    {
        int temp = 0;
        DeathCheck(EnemyList, temp);
    }

    void DeathCheck(List<GameObject> EnemyChar2, int temp)
    {
        if(temp < EnemyChar2.Count && EnemyChar2[temp].GetComponent<EnemyController>().GetState() == State.Death)
        {
            temp++;
            DeathCheck(EnemyChar2, temp);

            if (temp == EnemyChar2.Count && EnemyChar2[temp - 1].GetComponent<EnemyController>().GetState() == State.Death)
            {
                IsStagePlaying = false;
            }
        }
    }

    public void NextStage()
    {
        Goblin_Count = HopGoblin_Count = Troll_Count = Wolf_Count = 0;
        TotalCount = 0;

        StageCount++;
        GameObject.Find("PlayUI").transform.Find("CountDown").transform.Find("Text").GetComponent<UICountDown>().SetCountState();
        GameObject.Find("PlayUI").transform.Find("CountDown").gameObject.SetActive(true);
    }

    public void ReCallEnemy(EnemyController Enemy)
    {
        Enemy.transform.SetParent(TempParent);
    }
}
