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

        public GameObject backgroundUI;
        public DynamicInventoryUI playerInventoryUI;
        public StaticInventoryUI playerEquipmentUI;

        public DialogueManager dialogueManager;
        public DialogueUIManager dialogueUIManager;

        [Header("Listening On")]
        public VoidEventChannelSO _ToggleInventoryUIEvent;
        public VoidEventChannelSO _ToggleEquipmentUIEevent;
        #endregion

        #region Unity Event Method
        private void OnEnable()
        {
            if (dialogueManager != null)
            {
                //이벤트 등록
                dialogueManager.openUIDialogEvent += OpenUIDialogue;
                dialogueManager.closeUIDialogEvent += CloseUIDialog;
            }

            _ToggleInventoryUIEvent.OnEventRaised += TogglePayerInventoryUI;
            _ToggleEquipmentUIEevent.OnEventRaised += ToggelPlayerEquipmentUI;
        }

        private void OnDisable()
        {
            if (dialogueManager != null)
            {
                //이벤트 제거
                dialogueManager.openUIDialogEvent -= OpenUIDialogue;
                dialogueManager.closeUIDialogEvent -= CloseUIDialog;
            }

            _ToggleInventoryUIEvent.OnEventRaised -= TogglePayerInventoryUI;
            _ToggleEquipmentUIEevent.OnEventRaised -= ToggelPlayerEquipmentUI;
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
            isOpen |= dialogueUIManager.gameObject.activeSelf;

            return isOpen;
        }

        //인벤토리 UI 토글
        public void TogglePayerInventoryUI()
        {
            Debug.Log("TogglePayerInventoryUI");
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


        //대화창 열기
        private void OpenUIDialogue(Dialog dialog)
        {
            dialogueUIManager.gameObject.SetActive(true);
            //dialog 대화창 셋팅
            dialogueUIManager.SetDialogue(dialog);
        }

        //대화창 닫기
        private void CloseUIDialog()
        {
            dialogueUIManager.gameObject.SetActive(false);
        }
        #endregion
    }
}