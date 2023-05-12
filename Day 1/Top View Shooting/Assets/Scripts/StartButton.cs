using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    private bool start = false;
    private float time = 0;

    public void Connect()
    {
        bool connect = CSocket.Instance.Connect();
        if (connect)
        {
            bool join = CSocket.Instance.Join("name", "FFFF00");
            if (join)
            {
                CSocket.Instance.Run();
                start = true;
            }
        }
    }

    private void Update()
    {
        if (start)
        {
            time += Time.deltaTime;

            // emit
            CSocket.Instance.EmitEvent(new EmitEvent_Update(time, time, time));

            // on
            OnEvent evt;
            while ((evt = CSocket.Instance.Dequeue_on()) != null)
                Debug.Log(JsonUtility.ToJson(evt));
        }
    }
}