using UnityEngine;

public enum ActStatus
{
    Attack = 0,
    Defence,
    UseSkill
}

public class Enemy : Unit
{
    public ActStatus act;
    public string SelectedSkill;

    public GameObject shildUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectAct()
    {
        act = (ActStatus)Random.Range(0, 3);

        while( act == ActStatus.UseSkill && haveSkills.Count <= 0)
        {
            act = (ActStatus)Random.Range(0, 3);
        }

        if(act == ActStatus.UseSkill)
        {
            SelectedSkill = haveSkills[Random.Range(0, haveSkills.Count)];
        }
    }

    public void StartAct()
    {
        if(act == ActStatus.Defence)
        {
            isDefence = true;
        }
        else if(act == ActStatus.UseSkill)
        {

        }
        else
        {
            Attack(this, GameObject.FindAnyObjectByType<Player>(), CurculateStr());
        }

        actionCount -= 1;
    }
}
