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

        [Header("Listening On")]
        public QuestEventChannelSO _OnAcceptQuest;
        public QuestEventChannelSO _OnGiveupQuest;
        public QuestEventChannelSO _OnCompletedQuest;        

        [Header("Broadcasting On")]
        public QuestEventChannelSO _OnSetCurrentQuest;
        public QuestEventChannelSO _OnRewardQuest;
        #endregion

        #region Unity Event Method
        protected override void OnEnable()
        {
            base.OnEnable();

            _OnAcceptQuest.OnEventRaised += AcceptQuest;
            _OnGiveupQuest.OnEventRaised += GiveupQuest;
            _OnCompletedQuest.OnEventRaised += CompletedQuest;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _OnAcceptQuest.OnEventRaised -= AcceptQuest;
            _OnGiveupQuest.OnEventRaised -= GiveupQuest;
            _OnCompletedQuest.OnEventRaised -= CompletedQuest;
        }

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

            //퀘스트 리스트의 맨 앞에 있는 퀘스트를 가져와서 UI CurrentQuest 셋팅
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
                    //보상
                    RewardQuest();
                    break;
            }
        }

        public void RewardQuest()
        {
            //보상 주기
            _OnRewardQuest.RaisedEvent(quests[0]);

            //보상준 퀘스트 제거
            quests.Remove(quests[0]);
        }

        //수락 상태로 바꾸기
        public void AcceptQuest(Quest quest)
        {
            foreach (var q in quests)
            {
                if(quest.number == q.number)
                {
                    q.qusetState = QusetState.Accept;
                }
            }
        }

        //대기 상태로 바꾸기
        public void GiveupQuest(Quest quest)
        {
            foreach (var q in quests)
            {
                if (quest.number == q.number)
                {
                    q.qusetState = QusetState.Ready;
                }
            }
        }

        //완료 상태로 바꾸기
        public void CompletedQuest(Quest quest)
        {
            foreach (var q in quests)
            {
                if (quest.number == q.number)
                {
                    q.qusetState = QusetState.Complete;
                }
            }
        }
        #endregion
    }
}