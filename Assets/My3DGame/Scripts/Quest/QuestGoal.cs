using System;

namespace My3DGame
{
    /// <summary>
    /// 퀘스트 목표를 관리하는 클래스
    /// </summary>
    [Serializable]
    public class QuestGoal
    {
        public QuestType questType;     //퀘스트 타입
        public int goalIndex;           //퀘스트 타입에 따른 목표 아이템 아이디, enemy 아이디...
        public int goalAmount;          //퀘스트 목표 수량
        public int currentAmount;       //퀘스트 현재 달성량

        public bool IsReached => (currentAmount >= goalAmount);     //퀘스트 달성 여부
    }
}
