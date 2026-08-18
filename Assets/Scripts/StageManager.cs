using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;
    public List<StageData> stages = new List<StageData>();
    public List<Button> stageButton = new List<Button>();

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int num;
        for (int i = 0; i < stages.Count; i++)
        {
            num = i;

            stageButton[num].onClick.AddListener(() => StageManager.instance.FindStage(stages[num].stageValue));
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < stages.Count;i++)
        {
            if(i <= GameManager.instance.clearedStage)
            {
                stageButton[i].interactable = true;
            }
            else
            {
                stageButton[i].interactable=false;
            }
        }
    }

    public void FindStage(string name)
    {
        foreach(StageData data in stages)
        {
            if(data.stageValue == name)
            {
                LoadStage(data.enemys);
            }
        }
    }


    void LoadStage(List<Enemy> enemys)
    {
        TurnManager.instance.StartBattle(enemys);
    }
}
