using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;
    public Unit turnUnit;

    public Player player;
    public List<Enemy> mobList = new();

    List<Unit> units = new List<Unit>();

    public List<GameObject> mobSpawnPoint;

    private void Awake()
    {
        instance = this;
    }


    public void StartBattle(List<Enemy> enemyList)
    {
        mobList = enemyList;
        StartPhase();
    }

    public void StartPhase()
    {
        units.Clear();
        foreach (Enemy enemy in mobList)
        {
            if(enemy.isDead) continue;
            enemy.SelectAct();
            if (enemy.act == ActStatus.Defence)
            {
                enemy.spd += 99;
                //방어하겠다를 알려주는 함수 추가
                enemy.shildUI.SetActive(true);
            }
            if(enemy.act == ActStatus.UseSkill && enemy.SelectedSkill == "파괴광선")
            {
                //파괴광선 쓴다는 문구 추가
            }
            units.Add(enemy);
        }
        units.Add(player);

        units = units.OrderByDescending(unit => unit.spd).ToList();

        NextTurn();
        
    }

    public void NextTurn()
    {
        if(turnUnit == null)
        {
            turnUnit = units[0];
        }
        else
        {
            if (units.IndexOf(turnUnit) + 1 >= units.Count)
            {
                turnUnit = units[0];
                NextPhase();
                return;
            }
            else
            {
                turnUnit = units[units.IndexOf(turnUnit) + 1];
            }
          
        }
        StartTurn();
    }

    public void StartTurn()
    {
        if (turnUnit.isDead) { NextTurn(); return; }
        if (turnUnit.IsStun()) { NextTurn(); return; }

        if (turnUnit.gameObject.GetComponent<Enemy>())
        {
            turnUnit.gameObject.GetComponent<Enemy>().StartAct();
        }
        else if (turnUnit.gameObject.GetComponent<Player>())
        {
            //나중에 채워넣기
        }
    }

    public void NextPhase()
    {
        foreach(Unit unit in units)
        {
            unit.effectCoolTime();
            unit.SkillCoolTime();
        }


        foreach(Enemy enemy in mobList)
        {
            if(enemy.isDead) { continue; }
            enemy.SelectAct();
            if(enemy.isDefence)
            {
                enemy.spd -= 99;
            }
            if(enemy.act == ActStatus.Defence)
            {
                enemy.spd += 99;
                enemy.shildUI.SetActive(true);
            }
            if (enemy.act == ActStatus.UseSkill && enemy.SelectedSkill == "파괴광선")
            {
                //파괴광선 쓴다는 문구 추가
            }
        }

        units = units.OrderByDescending(unit => unit.spd).ToList();

        StartTurn();
    }
}
