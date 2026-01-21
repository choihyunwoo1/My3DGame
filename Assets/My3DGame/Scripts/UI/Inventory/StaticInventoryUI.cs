using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace My3DGame.UI
{
    /// <summary>
    /// 갯수와 위치가 고정된 아이템 슬롯 목록을 가진 인벤토리 UI를 관리하는 클래스
    /// </summary>
    public class StaticInventoryUI : InventoryUI
    {
        #region Variables
        public InventorySO playerInventory;

        public GameObject[] staticSlot;
        #endregion

        #region Unity Event Method
        #endregion

        #region Custom Method
        public override void CreateSlots()
        {
            //UI에 있는 슬롯 오브젝트 목록을 관리하는 Dictionary 생성
            slotUIs = new Dictionary<GameObject, ItemSlot>();

            //인벤토리 오브젝트에 있는 슬롯 숫자만큼 슬롯 오브젝트를 생성
            for (int i = 0; i < inventoryObejct.Slots.Length; i++)
            {
                GameObject go = staticSlot[i];

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

        //아이템 장착: 매개변수로 들어온 아이템과 장착할 슬롯의 아이템과 SwapItem 한다
        public void Equip(ItemSlot itemSlot)
        {
            //장착할 아이템이 장착될 위치를 찾아 장착
            foreach (var go in staticSlot)
            {
                ItemSlot slot = slotUIs[go];
                if(slot.CanPalceInSlot(itemSlot.ItemObject))
                {
                    inventoryObejct.SwapItems(slot, itemSlot);
                    break;
                }
            }
        }

        //아이템 탈착
        public void UnEquip()
        {
            //선택된 슬롯 체크
            if (selectSlotObect == null)
                return;

            //플레이어 인벤토리 풀체크
            if (playerInventory.AddItem(slotUIs[selectSlotObect].item, 1))
            {
                //추가가 성공되면 슬롯 목록에서 제거
                slotUIs[selectSlotObect].RemoveItem();

                //선택 해제
                UpdateSelectSlot(null);
            }
        }

        //모든 아이템 탈착
        public void UnEquipAll()
        {
            foreach(var slotObject in staticSlot)
            {
                //빈 슬롯 체크
                if (slotUIs[slotObject].item.id <= -1 || slotUIs[slotObject].amount <= 0)
                    continue;

                //플레이어 인벤토리 풀체크
                if (playerInventory.AddItem(slotUIs[slotObject].item, 1))
                {
                    //추가가 성공되면 슬롯 목록에서 제거
                    slotUIs[slotObject].RemoveItem();
                }
            }

            //선택 해제
            UpdateSelectSlot(null);
        }
        #endregion

    }
}