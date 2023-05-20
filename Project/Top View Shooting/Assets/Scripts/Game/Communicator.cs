using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Communicator : MonoBehaviour
{
    [SerializeField]
    private MainManager main;
    private bool start = false;

    void Start()
    {
        OnEvent_Init data = CSocket.Instance.Init();
        main.Init(data);
        start = true;
        CSocket.Instance.Run();
    }

    void Update()
    {
        if (start)
        {
            OnEvent evt;
            while ((evt = CSocket.Instance.Dequeue_on()) != null)
            {
                // do something
            }
        }
    }
}
