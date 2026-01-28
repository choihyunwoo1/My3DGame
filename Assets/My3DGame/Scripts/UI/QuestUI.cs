using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace My3DGame.UI
{
    /// <summary>
    /// 퀘스트 정보창을 관리하는 클래스
    /// </summary>
    public class QuestUI : MonoBehaviour
    {
        #region Variables
        public QuestSO questObejct;     //퀘스트 데이터 베이스
        public ItemDataBaseSO itemDataBase; //아이템 데이터 베이스

        //UI
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI descriptionText;

        public TextMeshProUGUI goalAmountText;
        public TextMeshProUGUI rewardGoldText;
        public TextMeshProUGUI rewardExpText;
        public TextMeshProUGUI rewardItemText;
        public Image itemImage;

        //button
        public GameObject acceptButton;
        public GameObject giveupButton;
        public GameObject okButton;

        //현재 UI에서 보여지는 Quest
        [SerializeField]
        protected Quest currentQuest = null;

        [Header("Listening On")]
        public QuestEventChannelSO _OnSetCurrentQuest;

        [Header("Broadcasting On")]
        public VoidEventChannelSO _ToggleQuestUIEvent;
        public QuestEventChannelSO _OnAcceptQuest;
        public QuestEventChannelSO _OnGiveupQuest;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //이벤트 함수 등록
            _OnSetCurrentQuest.OnEventRaised += SetCurrentQuest;
        }
        #endregion

        #region Custom Method
        public void SetCurrentQuest(Quest quest)
        {
            Debug.Log("set quest.number:" + quest.number);
            currentQuest = quest;
        }

        //매개변수로 받은 퀘스트로 UI 셋팅
        private void SetQuestUI(Quest quest)
        {
            Debug.Log("ui quest.number:" + quest.number);
            QuestData questData = questObejct.database[quest.number];

            nameText.text = questData.name;
            if(quest.questGoal.IsReached)
            {
                descriptionText.text = "Quest Completed!!!";
                goalAmountText.text = quest.questGoal.goalAmount.ToString() + " / "
                    + quest.questGoal.goalAmount.ToString();
            }
            else
            {
                descriptionText.text = questData.description;
                goalAmountText.text = quest.questGoal.currentAmount.ToString() + " / "
                    + quest.questGoal.goalAmount.ToString();
            }
            
            rewardGoldText.text = questData.rewardGold.ToString();
            rewardExpText.text = questData.rewardExp.ToString();

            //보상 아이템
            if(questData.reawrdItem >= 0)
            {
                rewardItemText.text = itemDataBase.itemObjects[questData.reawrdItem].name;
                itemImage.sprite = itemDataBase.itemObjects[questData.reawrdItem].icon;
                itemImage.enabled = true;
            }
            else
            {
                rewardItemText.text = string.Empty;
                itemImage.sprite = null;
                itemImage.enabled = false;
            }

            //버튼 셋팅
            ResetButton();
            switch(quest.qusetState)
            {
                case QusetState.Ready:
                    acceptButton.SetActive(true);
                    break;
                case QusetState.Accept:
                    giveupButton.SetActive(true);
                    break;
                case QusetState.Complete:
                    okButton.SetActive(true);
                    break;
            }
        }

        private void ResetButton()
        {
            acceptButton.SetActive(false);
            giveupButton.SetActive(false);
            okButton.SetActive(false);
        }

        //퀘스트 UI 열기
        public void OpenQuestUI()
        {
            if (currentQuest == null)
                return;

            SetQuestUI(currentQuest);
        }

        //퀘스트 UI 초기화
        public void CloseQuestUI()
        {
            currentQuest = null;
            ResetButton();
        }

        //퀘스트 UI 닫기
        public void Close()
        {
            _ToggleQuestUIEvent.RaisedEvent();
        }

        //수락 버튼 클릭시
        public void AcceptQuest()
        {
            //선택된 퀘스트를 플레어 퀘스트에 추가한다
            //npc 퀘스트의 상태를 수락상태로 바꾼다
            if(currentQuest != null)
            {
                _OnAcceptQuest.RaisedEvent(currentQuest);
            }
            Close();
        }

        //포기 버튼
        public void GiveupQuest()
        {
            //선택된 퀘스트를 플레어 퀘스트에 추가한다
            //npc 퀘스트의 상태를 수락상태로 바꾼다
            if (currentQuest != null)
            {
                _OnGiveupQuest.RaisedEvent(currentQuest);
            }
            Close();
        }
        #endregion
    }
}