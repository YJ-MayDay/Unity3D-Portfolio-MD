using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class PlayerSKillSystem : MonoBehaviour, IDropHandler
{
    public SkillSet SkillData;
    public SkillSet GetThisSkillData() { return SkillData; }
    public SkillSet SetThisSkilldata(SkillSet skill)
    {
        return SkillData = skill;
    }
   
    public void Start()
    {
        ImageTransparent(SkillData.IsSkillActivate);
    }

    public void Update()
    {
        if (SkillData.IsSkillActivate)
        {
            if (!SkillData.IsReadySkill)
            {
                SkillData.CurrentCoolTime += Time.deltaTime;

                float ratio = (SkillData.CurrentCoolTime / SkillData.CoolTime);
                this.GetComponent<Image>().fillAmount = ratio;
                this.GetComponent<Image>().color = new Color(this.GetComponent<Image>().color.r,
                                                             this.GetComponent<Image>().color.g,
                                                             this.GetComponent<Image>().color.b,
                                                             0.5f);

                if (SkillData.CoolTime < SkillData.CurrentCoolTime)
                {
                    SkillData.IsReadySkill = true;
                    SkillData.CurrentCoolTime = 0.0f;
                    this.GetComponent<Image>().color = new Color(this.GetComponent<Image>().color.r,
                                                             this.GetComponent<Image>().color.g,
                                                             this.GetComponent<Image>().color.b,
                                                             1.0f);
                }
            }
        }
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        Skill_DragDrop DropSkill = eventData.pointerDrag.GetComponent<Skill_DragDrop>();
        SkillSet[] Data = GameObject.Find("SkillList").GetComponent<SkillList>().GetSkillList();

        this.GetComponent<Image>().sprite = DropSkill.GetComponent<Image>().sprite;
        this.GetComponent<Image>().type = Image.Type.Filled;
        this.GetComponent<Image>().fillAmount = 1.0f;

        switch (this.name)
        {
            case "Skill0":
                for(int i = 0;i< Data.Length;i++)
                {
                    if(DropSkill.SkillName == Data[i].Name)
                    {
                        SkillData = Data[i];
                        ImageTransparent(Data[i].IsSkillActivate);
                    }
                }
                break;

            case "Skill1":
                for (int i = 0; i < Data.Length; i++)
                {
                    if (DropSkill.SkillName == Data[i].Name)
                    {
                        SkillData = Data[i];
                        ImageTransparent(Data[i].IsSkillActivate);
                    }
                }
                break;
        }
    }

    void ImageTransparent(bool value)
    {
        if (value)
        {
            this.GetComponent<Image>().color = new Color(this.GetComponent<Image>().color.r,
                                        this.GetComponent<Image>().color.g,
                                        this.GetComponent<Image>().color.b,
                                        1.0f);
        }
        else
        {
            this.GetComponent<Image>().color = new Color(this.GetComponent<Image>().color.r,
                                        this.GetComponent<Image>().color.g,
                                        this.GetComponent<Image>().color.b,
                                        0.5f);
        }
    }
}
