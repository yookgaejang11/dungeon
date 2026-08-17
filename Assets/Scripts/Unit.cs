using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class Unit : MonoBehaviour
{
    public int maxLevel;
    public int curLevel;

    public float maxExp;
    public float curExp;

    public float maxHp;
    public float curHp;

    public float maxMp;
    public float curMp;

    public float str;
    public float def;
    public float spd;
    public float crit;
    public float avd;

    public bool isDefence;

    public string EquipedWeapon;
    public string EquipedEquipment;

    public bool isDead;

    public List<string> haveSkills = new();

    public Dictionary<string, int> cooltime = new();
    public List<effect> effects = new();

    public int actionCount = 1;

    public float attackedDmg;


    public void SetHp(float dmg)
    {
        curHp -= dmg;
        if(curHp < 0)
        {
            curHp = 0;
            isDead = true;
        }

    }

    public void Heal(float value)
    {
        curHp += value;
        if(curHp > maxHp)
        {
            curHp = maxHp;
        }
    }   

    public bool isSkillReady(string name)
    {
        return !cooltime.ContainsKey(name) || cooltime[name] <= 0;
    }

    public void SkillCoolTime()
    {
        var key = new List<string>(cooltime.Keys);

        foreach(string name in key)
        {
            if(cooltime[name] > 0)
            {
                cooltime[name]--;
            }
        }
    }

    public void Attack(Unit attacker, Unit target, float value, bool isSkill = false)
    {
        int ran = Random.Range(1, 101);
        if(ran <= target.Curculateavd()) { return; }

        float dmg = attacker.CurculateStr() * value;
        
        if(isSkill)
        {
            dmg = CulcurlateDmg(dmg);
        }

        int ran2 = Random.Range(1, 101);

        if(ran2 <= attacker.Curculatecrit()) { dmg *=2; }

        float defence = dmg * (1 - target.Curculatedef() / 100f);

        dmg = Mathf.Max(defence, dmg * 0.1f);

        if(target.isDefence)
        {
            dmg *= 1 - ((50 + target.curLevel * 3) / 100f);  
        }

        if(dmg <0)
        {
            dmg = 0;
        }

        attackedDmg = dmg;

        target.SetHp(dmg);

    }

    public void effectCoolTime()
    {
        for(int i = effects.Count - 1; i >= 0; i--)
        {
            effects[i].duration -= 1;
            if(effects[i].duration <= 0)
            {
                effects.RemoveAt(i);
            }
        }
    }

    public bool IsStun()
    {
        foreach(effect i in effects)
        {
            if(i.type == effectType.stun) {  return true; }
        }
        return false;
    }

    public float GetBuff(effectType type)
    {
        float sum = 0;
        for(int i = 0; i < effects.Count; i++)
        {
            if(effects[i].type == type)
            {
                sum += effects[i].value;
            }
        }
        return sum;
    }

    public float CurculateStr()
    {
        float sum = str;
        if(EquipedWeapon != null || EquipedWeapon.Length > 0)
        {
            sum += EquipmentData.equipments[EquipedWeapon].atk;
        }
         sum *= GetBuff(effectType.strUp);

        return sum;
    }

    public float Curculatedef()
    {

        float sum = def;
        if (EquipedWeapon != null || EquipedWeapon.Length > 0)
        {
            sum += EquipmentData.equipments[EquipedWeapon].def;
        }
        sum *= GetBuff(effectType.defUp);
        return sum;
    }

    public float Curculatecrit()
    {

        float sum = crit;
        if (EquipedWeapon != null || EquipedWeapon.Length > 0)
        {
            sum += EquipmentData.equipments[EquipedWeapon].crit;
        }
        sum += GetBuff(effectType.strUp);
        return sum;

    }

    public float Curculateavd()
    {
        return avd * GetBuff(effectType.avdUp);
    }

    public float CulcurlateDmg(float dmg)
    {
        return dmg + dmg * GetBuff(effectType.skillDmgUp);
    }

    public void UseSkill(string skillName, Unit attacker, Unit target, List<Unit> units)
    {
        Skill skill = SkillData.skills[skillName];

        if(curMp >= skill.mp)
        {
            curMp -= skill.mp;
        }
        else
        {
            Debug.Log("스킬 사용 불가");
            return;
        }

        if (cooltime.ContainsKey(skillName))
        {
            cooltime[skillName] = skill.coolTime;
        }
        else
        {
            cooltime.Add(skillName, skill.coolTime);
        }

        switch (skill.name)
        {
            case "베기":
                Attack(attacker, target, 1.7f, true);
                break;
            case "가르기":
                foreach (Unit unit in FindUnit(target, units))
                {
                    Attack(attacker, unit, 1.4f, true);
                }

                break;
            case "노려보기":
                attacker.effects.Add(new effect(effectType.critUp, 25, 3));
                break;
            case "명상":
                attacker.Heal(maxHp * 0.3f);
                break;
            case "필살기":
                foreach (Unit unit in units)
                {
                    Attack(attacker, unit, 3f, true);
                }
                break;
            case "가드":
                attacker.effects.Add(new effect(effectType.defUp, 0.3f, 2));
                break;
            case "기사회생":
                float val = 1.5f + (maxHp - curHp) / maxHp;
                Attack(attacker,target,val, true);
                break;
            case "약점격파":
                Attack(attacker,target,1.3f,true);
                target.effects.Add(new effect(effectType.defUp, -0.3f, 3));
                break;
            case "화염구":
                foreach (Unit unit in FindUnit(target, units))
                {
                    Attack(attacker, unit, 1.8f, true);
                }
                break;
            case "급습":
                if (target.isDefence)
                {
                    target.isDefence = false;
                    Attack(attacker, target, 2.5f, true);
                }
                else
                {
                    Attack(attacker, target, 1.5f, true);
                }
                break;
            case "최후의 일격":
                if (target.curHp / target.maxHp <= 0.3f)
                {
                    Attack(attacker, target, 3f, true);
                }
                else
                {
                    Attack(attacker, target, 1.7f, true);
                }
                break;
            case "공방일체":
                foreach(Unit unit in FindUnit(target, units))
                {
                    Attack(attacker, unit, 1f, true);
                }
                attacker.effects.Add(new effect(effectType.defUp, 0.3f, 3));
                break;
            case "두 개의 심장":
                actionCount += 2;
                break;
            case "내려찍기":
                Attack(attacker, target, 1.8f, true);
                break;
            case "혼란의 일격":
                Attack(attacker, target, 1.2f, true);
                target.effects.Add(new effect(effectType.canNotUseSkill, 0, 1));
                break;
            case "혼신의 일격":
                Attack(attacker, target, 2f, true);
                break;
            case "암흑":
                Attack(attacker, target, 1.2f, true);
                target.effects.Add(new effect(effectType.stun, 0, 1));
                break;
            case "영혼 흡수":
                Attack(attacker, target, 1.7f, true);
                attacker.Heal(attackedDmg / 2f);
                break;
            case "파괴광선":
                Attack(attacker, target, 3f, true);
                target.effects.Add(new effect(effectType.stun, 0, 3));
                break;

        }


        switch(skillName)
        {
            case "두 개의 심장":
                break;
            default:
                actionCount -= 1;
                break;

        }
    }

    public List<Unit> FindUnit(Unit target, List<Unit> targets)
    {
        List<Unit> result = new List<Unit>();

        int num = targets.IndexOf(target);

        result.Add(target);
        if(num - 1 >= 0)
        {
            result.Add(targets[num - 1]);
        }
        if(num+1 <= targets.Count - 1)
        {
            result.Add(targets[num + 1]);
        }

        return result;

    }


}
