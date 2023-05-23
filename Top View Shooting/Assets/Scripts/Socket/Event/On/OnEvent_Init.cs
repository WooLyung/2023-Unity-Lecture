using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[Serializable]
public class OnEvent_Init : OnEvent
{
    public class Map
    {
        public int type;
        public int x;
        public int y;

        public Map(byte[] buffer, int offset)
        {
            type = ByteUtil.ToInt(buffer, offset);
            x = ByteUtil.ToInt(buffer, offset + 4);
            y = ByteUtil.ToInt(buffer, offset + 8);
        }
    }

    public class Other
    {
        public int id;
        public string nickname;
        public string color;
        public float x;
        public float y;
        public float angle;
        public int hp;

        public Other(byte[] buffer, int offset)
        {
            try
            {
                id = ByteUtil.ToInt(buffer, offset);
                nickname = ByteUtil.ToString(buffer, offset + 4, 16);
                color = ByteUtil.ToString(buffer, offset + 20, 6);
                x = ByteUtil.ToFloat(buffer, offset + 26);
                y = ByteUtil.ToFloat(buffer, offset + 30);
                angle = ByteUtil.ToFloat(buffer, offset + 34);
                hp = ByteUtil.ToInt(buffer, offset + 38);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log("!!!!!");
            }
        }
    }

    public int id;
    public string nickname;
    public string color;
    public float x;
    public float y;
    public int hp;
    public List<Map> map;
    public List<Other> others;

    public OnEvent_Init(byte[] buffer)
    {
        id = ByteUtil.ToInt(buffer, 0);
        nickname = ByteUtil.ToString(buffer, 4, 16);
        color = ByteUtil.ToString(buffer, 20, 6);
        x = ByteUtil.ToFloat(buffer, 26);
        y = ByteUtil.ToFloat(buffer, 30);
        hp = ByteUtil.ToInt(buffer, 34);

        int mapSize = ByteUtil.ToInt(buffer, 38);
        map = new List<Map>();
        for (int i = 0; i < mapSize; i++)
            map.Add(new Map(buffer, 42 + i * 12));

        int playerSize = ByteUtil.ToInt(buffer, 42 + mapSize * 12);
        others = new List<Other>();
        for (int i = 0; i < playerSize; i++)
            others.Add(new Other(buffer, 46 + mapSize * 12 + i * 42));
    }
}