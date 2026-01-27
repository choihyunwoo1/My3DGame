using System;

namespace My3DGame
{
    /// <summary>
    /// 퀘스트 데이터 정의 클래스
    /// </summary>
    [Serializable]
    public class QuestData
    {
        public int number;              //퀘스트 인덱스
        public int npcNumber;           //퀘스트를 가지고 있는 Npc 번호
        public string name;             //퀘스트 이름
        public string description;      //퀘스트 내용
        public int startDialog;         //퀘스트 대화 인덱스 - 의뢰
        public int ingDialog;           //퀘스트 대화 인덱스 - 진행중
        public int endDialog;           //퀘스트 대화 인덱스 - 완료
        public int level;               //퀘스트 레벨 제한
        public QuestType questType;     //퀘스트 타입
        public int goalIndex;           //퀘스트 타입에 따른 목표 아이템 아이디, enemy 아이디...
        public int goalAmount;          //퀘스트 목표 수량
        public int rewardGold;          //완료 보상 골드
        public int rewardExp;           //완료 보상 경험치
        public int reawrdItem;          //보상 아이템 아이디
    }
}