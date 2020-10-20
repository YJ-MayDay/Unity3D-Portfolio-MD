using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public struct SkillSet
{
    [Tooltip("툴팁 테스트")]
    public string Name;
    public string ImagePath;
    public string ClipName;
    public int NeedMana;
    public float CoolTime;
    public float CurrentCoolTime;

    public ParticleSystem SkillEffect;

    public bool IsReadySkill;
    public bool IsSkillActivate;
}


public class SkillList : MonoBehaviour
{
    Image[] PlayerSkill;

    public SkillSet[] SkillData;
    public SkillSet[] GetSkillList() { return SkillData; }

    public bool SetSkillActivate(SkillSet skill,  bool value) { return skill.IsSkillActivate = value; }

    void Start()
    {
        List<Dictionary<string, object>> List = CSVReader.Read("SkillList");

        SkillData = new SkillSet[List.Count];
        for(int i = 0;i<List.Count;i++)
        {
            SkillData[i].Name = List[i]["Name"].ToString();
            SkillData[i].ImagePath = List[i]["Image"].ToString();
            SkillData[i].ClipName = List[i]["ClipName"].ToString();
            SkillData[i].NeedMana = int.Parse(List[i]["NeedMana"].ToString());
            SkillData[i].CoolTime = float.Parse(List[i]["CoolTime"].ToString());
            SkillData[i].CurrentCoolTime = float.Parse(List[i]["CurrentCoolTime"].ToString());

            SkillData[i].IsReadySkill = bool.Parse(List[i]["ReadySkill"].ToString());
            SkillData[i].IsSkillActivate = bool.Parse(List[i]["IsActivate"].ToString());

            SkillData[i].SkillEffect = Resources.Load<ParticleSystem>(List[i]["ParticleName"].ToString());
        }

        GameObject[] ImageParent = GameObject.FindGameObjectsWithTag("Skill");
        for(int i = 0;i< ImageParent.Length; i++)
        {
            if (SkillData[i].Name == null) break;

            Image SkillImage = ImageParent[i].transform.GetChild(0).GetComponent<Image>();
            SkillImage.sprite = Resources.Load<Sprite>(SkillData[i].ImagePath);
           
        }
        
    }
}
