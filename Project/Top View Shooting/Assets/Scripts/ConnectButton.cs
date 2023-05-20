using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectButton : MonoBehaviour
{
    private bool start = false;

    public void Connect()
    {
        if (CSocket.Instance.Connect())
        {
            bool result = CSocket.Instance.Join("nickname!!", "FFFF00");
            if (result)
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
            // emit
            CSocket.Instance.EmitEvent(new EmitEvent_Update(127.0f, 127.0f, 127.0f));

            // on
            OnEvent evt;
            while ((evt = CSocket.Instance.Dequeue_on()) != null)
                Debug.Log(JsonUtility.ToJson(evt));
        }
    }
}