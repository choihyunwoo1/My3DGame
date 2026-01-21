using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace My3DGame.UI
{
    /// <summary>
    /// 인벤토리 UI를 관리하는 클래스들의 부모 (추상) 클래스
    /// 속성: 인벤토리 오브젝트, UI에 있는 슬롯 오브젝트 목록을 관리하는 Dictionary
    /// abstract 메서드: 인벤토리 오브젝트에 있는 슬롯 숫자만큼 슬롯 오브젝트를 생성
    /// </summary>
    [RequireComponent(typeof(EventTrigger))]
    public abstract class InventoryUI : MonoBehaviour
    {
        #region Variables
        public InventorySO inventoryObejct;

        //key: 생성하는 슬롯 게임 오브젝트, value: 생성시 매칭되는 ItemSlot
        public Dictionary<GameObject, ItemSlot> slotUIs = new Dictionary<GameObject, ItemSlot>();

        //슬롯 선택
        public ItemInfoUI itemInfoUI;
        protected GameObject selectSlotObect = null;        //선택된 슬롯 오브젝트

        [SerializeField] protected bool isEquipInven = false;

        //창 닫기
        public UnityAction _OnCloseUIEvent;
        #endregion

        #region abstract
        //기능 정의: 인벤토리 오브젝트의 슬롯으로 슬롯 오브젝트 생성
        public abstract void CreateSlots();
        #endregion

        #region Unity Event Method
        protected void Awake()
        {
            //슬롯 오브젝트 생성
            CreateSlots();

            //인벤토리 슬롯 셋팅
            if (inventoryObejct != null)
            {
                for (int i = 0; i < inventoryObejct.Slots.Length; i++)
                {
                    inventoryObejct.Slots[i].parents = inventoryObejct;
                    inventoryObejct.Slots[i].OnPostUpdate += OnPostUpdate;

                    //강제로 슬롯 업데이트 실행
                    if(inventoryObejct.Slots[i].OnPostUpdate != null )
                    {
                        inventoryObejct.Slots[i].OnPostUpdate.Invoke(inventoryObejct.Slots[i]);
                    }
                }
            }

            //슬롯 선택 초기화
            UpdateSelectSlot(null);

            //이벤트 함수 등록
            _OnCloseUIEvent += CloseInventoryUI;
        }
        #endregion

        #region Custom Method
        //아이템 슬롯 아이템,수량 갱신후 호출되는 함수
        public void OnPostUpdate(ItemSlot slot)
        {
            //슬롯 체크
            if (slot == null || slot.slotUI == null)
                return;

            //슬롯 UI 갱신
            ItemSlotUI itemSlotUI = slot.slotUI.GetComponent<ItemSlotUI>();
            if (itemSlotUI)
            {
                itemSlotUI.UpdateSlot(slot);
            }
        }

        //슬롯 오브젝트 선택
        public void UpdateSelectSlot(GameObject go)
        {
            //아이템 설명창 오픈 체크
            if (selectSlotObect != null)
            {
                itemInfoUI.CloseItemInfoUI();
                _OnCloseUIEvent -= itemInfoUI.CloseItemInfoUI;
            }

            //선택된 슬롯 오브젝트 저장
            selectSlotObect = go;

            //선택한 슬롯에 아이템이 있으면 설명창을 연다
            if (selectSlotObect != null)
            {
                itemInfoUI.OpenItemInfoUI();
                itemInfoUI.SetItemInfoUI(slotUIs[go], isEquipInven);
                _OnCloseUIEvent += itemInfoUI.CloseItemInfoUI;
            }

            //선택된 슬롯 선택 이미지 활성화
            foreach (KeyValuePair<GameObject, ItemSlot> slot in slotUIs)
            {
                ItemSlotUI itemSlotUI = slot.Key.GetComponent<ItemSlotUI>();
                if(itemSlotUI)
                {
                    //선택한 슬롯 오브젝트 찾기
                    if(slot.Key == go)
                    {
                        itemSlotUI.SelectSlot(true);
                    }
                    else
                    {
                        itemSlotUI.SelectSlot(false);
                    }
                }
            }
        }

        //인벤토리 UI 닫기
        private void CloseInventoryUI()
        {
            selectSlotObect = null;
            //모든 슬롯 비활성화
            foreach (KeyValuePair<GameObject, ItemSlot> slot in slotUIs)
            {
                ItemSlotUI itemSlotUI = slot.Key.GetComponent<ItemSlotUI>();
                if (itemSlotUI)
                {
                    itemSlotUI.SelectSlot(false);
                }
            }
        }
        #endregion

        #region EventTrigger
        //이벤트 함수 등록하는 함수
        protected void AddEvent(GameObject go, EventTriggerType type, UnityAction<BaseEventData> action)
        {
            //이벤트 트리거 오브젝트 체크
            EventTrigger trigger = go.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                return;
            }

            //이벤트 엔트리 구성
            EventTrigger.Entry eventTrigger = new EventTrigger.Entry { eventID = type };
            eventTrigger.callback.AddListener(action);

            //이벤트 엔트리를 트리거에 등록
            trigger.triggers.Add(eventTrigger);
        }

        //인벤토리 UI에 마우스 들어오면 호출
        public void OnEnterInterface(GameObject go)
        {
            Debug.Log($"OnEnterInterface Object: {go.name}");
            MouseData.inventoryUIMouseOver = go.GetComponent<InventoryUI>();
        }

        //인벤토리 UI에 마우스 나가면 호출
        public void OnExitInterface(GameObject go)
        {
            Debug.Log($"OnExitInterface Object: {go.name}");
            MouseData.inventoryUIMouseOver = null;
        }

        //슬롯 오브젝트에 마우스 들어오면 호출
        public void OnEnterSlot(GameObject go)
        {
            Debug.Log($"OnEnterSlot Object: {go.name}");
            MouseData.slotObjectMouseOver = go;
        }

        //슬롯 오브젝트에 마우스 들어오면 호출
        public void OnExitSlot(GameObject go)
        {
            Debug.Log($"OnExitSlot Object: {go.name}");
            MouseData.slotObjectMouseOver = null;
        }

        //슬롯 UI 오브젝트를 선택하고 마우스 드래그 시작할때 호출
        public void OnStartDrag(GameObject go)
        {
            Debug.Log($"OnStartDrag Object: {go.name}");
            MouseData.tempItemBeginDragged = CreateDragItem(go);

            //슬롯 선택 초기화
            UpdateSelectSlot(null);
        }

        //슬롯 UI 오브젝트를 선택하고 마우스 드래그 ing 호출
        public void OnDrag(GameObject go)
        {
            if(MouseData.slotObjectMouseOver == null)
            {
                return;
            }

            //마우스 위치를 임시 드래그 이미지와 동기화
            MouseData.tempItemBeginDragged.GetComponent<RectTransform>().position
                = Input.mousePosition;
        }

        //슬롯 UI 오브젝트를 선택하고 마우스 드래그를 끝낼때 호출
        public void OnEndDrag(GameObject go)
        {
            Debug.Log($"OnEndDrag Object: {go.name}");
            //임시 드래그 이미지 오브젝트 킬
            Destroy(MouseData.tempItemBeginDragged);

            //마우스의 위치가 인벤토리 UI 밖에 있을때
            if(MouseData.inventoryUIMouseOver == null)
            {
                //아이템 버리기
                slotUIs[go].AddAmount(-1);
            }
            else //인벤토리 UI 안에 있을때
            {
                //마우스의 위치가 슬롯 오브젝트 안에 있으면
                if (MouseData.slotObjectMouseOver != null)
                {
                    //먼저 선택해서 드래그 아이템 슬롯과 현재 마우스 위치한 슬롯과의 아이템 교환
                    ItemSlot mouseHoverSlot = MouseData.inventoryUIMouseOver
                        .slotUIs[MouseData.slotObjectMouseOver];
                    //아이템 교환
                    inventoryObejct.SwapItems(slotUIs[go], mouseHoverSlot);
                }
            }
        }

        //슬롯 UI 오브젝트를 마우스 선택시 호출
        public void OnClick(GameObject go)
        {
            //선택한 게임오브젝트에서 슬롯 얻어오기
            ItemSlot itemSlot = slotUIs[go];

            //아이템 체크
            if (itemSlot.item.id >= 0)
            {
                //선택한 오브젝트 체크
                if(selectSlotObect == go)
                {
                    UpdateSelectSlot(null);
                }
                else
                {
                    UpdateSelectSlot(go);
                }   
            }
            else //빈슬롯 선택
            {
                //슬롯 선택 초기화
                UpdateSelectSlot(null);
            }
            
        }

        //마우스 드래그시 마우스 포인터에 달고 다니는 아이템 오브젝트(아이콘 이미지) 생성
        public GameObject CreateDragItem(GameObject go)
        {
            //슬롯 오브젝트 체크
            if (slotUIs[go].item.id <= -1)
            {
                return null;
            }

            GameObject dragItem = new GameObject();
            RectTransform rectTransform = dragItem.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(70, 70);
            dragItem.transform.SetParent(this.transform.parent);
            Image itemIamge = dragItem.AddComponent<Image>();
            itemIamge.sprite = slotUIs[go].ItemObject.icon;
            itemIamge.raycastTarget = false;
            dragItem.name = "Drag Item Image";

            return dragItem;
        }
        #endregion
    }
}