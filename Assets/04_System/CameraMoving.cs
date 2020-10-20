using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CameraMoving : MonoBehaviour
{
    GameObject[] TestMove;
    NavMeshAgent nav;
    int ColliderCount = 0;
    bool EndPoint = false;

    void Start()
    {
        TestMove = GameObject.FindGameObjectsWithTag("MoveAI");
        nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        this.transform.position = new Vector3(transform.position.x, 3.0f, transform.position.z);
        nav.SetDestination(TestMove[ColliderCount].transform.position);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MoveAI")
        {

            if (ColliderCount >= TestMove.Length -1)
                EndPoint = true;
            else if (ColliderCount == 0)
                EndPoint = false;

            if (!EndPoint)
                ColliderCount++;
            else
                ColliderCount--;

            Vector3 dir = TestMove[ColliderCount].transform.position - this.transform.position;
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime*2);
        }
    }
}
