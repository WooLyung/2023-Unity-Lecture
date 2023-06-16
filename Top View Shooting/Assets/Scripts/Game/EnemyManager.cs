using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static OnEvent_Init;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;

    public Dictionary<int, Enemy> enemies = new Dictionary<int, Enemy>();

    public void InitEvent(OnEvent_Init evt)
    {
        foreach (var other in evt.others)
        {
            if (evt.id == other.id)
                continue;

            GameObject newObj = Instantiate(enemy);
            Enemy comp = newObj.GetComponent<Enemy>();
            comp.Init(other);
            enemies.Add(other.id, comp);
        }
    }

    public void JoinEvent(OnEvent_Join evt)
    {
        GameObject newObj = Instantiate(enemy);
        Enemy comp = newObj.GetComponent<Enemy>();
        comp.Init(evt);
        enemies.Add(evt.id, comp);
    }

    public void LeaveEvent(OnEvent_Leave evt)
    {
        Enemy enemy;
        if (enemies.TryGetValue(evt.id, out enemy))
            enemy.Destroy();
    }

    public void DeathEvent(OnEvent_Death evt)
    {
        Enemy enemy;
        if (enemies.TryGetValue(evt.victim, out enemy))
            enemy.Destroy();
    }

    public void UpdateEvent(OnEvent_Update evt)
    {
        foreach (var player in evt.players)
        {
            Enemy enemy;
            if (enemies.TryGetValue(player.id, out enemy))
                enemy.UpdateEvent(player);
        }
    }
}
