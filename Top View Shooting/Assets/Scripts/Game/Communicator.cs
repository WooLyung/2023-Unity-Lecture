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
                if (evt is OnEvent_Update)
                    main.UpdateEvent(evt as OnEvent_Update);
                if (evt is OnEvent_Join)
                    main.JoinEvent(evt as OnEvent_Join);
                if (evt is OnEvent_Leave)
                    main.LeaveEvent(evt as OnEvent_Leave);
            }
        }
    }
}
