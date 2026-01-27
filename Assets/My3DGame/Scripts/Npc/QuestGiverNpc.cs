using UnityEngine;
using System.Collections.Generic;

namespace My3DGame
{
    /// <summary>
    /// 퀘스트를 주는 Npc를 관리하는 클래스
    /// </summary>
    public class QuestGiverNpc : PickupNpc
    {
        #region Variables
        public QuestSO questObejct;         //퀘스트 데이터베이스

        public List<Quest> quests;          //Npc가 가지고 있는 퀘스트 리스트

        [Header("Broadcasting On")]
        public QuestEventChannelSO _OnSetCurrentQuest;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //Npc가 가질 퀘스트 리스트 가져오기
            quests = GetNpcQuest(npc.number);
        }
        #endregion

        #region Custom Method
        //Npc가 가질 퀘스트 리스트 가져오기
        private List<Quest> GetNpcQuest(int npcNumber)
        {
            List<Quest> questList = new List<Quest>();

            foreach (var questData in questObejct.database)
            {
                if (questData.npcNumber == npcNumber)
                {
                    Quest quest = new Quest(questData);
                    questList.Add(quest);
                }
            }

            return questList;
        }

        //인터랙티브 기능 구현 - 대화 시도
        protected override void DoAction()
        {
            //픽업 체크
            if (isPickup == false)
                return;

            //퀘스트 갯수 체크
            if(quests.Count <= 0)
            {
                //랜덤 대화 보여준다
                int randNum = Random.Range(0, 3);
                _StartDialogEvent.RaisedEvent(randNum);
                return;
            }

            //퀘스트 리스트의 맨 앞에 있는 퀘스트를 가져와서 CurrentQuest 셋팅
            _OnSetCurrentQuest.RaisedEvent(quests[0]);

            switch (quests[0].qusetState)
            {
                case QusetState.Ready:  //퀘스트 의뢰 대화
                    _StartDialogEvent.RaisedEvent(questObejct.database[quests[0].number].startDialog);
                    break;
                case QusetState.Accept: //퀘스트 진행 대화
                    _StartDialogEvent.RaisedEvent(questObejct.database[quests[0].number].ingDialog);
                    break;
                case QusetState.Complete:   //퀘스트 완료 대화
                    _StartDialogEvent.RaisedEvent(questObejct.database[quests[0].number].endDialog);
                    break;
            }
        }
        #endregion
    }
}