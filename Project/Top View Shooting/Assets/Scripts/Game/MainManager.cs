using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    [SerializeField]
    private Player player;

    public void Init(OnEvent_Init evt)
    {
        player.Init(evt.id, evt.nickname, evt.color, evt.x, evt.y);
    }

    float time = 0;

    private void Update()
    {
        time += Time.deltaTime;
        if (time > 0.2f)
        {
            time -= 0.2f;
            Transform pt = player.transform;
            CSocket.Instance.EmitEvent(new EmitEvent_Update(pt.position.x, pt.position.y, pt.eulerAngles.z));
        }
    }
}
