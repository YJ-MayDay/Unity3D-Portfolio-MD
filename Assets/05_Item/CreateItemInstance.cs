using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateItemInstance : MonoBehaviour
{
    public GameObject[] Items;
    public int IntanceCount;

    private GameObject[] Temp;
    private GetItemEffect[] PotionItems;
    // Start is called before the first frame update
    void Start()
    {
        for (int k = 0; k < Items.Length; k++)
        {
            for (int i = 0; i < 10; i++)
            {
                GameObject item = Instantiate<GameObject>(Items[k], this.transform);

            }
        }

        Temp = GameObject.FindGameObjectsWithTag("UseItem");
        PotionItems = new GetItemEffect[Temp.Length];
        for (int i = 0; i < Temp.Length; i++)
        {
            PotionItems[i] = Temp[i].GetComponent<GetItemEffect>();
            PotionItems[i].gameObject.SetActive(false);
        }
    }

   public void SetRespawnItem(EnemyController enemy)
    {
        int Randomint = Random.Range(0, PotionItems.Length);

        while(PotionItems[Randomint].gameObject.activeSelf == true)
        {
            Randomint = Random.Range(0, PotionItems.Length);

            if (!PotionItems[Randomint].gameObject.activeSelf == false)
                break;
        }

        if(PotionItems[Randomint].enabled)
        {
            Vector3 TempVector = enemy.transform.position;
            TempVector.y = TempVector.y + 0.5f;
            PotionItems[Randomint].transform.position = TempVector;

            PotionItems[Randomint].gameObject.SetActive(true);
        }
    }
}
