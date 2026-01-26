using System;

namespace My3DGame
{
    /// <summary>
    /// NPC의 속성 정의
    /// </summary>
    [Serializable]
    public class Npc
    {
        public NpcType type;        //NPC 타입
        public int number;          //고유번호
        public string name;         //이름

    }
}
