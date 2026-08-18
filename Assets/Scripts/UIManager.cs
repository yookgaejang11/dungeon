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

        }

    }

}
