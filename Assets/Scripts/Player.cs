using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerStatus
{
    battle,
    Attack,
    useSkill,
    none
}


public class Player : Unit
{
    public Dictionary<string, int> invenValue;
    public PlayerStatus status;
    public string selectedSkill;
    public int maxInven = 6;
    public int curInven;
    public Slider hpSlider;
    public Text hpText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TurnManager.instance.turnUnit == this && actionCount == 0)
        {
            TurnManager.instance.NextTurn();
        }
        AttackTargetSelect();
        SkillSelectTarget();

        hpSlider.maxValue = maxHp;
        hpSlider.value = curHp;
    }

    public void EquipItem(string itemName)
    {
        if (EquipmentData.equipments[itemName].equipmentType== EquipmentType.weapon)
        {
            EquipedWeapon = EquipmentData.equipments[itemName].name;
        }
        else if(EquipmentData.equipments[itemName].equipmentType == EquipmentType.cloth)
        {
            EquipedEquipment = EquipmentData.equipments[itemName].name;
        }
    }

   void AttackTargetSelect()
    {
        if (status != PlayerStatus.Attack) { return; }

        if(Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("Enemy")))
            {
                if(hit.collider.GetComponent<Enemy>() != null)
                {
                    Attack(this, hit.collider.GetComponent<Enemy>(), this.CurculateStr(),false);
                    status = PlayerStatus.battle;
                    actionCount -= 1;
                } 
            }


        }
    }

    public void SkillSelectTarget()
    {
        if (status != PlayerStatus.useSkill) { return; }

        switch(SkillData.skills[selectedSkill].target)
        {
            case "인접":
            case "단일":
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("Enemy")))
                    {
                        if (hit.collider.GetComponent<Enemy>() != null)
                        {
                            UseSkill(selectedSkill, this, hit.collider.GetComponent<Enemy>(), TurnManager.instance.mobList.Cast<Unit>().ToList());
                            status = PlayerStatus.battle;
                        }
                    }
                }
                break;
            default:
                UseSkill(selectedSkill,this, null, TurnManager.instance.mobList.Cast<Unit>().ToList());
                break;
        }


       
    }


    public void Attack()
    {
        if(TurnManager.instance.turnUnit == this)
        {
            status = PlayerStatus.Attack;
        }
    }
    public void UseSkill()
    {
        if (TurnManager.instance.turnUnit == this)
        {
            //스킬 창 열기
        }
    }


    public void SkillSelect(string skillName)
    {
        status = PlayerStatus.useSkill;
        selectedSkill = skillName;
    }
}
