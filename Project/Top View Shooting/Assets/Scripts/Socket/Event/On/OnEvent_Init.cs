using System;

[Serializable]
public class OnEvent_Init : OnEvent
{
    [Serializable]
    public class Map
    {
        public string type;
        public int x;
        public int y;
    }

    [Serializable]
    public class Other
    {
        public int id;
        public string nickname;
        public string color;
        public float x;
        public float y;
        public float angle;
        public int hp;
    }

    public int id;
    public string nickname;
    public string color;
    public float x;
    public float y;
    public int hp;
    public Map[] map;
    public Other[] others;
}
