using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySearch : MonoBehaviour
{
    GameObject Player;
    float NearDistance;
    bool IsRange = false;
    void Start()
    {
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find("RespawnSpot").GetComponent<EnemyRespawn>().GetIsStagePlaying())
        {
            GameObject[] Enemys = GameObject.FindGameObjectsWithTag("Enemy");
            if(Enemys.Length >1)
            {
                for(int i = 0;i< Enemys.Length;i++)
                {
                    for(int k = 0; k<Enemys.Length ;k++)
                    {
                        float ADistance = Vector3.Distance(Player.transform.position, Enemys[i].transform.position);
                        float BDistance = Vector3.Distance(Player.transform.position, Enemys[k].transform.position);

                        if(ADistance < BDistance)
                        {
                            NearDistance = ADistance;

                            Vector3 temp = Enemys[i].transform.position;
                            temp.y = 90;
                            transform.LookAt(temp);
                            EnemyNearDistance(NearDistance);
                        }
                    }
                }
            }
            else
            {
                float EnemyDistance = Vector3.Distance(Player.transform.position, Enemys[0].transform.position);
                NearDistance = EnemyDistance;
                transform.LookAt(Enemys[0].transform);

                Vector3 vec = Enemys[0].transform.position - transform.position;
                vec.y = 90;
                Quaternion q = Quaternion.LookRotation(vec).normalized;

                transform.rotation = q;
                EnemyNearDistance(NearDistance);
            }
        }
    }

    void EnemyNearDistance(float Distance)
    {
        if (Distance > 15.0f && Distance < 25f)
        {
            this.GetComponent<MeshRenderer>().material.color = Color.green;
        }
        else if( Distance < 15.0f && Distance > 10f)
        {
            this.GetComponent<MeshRenderer>().material.color = Color.yellow;
        }
        else if(Distance < 10f )
        {
            this.GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }
}
