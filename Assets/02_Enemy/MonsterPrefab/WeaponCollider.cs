using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    BoxCollider boxCollider;
    SphereCollider sphereCollider;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null)
            sphereCollider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && this.transform.GetComponentInParent<EnemyController>().GetIsEnemyAttackState())
        {
            other.GetComponent<PlayerController>().status.DecreaseHealth(this.transform.GetComponentInParent<EnemyController>().status.GetAttackDamage());
            //Debug.Log(other.GetComponent<PlayerController>().status.GetHealth());
           // other.GetComponent<PlayerUI>().SetUI();
        }
    }
}
