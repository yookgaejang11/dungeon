using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    weapon,
    cloth
}
public enum effectType
{
    strUp, defUp, critUp, avdUp,
    skillDmgUp,
    stun,
    canNotUseSkill
}

public class effect
{
    public effectType type;
    public float value;
    public int duration;

    public effect(effectType type, float value, int duration)
    {
        this.type = type;
        this.value = value;
        this.duration = duration;
    }
}

public class Item
{
    public string name, desc;
    public float value;
    public int maxCount,price;

    public Item(string name, string desc, float value, int maxCount, int price)
    {
        this.name = name;
        this.desc = desc;
        this.value = value;
        this.maxCount = maxCount;
        this.price = price;
    }
}

public static class ItemData
{
    public static Dictionary<string, Item> items = new()
    {
        ["빨간 포션"] = new("빨간 포션", "캐릭터의 HP 20% 회복", 0.2f, 5, 20),
        ["파란 포션"] = new("파란 포션", "캐릭터의 mp 20% 회복", 0.2f, 5, 30),
        ["힘의 영약"] = new("힘의 영약", "5턴간 30% 공증", 0.3f, 1, 100),
        ["지식의 영약"] = new("지식의 영약", "5턴간 스킬피해 30% 증가", 0.3f, 1, 300),
        ["회피의 물약"] = new("회피의 물약", "회피율이 2배 증가한다", 2f, 1, 1000),
    };
}


public class Skill
{
    public string name, desc, target;
    public int mp, coolTime;

    public Skill(string name, int mp, int coolTime, string desc, string target)
    {
        this.name = name;
        this.desc = desc;
        this.target = target;
        this.mp = mp;
        this.coolTime = coolTime;
    }
}

public static class SkillData
{
    public static Dictionary<string, Skill> skills = new()
    {
        ["베기"] = new("베기", 30, 0, "지정 단일 대상 공격력 170% 피해", "단일"),
        ["가르기"] = new("가르기", 35, 0, "지정 단일 대상과 인접 대상 공격력 140% 피해", "인접"),
        ["노려보기"] = new("노려보기", 25, 0, "3턴간 크확 25% 증가", "본인"),
        ["명상"] = new("명상", 45, 5, "최대체력 30% 회복", "본인"),
        ["필살기"] = new("필살기", 100, 10, "모든 대상 공격력 300% 피해", "전체"),
        ["가드"] = new("가드", 30, 0, "2턴간 방어력 30% 증가", "본인"),
        ["기사회생"] = new("기사회생", 45, 0, "자신 잃은체력 비례 단일 대상 공격력 150%~250% 피해", "단일"),
        ["약점격파"] = new("약점격파", 55, 2, "지정 단일 대상 공격력 130% 피해 + 방깎 30% 감소", "단일"),
        ["화염구"] = new("화염구", 65, 0, "지정 단일+인접 대상 공격력 180% 피해", "인접"),
        ["급습"] = new("급습", 60, 2, "지정 단일 대상 공격력 150% 피해, 만약 대상이 방어 상태라면 방어 해제 후 250% 피해", "단일"),
        ["최후의 일격"] = new("최후의 일격", 70, 1, "지정 단일 대상 공격력 170% 피해, 대상 채력 30% 이하라면 300% 피해", "단일"),
        ["공방일체"] = new("공방일체", 100, 3, "지정 단일 대상과 인접 대상 공격력 100% 피해, 3턴간 방어력 30% 증가", "단일"),
        ["두 개의 심장"] = new("두 개의 심장", 0, 10, "이 턴 동안 총 두 번 행동 가능(이 스킬은 턴을 소모하지 않음)", "본인"),

        //1보스
        ["내려찍기"] = new("내려찍기", 0, 8, "공격력 180% 피해", "플레이어"),
        //2 보스
        ["혼란의 일격"] = new("혼란의 일격", 0, 5, "", "플레이어"),
        ["혼신의 일격"] = new("혼신의 일격", 0, 9, "", "플레이어"),
        //3보스
        ["암흑"] = new("암흑", 0, 7, "", "플레이어"),
        ["영혼 흡수"] = new("영혼 흡수", 0, 4, "", "플레이어"),
        ["파괴광선"] = new("파괴광선", 0, 10, "", "플레이어"),

    };
}

public class Equipment
{
    public string name,desc;
    public float atk, crit, def, price;
    public EquipmentType equipmentType;
    public Equipment(string name, string desc, float atk, float crit, float def, float price,EquipmentType type)
    {
        this.name = name;
        this.desc = desc;
        this.atk = atk;
        this.crit = crit;
        this.def = def;
        this.price = price;
        this.equipmentType = type;
    }
}

public static class EquipmentData
{
    public static Dictionary<string, Equipment> equipments = new()
    {
        ["롱소드"] = new("롱소드", "공10증가", 10, 0, 0, 0, EquipmentType.weapon),
        ["대검"] = new("대검", "공20증가", 20, 0, 0, 100, EquipmentType.weapon),
        ["단검"] = new("단검", "공5증가,크확+10%", 5, 10, 0, 200, EquipmentType.weapon),
        ["도끼"] = new("도끼", "공20증가,크확+20%", 20, 20, 0, 400, EquipmentType.weapon),
        ["망치"] = new("망치", "공40증가", 60, 0, 0, 500, EquipmentType.weapon),
        ["마스터 소드"] = new("마스터 소드,", "공 60 증가 크확 30%", 60, 30, 0, 1000, EquipmentType.weapon),
        ["천 갑옷"] = new("천 갑옷", "방어력 5 증가", 0, 0, 5, 0, EquipmentType.cloth),
        ["가죽 갑옷"] = new("가죽 갑옷", "방어력 10 증가", 0, 0, 10, 200, EquipmentType.cloth),
        ["사슬 갑옷"] = new("사슬 갑옷", "방어력 25 증가", 0, 0, 25, 300, EquipmentType.cloth),
        ["무쇠 갑옷"] = new("무쇠 갑옷", "방어력 35 증가", 0, 0, 35, 400, EquipmentType.cloth),
        ["풀 플레이트 아머"] = new("풀 플레이트 아머", "방 50 증가", 0, 0, 50, 1200, EquipmentType.cloth),
    };
}


public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
