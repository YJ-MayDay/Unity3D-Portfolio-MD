using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPooling : MonoBehaviour
{
    public GameObject[] ChildObject;
    public GameObject[] Prefab;

    List<GameObject> Enemy = new List<GameObject>();
    public List<GameObject> GetEnemyList() { return Enemy; }

    const int MaxCount = 20;

    void Start()
    {
        ChildObject = GameObject.FindGameObjectsWithTag("Pooling");
        for (int k = 0; k < Prefab.Length; k++)
        {
            for (int i = 0; i < MaxCount; i++)
            {
                Enemy.Add(CreatePooling(ChildObject[k], Prefab[k]));
            }
        }
    }

    GameObject CreatePooling(GameObject ChildObject, GameObject Prefab)
    {
        GameObject Temp = Instantiate(Prefab);
        Temp.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        Temp.transform.SetParent(ChildObject.transform);
        Temp.SetActive(false);
        Temp.GetComponent<EnemyController>().SetState(State.Death);
        return Temp;
    }
}
