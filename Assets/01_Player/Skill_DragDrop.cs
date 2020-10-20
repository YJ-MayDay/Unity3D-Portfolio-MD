using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Skill_DragDrop : MonoBehaviour,IBeginDragHandler, IDragHandler,IEndDragHandler, IPointerClickHandler
{
    public Skill_DragDrop DragImage;

    public string SkillName;
    public static Vector2 Defaultposition;
    Transform StartParent;

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        Defaultposition = this.transform.position;
        StartParent = transform.parent;
        transform.SetParent(GameObject.Find("SkillList").transform);

        GetComponent<Image>().raycastTarget = false;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Vector2 CurrentPosition = Input.mousePosition;
        this.transform.position = CurrentPosition;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        this.transform.position = Defaultposition;
        transform.SetParent(StartParent);
        transform.localPosition = Vector3.zero;

        GetComponent<Image>().raycastTarget = true;
        DragImage = null;
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        GetComponent<Image>().color = new Color(1,1,1,1);

        SkillSet[] TempList = GameObject.Find("SkillList").GetComponent<SkillList>().GetSkillList();
        for(int i = 0;i< TempList.Length;i++)
        {
            if(SkillName == TempList[i].Name)
            {
                GameObject[] CurrentSkill = GameObject.FindGameObjectsWithTag("PlayerSkill");
                for (int k = 0; k < CurrentSkill.Length; k++)
                {
                    if (SkillName == CurrentSkill[k].GetComponentInChildren<PlayerSKillSystem>().SkillData.Name)
                        CurrentSkill[k].transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
                }

                TempList[i].IsSkillActivate = true;
            }
        }
    }
}
