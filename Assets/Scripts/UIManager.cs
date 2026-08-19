using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Player player;
    public RectTransform BaseSkill;
    public RectTransform SkillUI;
    public Text statusTxt;
    public Text DescText;
    public GameObject basePotion;
    public RectTransform invenUI;
    public Text potionDescTxt;
    public Text effectText;
    private void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        statusTxt.text = "Str: " + player.str + "->" + player.CurculateStr() +
            "  Def: " + player.def + "->" + player.Curculatedef() + "\nSpd: " + player.spd + player.spd + "  crit: "
            + player.crit + "->" + player.Curculatecrit() + "\nAvd: " + player.avd + "->" + player.Curculateavd();
        if(player.selectedSkill.Length >0)
        {
            DescText.text = SkillData.skills[player.selectedSkill].desc;
        }
        if(player.selectedItem.Length > 0)
        {
            potionDescTxt.text = ItemData.items[player.selectedItem].desc.ToString();
        }

        effectText.text = UpdateEffect();
    }

    public string UpdateEffect()
    {
        string text = "";

        foreach(effect effect in player.effects)
        {
            switch(effect.type)
            {
                case effectType.strUp:
                    text += " str + " + effect.value + "%";
                    break;
                case effectType.defUp:
                    text += " def + " + effect.value + "%";
                    break;
                case effectType.critUp:
                    text += " crit + " + effect.value + "%";
                    break;
                case effectType.avdUp:
                    text += " avd * " + effect.value;
                    break;
                case effectType.skillDmgUp:
                    text += " skillDmg + " + effect.value + "%";
                    break;
                case effectType.stun:
                    text += " stun";
                    break;
                case effectType.canNotUseSkill:
                    text += " 스킬 사용 불가";
                    break;
            }
            text += "(" + effect.duration + ")";
        }

        return text;
    }


    /// <summary>
    /// 전투중
    /// </summary>
    public void UpdateInvenUI()
    {
        for(int i = 0; i < invenUI.childCount; i++)
        {
            Destroy(invenUI.GetChild(i).gameObject);
        }


        var invens = player.invenValue.Keys;

        GameObject potionUI;

        foreach( var key in invens )
        {
            if (ItemData.items.ContainsKey(key))
            {
                potionUI = Instantiate(basePotion, invenUI.transform);
                potionUI.GetComponent<Image>().sprite = Resources.Load<Sprite>("potions/" + key);
                potionUI.transform.GetChild(0).GetComponent<Text>().text = player.invenValue[key].ToString();
                potionUI.GetComponent<Button>().onClick.AddListener(() =>player.UsePotion(key));
            }
        }

    }
    public void UpdateSkillUI()
    { 
        for(int i = 0; i < SkillUI.childCount; i++)
        {
            Destroy(SkillUI.GetChild(i).gameObject);
        }
        for (int i = 0; i < player.haveSkills.Count; i++)
        {
            int num = i;
            RectTransform button = Instantiate(BaseSkill, SkillUI.transform);
            
            button.GetComponent<Button>().onClick.AddListener(() => player.SkillSelect(player.haveSkills[num]));
            button.GetComponent<Text>().text = player.haveSkills[num];
            if (player.cooltime.ContainsKey(player.haveSkills[num]))
            {
                button.GetComponent<Button>().interactable = false;
                button.transform.GetChild(0).gameObject.SetActive(true);
                button.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = player.cooltime[player.haveSkills[num]].ToString();
            }

        }

    }

}
