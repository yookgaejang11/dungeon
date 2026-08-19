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
    public Dictionary<string, int> invenValue = new();
    public PlayerStatus status;
    public string selectedSkill;
    public int maxInven = 6;
    public int curInven;
    public Slider hpSlider;
    public Text hpText;
    public Slider mpSlider;
    public Text mpText;
    public GameObject SkillSelectObj;
    public GameObject bagUI;
    public string selectedItem;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        invenValue.Add("힘의 영약", 1);
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
        hpText.text = curHp + "/" + maxHp;


        mpSlider.maxValue = maxMp;
        mpSlider.value = curMp;
        mpText.text = curMp + "/" + maxMp;


        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            SkillSelectObj.SetActive(false);
            bagUI.SetActive(false);
            status = PlayerStatus.battle;
        }
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
                    Debug.Log(this.CurculateStr());
                    Attack(this, hit.collider.GetComponent<Enemy>(), 1,false);
                    status = PlayerStatus.battle;
                    actionCount -= 1;
                    animator.SetTrigger("attack");
                } 
            }


        }
    }

    public void SelectPotion(string value)
    {
        if (value == selectedItem)
        {
            UsePotion(selectedItem);
        }
        else
        {
            selectedItem = value;
        }
    }


    public void UsePotion(string value)
    {
        switch(value)
        {
            case "빨간 포션":
                Heal(maxHp * 0.2f);
                break;
            case "파란 포션":
                HealMp(maxMp * 0.2f);
                break;
            case "힘의 영약":
                effects.Add(new effect(effectType.strUp, 0.3f, 5));
                break;
            case "지식의 영약":
                effects.Add(new effect(effectType.skillDmgUp, 0.3f, 5));
                break;
            case "회피의 물약":
                effects.Add(new effect(effectType.avdUp, 2, 5));
                break;



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
                            animator.SetTrigger("skill");
                        }
                    }
                }
                break;
            default:
                UseSkill(selectedSkill,this, null, TurnManager.instance.mobList.Cast<Unit>().ToList());
                animator.SetTrigger("skill");
                break;
        }

        SkillSelectObj.SetActive(false);
        bagUI.SetActive(false);
        status = PlayerStatus.battle;

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
            SkillSelectObj.SetActive(true);
        }
    }


    public void SkillSelect(string skillName)
    {
        status = PlayerStatus.useSkill;
        selectedSkill = skillName;
    }
}
