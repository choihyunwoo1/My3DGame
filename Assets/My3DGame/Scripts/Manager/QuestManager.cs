using My3DGame.UI;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 플레이어가 진행하는 퀘스트 목록을 관리하는 클래스
    /// </summary>
    public class QuestManager : MonoBehaviour
    {
        #region Variables
        public QuestSO questObejct;             //퀘스트 데이터 베이스
        public StatsSO statsObject;             //캐릭터 스탯 오브젝트

        public List<Quest> playerQuests;        //플레이어가 진행중 퀘스트 목록
        //protected Quest currentQuest;           //현재 선택된 퀘스트

        [Header("Listening On")]
        public VoidEventChannelSO _OpenQuestUIEvent;
        public QuestEventChannelSO _OnAcceptQuest;
        public QuestEventChannelSO _OnGiveupQuest;
        public UpdateQuestEventChannelSO _OnUpdateQuest;
        public QuestEventChannelSO _OnRewardQuest;

        [Header("Broadcasting On")]
        public VoidEventChannelSO _ToggleQuestUIEvent;
        public QuestEventChannelSO _OnSetCurrentQuest;
        public QuestEventChannelSO _OnCompletedQuest;
        #endregion

        #region Unity Event Method
        private void OnEnable()
        {
            _OpenQuestUIEvent.OnEventRaised += OpenPlayerQuestUI;
            _OnAcceptQuest.OnEventRaised += AddPlayerQuest;
            _OnGiveupQuest.OnEventRaised += RemovePlayerQuest;
            _OnUpdateQuest.OnEventRaised += UpdateQuest;
            _OnRewardQuest.OnEventRaised += RewardQuest;
        }

        private void OnDisable()
        {
            _OpenQuestUIEvent.OnEventRaised -= OpenPlayerQuestUI;
            _OnAcceptQuest.OnEventRaised -= AddPlayerQuest;
            _OnGiveupQuest.OnEventRaised -= RemovePlayerQuest;
            _OnUpdateQuest.OnEventRaised -= UpdateQuest;
            _OnRewardQuest.OnEventRaised -= RewardQuest;
        }

        private void Start()
        {
            //초기화 - 플레이어 퀘스트 셋팅
            playerQuests = new List<Quest>();
        }
        #endregion

        #region Custom Method
        //
        public void OpenPlayerQuestUI()
        {
            if (playerQuests.Count <= 0)
                return;

            //플레이어 퀘스트 리스트의 맨 앞에 있는 퀘스트를 가져와서 UI CurrentQuest 셋팅
            _OnSetCurrentQuest.RaisedEvent(playerQuests[0]);

            //UI 창을 연다 - 완료 상태일때 보상 주면 안된다
            _ToggleQuestUIEvent.RaisedEvent();
        }

        //플레이어 퀘스트 목록에 퀘스트 추가
        public void AddPlayerQuest(Quest quest)
        {
            if (quest == null)
                return;

            QuestData questData = questObejct.database[quest.number];
            Quest newQuest = new Quest(questData);
            newQuest.qusetState = QusetState.Accept;

            playerQuests.Add(newQuest);
        }

        //플레이어 퀘스트 목록에서 퀘스트 제거
        public void RemovePlayerQuest(Quest quest)
        {
            if (quest == null)
                return;

            playerQuests.Remove(quest);
        }

        //플레이어 퀘스트 업데이트
        public void UpdateQuest(QuestType questType, int goalIndex)
        {
            switch(questType)
            {
                case QuestType.Kill:
                    foreach (var quest in playerQuests)
                    {
                        quest.EnemyKill(goalIndex);

                        //퀘스트 달성 체크
                        if(quest.questGoal.IsReached)
                        {
                            quest.qusetState = QusetState.Complete;
                            _OnCompletedQuest.RaisedEvent(quest);
                        }
                    }
                    break;

                case QuestType.Collect:
                    foreach (var quest in playerQuests)
                    {
                        quest.ItemCollect(goalIndex);

                        //퀘스트 달성 체크
                        if (quest.questGoal.IsReached)
                        {
                            quest.qusetState = QusetState.Complete;
                            _OnCompletedQuest.RaisedEvent(quest);
                        }
                    }
                    break;
            }
        }

        public void RewardQuest(Quest quest)
        {
            if(quest == null)
                return;

            //보상 처리
            Debug.Log("퀘스트 보상 처리");
            QuestData questData = questObejct.database[quest.number];
            statsObject.AddGold(questData.rewardGold);
            statsObject.AddExp(questData.rewardExp);
            if(questData.reawrdItem >= 0)
            {
                Debug.Log("아이템 지급");
            }

            //플레이어 퀘스트 리스트에서 제거
            foreach (var q in playerQuests)
            {
                if (quest.number == q.number)
                {
                    playerQuests.Remove(q);
                    break;
                }
            }
        }
        #endregion
    }
}