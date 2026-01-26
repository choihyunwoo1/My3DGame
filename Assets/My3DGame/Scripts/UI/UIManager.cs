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

        [Header("Listening On")]
        public VoidEventChannelSO _ToggleInventoryUIEvent;
        public VoidEventChannelSO _ToggleEquipmentUIEevent;
        public DialogEventChannelSO _ToggleDialogUIEvent;
        public StringEventChannelSO _ToggleActionUIEvent;
        #endregion

        #region Unity Event Method
        private void OnEnable()
        {
            _ToggleInventoryUIEvent.OnEventRaised += TogglePayerInventoryUI;
            _ToggleEquipmentUIEevent.OnEventRaised += ToggelPlayerEquipmentUI;
            _ToggleDialogUIEvent.OnEventRaised += ToggleDialogUI;
            _ToggleActionUIEvent.OnEventRaised += ToggleActionUI;
        }

        private void OnDisable()
        {
            _ToggleInventoryUIEvent.OnEventRaised -= TogglePayerInventoryUI;
            _ToggleEquipmentUIEevent.OnEventRaised -= ToggelPlayerEquipmentUI;
            _ToggleDialogUIEvent.OnEventRaised -= ToggleDialogUI;
            _ToggleActionUIEvent.OnEventRaised -= ToggleActionUI;
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

        //대화창 열기 토글
        public void ToggleDialogUI(Dialog dialog)
        {
            if(dialog == null)
            {
                ToggleUI(dialogUI.gameObject);
            }
            else
            {
                if (dialogUI.gameObject.activeSelf == false)
                {
                    ToggleUI(dialogUI.gameObject);
                }
                dialogUI.SetDialogue(dialog);
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