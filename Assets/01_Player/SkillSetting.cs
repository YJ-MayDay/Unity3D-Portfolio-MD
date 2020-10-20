using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSetting : MonoBehaviour
{
    Image[] CurrentSkill;
    SkillSet[] PlayerSkillSet;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] playerSkill = GameObject.FindGameObjectsWithTag("PlayerSkill");
        SkillSet[] List = GameObject.Find("SkillList").GetComponent<SkillList>().GetSkillList();

        for(int i = 0;i<playerSkill.Length;i++)
        {
            playerSkill[i].transform.GetChild(0).GetComponent<Image>().type = Image.Type.Filled;
            playerSkill[i].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(List[i].ImagePath.ToString());
            if (!List[i].IsSkillActivate)
                playerSkill[i].transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);

            playerSkill[i].transform.GetChild(0).GetComponent<PlayerSKillSystem>().SetThisSkilldata(List[i]);
        }
    }
}
