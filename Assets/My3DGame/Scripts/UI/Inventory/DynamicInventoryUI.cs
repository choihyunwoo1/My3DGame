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
        public GameObject slotPrefab;       //슬롯 UI 오브젝트
        public Transform slotsParents;      //생성되는 슬롯 오브젝트의 부모 오브젝트
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
        #endregion
    }
}