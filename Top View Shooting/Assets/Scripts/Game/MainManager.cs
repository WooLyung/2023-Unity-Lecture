using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private EnemyManager enemyManager;

    public void Init(OnEvent_Init evt)
    {
        player.Init(evt.id, evt.nickname, evt.color, evt.x, evt.y);
        enemyManager.InitEvent(evt);
    }

    private float time = 0;

    private void Update()
    {
        time += Time.deltaTime;

        if (time >= 0.02f)
        {
            time -= 0.02f;
            Transform pt = player.transform;
            CSocket.Instance.EmitEvent(new EmitEvent_Update(pt.position.x, pt.position.y, pt.eulerAngles.z));
        }
    }

    public void UpdateEvent(OnEvent_Update evt)
    {
        enemyManager.UpdateEvent(evt);
        player.UpdateEvent(evt);
    }

    public void JoinEvent(OnEvent_Join evt)
    {
        enemyManager.JoinEvent(evt);
    }

    public void LeaveEvent(OnEvent_Leave evt)
    {
        enemyManager.LeaveEvent(evt);
    }

    public void DeathEvent(OnEvent_Death evt)
    {
        enemyManager.DeathEvent(evt);
    }
}