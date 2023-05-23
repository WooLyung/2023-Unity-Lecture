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

    private float time = 0;

    private void Update()
    {
        time += Time.deltaTime;

        if (time >= 0.15f)
        {
            time -= 0.15f;
            Transform pt = player.transform;
            CSocket.Instance.EmitEvent("update", new EmitEvent_Update(pt.position.x, pt.position.y, pt.eulerAngles.z));
        }
    }

    [SerializeField]
    private GameObject prefab;

    public void UpdateEvent(OnEvent_Update evt)
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
            Destroy(obj);

        foreach (var other in evt.others)
        {
            GameObject enemy = Instantiate(prefab);
            enemy.transform.position = new Vector3(other.x, other.y);
            enemy.transform.rotation = Quaternion.Euler(0, 0, other.angle);
        }
    }
}