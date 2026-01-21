using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace My3DGame.UI
{
    /// <summary>
    /// 가변적인 아이템 슬롯 목록을 가진 인벤토리 UI를 관리하는 클래스
    /// </summary>
    public class DynamicInventoryUI : InventoryUI
    {
        #region Variables
        public InventorySO palyerEquipment;

        public GameObject slotPrefab;       //슬롯 UI 오브젝트
        public Transform slotsParents;      //생성되는 슬롯 오브젝트의 부모 오브젝트
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //초기화
            isEquipInven = false;
        }
        #endregion

        #region Custom Method
        public override void CreateSlots()
        {
            //UI에 있는 슬롯 오브젝트 목록을 관리하는 Dictionary 생성
            slotUIs = new Dictionary<GameObject, ItemSlot>();

            //인벤토리 오브젝트에 있는 슬롯 숫자만큼 슬롯 오브젝트를 생성
            for (int i = 0; i < inventoryObejct.Slots.Length; i++)
            {
                GameObject go = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, slotsParents);

                //생성된 슬롯 오브젝트의 트리거에 이벤트 등록
                AddEvent(go, EventTriggerType.PointerEnter, delegate { OnEnterSlot(go); });
                AddEvent(go, EventTriggerType.PointerExit, delegate { OnExitSlot(go); });
                AddEvent(go, EventTriggerType.BeginDrag, delegate { OnStartDrag(go); });
                AddEvent(go, EventTriggerType.Drag, delegate { OnDrag(go); });
                AddEvent(go, EventTriggerType.EndDrag, delegate { OnEndDrag(go); });
                AddEvent(go, EventTriggerType.PointerClick, delegate { OnClick(go); });

                //slotUIs 등록
                inventoryObejct.Slots[i].slotUI = go;
                slotUIs.Add(go, inventoryObejct.Slots[i]);
            }
        }

        //인벤토리에 아이템 넣기, 슬롯 체크하지 않는다
        public bool AddInventoryItem(Item newItem, int amount)
        {
            return inventoryObejct.AddItem(newItem, amount);
        }

        //아이템 사용하기
        public void UseItem()
        {
            //선택된 슬롯 체크
            if (selectSlotObect == null)
                return;

            //소모품 사용
            inventoryObejct.UseItem(slotUIs[selectSlotObect]);

            //선택 해제
            UpdateSelectSlot(null);
        }

        //아이템 장착하기
        public void EquipItem()
        {
            //선택된 슬롯 체크
            if (selectSlotObect == null)
                return;

            //아이템 장착하기
            //Debug.Log($"선택된 아이템을 장착합니다");
            palyerEquipment.EquipItem(slotUIs[selectSlotObect]);

            //선택 해제
            UpdateSelectSlot(null);
        }

        //아이템 판매하기
        public void SellItem()
        {
            //선택된 슬롯 체크
            if (selectSlotObect == null)
                return;

            //아이템 판매
            int sellPrice = (int)(slotUIs[selectSlotObect].ItemObject.shopPrice / 2);
            Debug.Log($"{sellPrice} 골드를 받고 아이템을 버린다");

            //슬롯에서 아이템 제거
            slotUIs[selectSlotObect].AddAmount(-1);

            //선택 해제
            UpdateSelectSlot(null);
        }
        #endregion
    }
}