using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartEffect : MonoBehaviour
{
    Text StartText;

    void Start()
    {
        StartText = GetComponent<Text>();        
    }

    void Update()
    {
        float test = this.transform.parent.GetChild(0).GetComponent<UICountDown>().GetCountDown();

        SetText();

        if (test < 0.0f)
        {
            this.transform.parent.gameObject.SetActive(false);
            GameObject.Find("RespawnSpot").GetComponent<EnemyRespawn>().SetIsStagePlaying(true);
            GameObject.Find("RespawnSpot").GetComponent<EnemyRespawn>().SetIsSpawn(true);
        }
    }

    void SetText()
    {
        if (GameObject.Find("RespawnSpot").GetComponent<EnemyRespawn>().GetStageCount() == 0)
        {
            StartText.fontSize = 50;
            StartText.text = "Start";
        }
        else
        {
            StartText.fontSize = 40;
            StartText.text = "Next\nStage";
        }
    }
}
