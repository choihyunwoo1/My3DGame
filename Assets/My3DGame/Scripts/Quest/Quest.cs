using My3DGame.AI;
using System;

namespace My3DGame
{
    /// <summary>
    /// 퀘스트를 관리하는 클래스
    /// </summary>
    [Serializable]
    public class Quest
    {
        #region Variables
        public int number;                  //퀘스트 번호
        public QusetState qusetState;       //퀘스트 상태
        public QuestGoal questGoal;         //퀘스트 목표
        #endregion

        #region Constructor
        //생성자 - 매개변수로 퀘스트 데이터를 받는다        
        public Quest(QuestData data)
        {
            number = data.number;
            qusetState = QusetState.Ready;

            questGoal = new QuestGoal();
            questGoal.questType = data.questType;
            questGoal.goalIndex = data.goalIndex;
            questGoal.goalAmount = data.goalAmount;
            questGoal.currentAmount = 0;
        }
        #endregion

        #region Custom Method
        //퀘스트 미션 달성시 호출되는 함수 - kill
        public void EnemyKill(int enemyId)
        {
            //퀘스트 타입 체크
            if(questGoal.questType == QuestType.Kill)
            {
                //적 아이디 체크
                //if(questGoal.goalIndex == enemyId)
                {
                    questGoal.currentAmount++;
                }
            }
        }

        //퀘스트 미션 달성시 호출되는 함수 - Collect
        public void ItemCollect(int itemId)
        {
            //퀘스트 타입 체크
            if (questGoal.questType == QuestType.Collect)
            {
                //아이템 아이디 체크
                //if (questGoal.goalIndex == itemId)
                {
                    questGoal.currentAmount++;
                }
            }
        }
        #endregion
    }
}