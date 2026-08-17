using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Player player;
    public RectTransform BaseSkill;
    public RectTransform SkillUI;
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
        
    }


    public void UpdateSkillUI()
    { 
        for(int i = 0; i < SkillUI.childCount; i++)
        {
            Destroy(SkillUI.GetChild(i).gameObject);
        }

        foreach(string skillName in player.haveSkills)
        {
            RectTransform button = Instantiate(BaseSkill,SkillUI.transform);
            button.GetComponent<Button>().onClick.AddListener(() => player.SkillSelect(skillName)); 
            button.GetComponent<Text>().text = skillName;
        }
    }

}
