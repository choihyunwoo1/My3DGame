using UnityEngine;
using System.Linq;

namespace My3DGame
{
    /// <summary>
    /// 인벤토리를 관리하는 스크립터블 오브젝트 클래스
    /// 속성: 인벤토리 컨터이너, 아이템 데이터 베이스, 인벤토리 타입
    /// 기능: 인벤토리에 아이템 추가하기, 아이템 바꾸기, 아이템 장착하기, 아이템 사용하기
    /// </summary>
    [CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
    public class InventorySO : ScriptableObject
    {
        #region Variables
        public Inventory container = new Inventory();
        
        public ItemDataBaseSO database;                 //아이템 데이터 베이스
        public InventoryType inventoryType;             //인벤토리 타입
        #endregion

        #region Property
        //인벤토리 컨테이너 슬롯에 직접 접근
        public ItemSlot[] Slots => container.slots;

        //인벤 풀 체크 - 빈 슬롯 갯수 
        public int EmptySlotCount
        {
            get
            {
                int count = 0;
                foreach (var slot in Slots)
                {
                    if(slot.item.id <= -1)
                        count++;
                }

                return count;
            }
        }
        #endregion

        #region Custom Method
        //인벤토리에 아이템 추가하기
        public bool AddItem(Item item, int amount)
        {
            if (database.itemObjects[item.id].stackable == false) //일반 아이템
            {
                //인벤 풀 체크
                if(EmptySlotCount <= 0)
                {
                    return false;
                }

                ItemSlot emptySlot = GetEmptySlot();
                emptySlot.UpdateSlot(item, amount);
            }
            else //슬롯에 누적 가능한 아이템
            {
                //인벤에 아이템을 가진 슬롯을 가져온다
                ItemSlot slot = FindItemInInventory(item);
                if (slot == null)
                {
                    //인벤 풀 체크
                    if (EmptySlotCount <= 0)
                    {
                        return false;
                    }

                    ItemSlot emptySlot = GetEmptySlot();
                    emptySlot.UpdateSlot(item, amount);
                }
                else
                {
                    //수량만 추가
                    slot.AddAmount(amount);
                }
            }

            return true;
        }

        //인벤토리에서 빈 슬롯 가져오기
        public ItemSlot GetEmptySlot()
        {
            return Slots.FirstOrDefault(i => i.item.id <= -1);
        }

        //인벤토리에서 매개변수로 들어오 아이템이 있는 슬롯 가져오기
        public ItemSlot FindItemInInventory(Item item)
        {
            return Slots.FirstOrDefault(i => i.item.id == item.id);
        }

        public ItemSlot FindItemInInventory(Item item, int addAmount)
        {
            return Slots.FirstOrDefault(i => i.item.id == item.id && (i.amount+ addAmount) <= 99);
        }

        //인벤토리에서 매개변수로 들어오 아이템 오브젝트로 만든 아이템이 있는지 체크
        public bool IsContainItem(ItemObjectSO itemObject)
        {
            return Slots.FirstOrDefault(i => i.item.id == itemObject.data.id) != null;
        }

        //아이템 바꾸기: 선택된 두개의 슬롯에서 아이템을 서로 바꾼다
        public void SwapItems(ItemSlot itemA, ItemSlot itemB)
        {
            //아이템 동일 여부 체크
            if (itemA == itemB)
                return;

            //교환할 아이템이 서로 교환할 슬롯에 장착 가능한지 체크
            if(itemB.CanPalceInSlot(itemA.ItemObject) && itemA.CanPalceInSlot(itemB.ItemObject))
            {
                ItemSlot tempSlot = new ItemSlot(itemA.item, itemA.amount);
                itemA.UpdateSlot(itemB.item, itemB.amount);
                itemB.UpdateSlot(tempSlot.item, tempSlot.amount);
            }
        }

        //아이템 사용하기
        public void UseItem(ItemSlot useSlot)
        {
            //빈 슬롯 체크
            if (useSlot.ItemObject == null 
                || useSlot.item.id <= -1 || useSlot.amount <= 0)
                return;

            Debug.Log($"{useSlot.item.name} 아이템 효과 구현");

            useSlot.AddAmount(-1);
        }

        //아이템 장착하기
        public void EquipItem(ItemSlot itemSlot)
        {
            //매개변수로 들어온 아이템이 장착될 위치 찾기
            foreach (var slot in Slots)
            {
                if(slot.CanPalceInSlot(itemSlot.ItemObject))
                {
                    SwapItems(slot, itemSlot);
                    break;
                }
            }
        }
        #endregion

    }
}