using System.Collections.Generic;

public class OnEvent_Update : OnEvent
{
    public class Player
    {
        public int id;
        public float x;
        public float y;
        public float angle;

        public Player(byte[] buffer, int offset)
        {
            id = ByteUtil.ToInt(buffer, offset);
            x = ByteUtil.ToFloat(buffer, offset + 4);
            y = ByteUtil.ToFloat(buffer, offset + 8);
            angle = ByteUtil.ToFloat(buffer, offset + 12);
        }
    }

    public List<Player> players;

    public OnEvent_Update(byte[] buffer)
    {
        int playersSize = ByteUtil.ToInt(buffer, 0);
        players = new List<Player>();
        for (int i = 0; i < playersSize; i++)
            players.Add(new Player(buffer, 4 + i * 16));
    }
}