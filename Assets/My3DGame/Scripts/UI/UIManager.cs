using UnityEngine;
using My3DGame;

namespace My3DGame.UI
{
    /// <summary>
    /// 게임 UI들을 관리하는 클래스
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        #region Variables
        //참조
        public InputReader inputReader;
        public ItemDataBaseSO itemDataBase;        

        //UI
        public GameObject backgroundUI;
        public DynamicInventoryUI playerInventoryUI;
        public StaticInventoryUI playerEquipmentUI;
        public DialogUI dialogUI;
        public ActionUI actionUI;
        public QuestUI questUI;

        [Header("Listening On")]
        public VoidEventChannelSO _ToggleInventoryUIEvent;
        public VoidEventChannelSO _ToggleEquipmentUIEevent;
        public DialogEventChannelSO _ToggleDialogUIEvent;
        public StringEventChannelSO _ToggleActionUIEvent;
        public VoidEventChannelSO _ToggleQuestUIEvent;
        #endregion

        #region Unity Event Method
        private void OnEnable()
        {
            _ToggleInventoryUIEvent.OnEventRaised += TogglePayerInventoryUI;
            _ToggleEquipmentUIEevent.OnEventRaised += ToggelPlayerEquipmentUI;
            _ToggleDialogUIEvent.OnEventRaised += ToggleDialogUI;
            _ToggleActionUIEvent.OnEventRaised += ToggleActionUI;
            _ToggleQuestUIEvent.OnEventRaised += ToggleQuestUI;
        }

        private void OnDisable()
        {
            _ToggleInventoryUIEvent.OnEventRaised -= TogglePayerInventoryUI;
            _ToggleEquipmentUIEevent.OnEventRaised -= ToggelPlayerEquipmentUI;
            _ToggleDialogUIEvent.OnEventRaised -= ToggleDialogUI;
            _ToggleActionUIEvent.OnEventRaised -= ToggleActionUI;
            _ToggleQuestUIEvent.OnEventRaised -= ToggleQuestUI;
        }

        private void Start()
        {
            //UI 초기화
            ToggleQuestUI();
        }
        #endregion

        #region Custom Method
        //매개변수로 받은 UI오브젝트 토글 오픈
        private void ToggleUI(GameObject uiGameObject)
        {
            uiGameObject.SetActive(!uiGameObject.activeSelf);

            if(IsUIOpen())
            {
                //Cursor.lockState = CursorLockMode.None;
                //Cursor.visible = true;
                inputReader.EnableMenuInput();
                backgroundUI.SetActive(true);

                Time.timeScale = 0f;
            }
            else
            {
                //Cursor.lockState = CursorLockMode.Locked;
                //Cursor.visible = false;
                inputReader.EnableGamePlayInput();
                backgroundUI.SetActive(false);

                Time.timeScale = 1f;
            }
        }

        //화면에 UI창이 오픈되었는지 체크
        private bool IsUIOpen()
        {
            bool isOpen = false;

            isOpen |= playerInventoryUI.gameObject.activeSelf;
            isOpen |= playerEquipmentUI.gameObject.activeSelf;
            isOpen |= dialogUI.gameObject.activeSelf;
            isOpen |= questUI.gameObject.activeSelf;

            return isOpen;
        }

        //인벤토리 UI 토글
        public void TogglePayerInventoryUI()
        {
            //퀘스트 UI 열려 있거나 UI를 못연다
            
            ToggleUI(playerInventoryUI.gameObject);
            //창이 닫힐때 이벤트 함수 호출
            if(playerInventoryUI.gameObject.activeSelf == false)
            {
                if(playerInventoryUI._OnCloseUIEvent != null)
                {
                    playerInventoryUI._OnCloseUIEvent.Invoke();
                }
            }
        }

        //장비 장착창 UI 토글
        public void ToggelPlayerEquipmentUI()
        {
            //퀘스트 UI 열려 있거나 UI를 못연다

            ToggleUI(playerEquipmentUI.gameObject);
            //창이 닫힐때 이벤트 함수 호출
            if (playerEquipmentUI.gameObject.activeSelf == false)
            {
                if (playerEquipmentUI._OnCloseUIEvent != null)
                {
                    playerEquipmentUI._OnCloseUIEvent.Invoke();
                }
            }
        }

        //인벤토리에 아이템 추가 - 치팅
        public void AddInventoryItem(int index)
        {
            Item newItem = itemDataBase.itemObjects[index].CreateItem();
            playerInventoryUI.AddInventoryItem(newItem, 1);
        }

        //대화창 열기 토글
        public void ToggleDialogUI(Dialog dialog)
        {
            if(dialog == null)
            {
                //대화창 닫기
                ToggleUI(dialogUI.gameObject);
                //퀘스트창 의뢰창 열기
                if(dialogUI._OnCloseUIEvent != null)
                {
                    dialogUI._OnCloseUIEvent.Invoke();
                    //등록된 함수를 모두 제거
                    dialogUI._OnCloseUIEvent = null;
                }
            }
            else
            {
                if (dialogUI.gameObject.activeSelf == false)
                {
                    ToggleUI(dialogUI.gameObject);
                    //퀘스트 의뢰 대화이면 퀘스트 오픈 함수 등록
                    if(dialog.type == DialogType.Quest)
                    {
                        dialogUI._OnCloseUIEvent += ToggleQuestUI;
                    }
                }
                dialogUI.SetDialogue(dialog);
            }
        }

        //퀘스트 UI 토글 기능
        public void ToggleQuestUI()
        {
            //인벤토리 UI 열려 있거나, 장착 UI 열려 있으면 UI를 못연다

            ToggleUI(questUI.gameObject);
            if(questUI.gameObject.activeSelf == true)
            {
                //창이 열리면 UI 셋팅
                questUI.OpenQuestUI();
            }
            else
            {
                questUI.CloseQuestUI();
            }
        }

        //액션 UI 토글 기능
        public void ToggleActionUI(string action)
        {
            if(action == string.Empty)
            {
                actionUI.gameObject.SetActive(false);
            }
            else
            {
                actionUI.gameObject.SetActive(true);
            }
            actionUI.SetActionUI(action);
        }
        #endregion
    }
}