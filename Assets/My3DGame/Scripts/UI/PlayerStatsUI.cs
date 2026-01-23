using UnityEngine;
using TMPro;

namespace My3DGame.UI
{
    /// <summary>
    /// 플레이어 스탯 UI를 관리하는 클래스
    /// </summary>
    public class PlayerStatsUI : MonoBehaviour
    {
        #region Variables
        public StatsSO statsObejct;
        public InventorySO palyerEquipment;

        //UI
        public TextMeshProUGUI[] attributesText;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //palyerEquipment의 슬롯에 이벤트 함수 등록
            foreach (var slot in palyerEquipment.Slots)
            {
                slot.OnPreUpdate += OnUnEquipItem;
                slot.OnPostUpdate += OnEquipItem;

                //강제로 슬롯 업데이트 실행
                OnEquipItem(slot);
            }
        }

        private void OnEnable()
        {
            statsObejct.OnChangedStats += OnChangedStats;

            //스탯값 UI Text 적용
            UpdateAttributesText();
        }

        private void OnDisable()
        {
            statsObejct.OnChangedStats -= OnChangedStats;
        }
        #endregion

        #region Custom Method
        //스탯값 UI Text 적용
        public void UpdateAttributesText()
        {
            attributesText[0].text = statsObejct.GetModifiredValue(CharacterAttribute.Agility).ToString();
            attributesText[1].text = statsObejct.GetModifiredValue(CharacterAttribute.Intellect).ToString();
            attributesText[2].text = statsObejct.GetModifiredValue(CharacterAttribute.Stamina).ToString();
            attributesText[3].text = statsObejct.GetModifiredValue(CharacterAttribute.Strength).ToString();
        }

        private void OnChangedStats()
        {
            UpdateAttributesText();
        }

        //아이템 장착시 stats에 아이템 buff값 추가
        private void OnEquipItem(ItemSlot itemSlot)
        {
            //빈 슬롯 체크
            if (itemSlot.ItemObject == null)
                return;

            //장착 장비 아이템 체크
            if (itemSlot.parents.inventoryType != InventoryType.Equipment)
                return;

            //아이템 buff값 추가
            foreach (var buff in itemSlot.item.buffs)
            {
                foreach (var attribute in statsObejct.attributes)
                {
                    if(buff.stat == attribute.type)
                    {
                        attribute.value.AddModifier(buff);
                    }
                }
            }
        }

        //아이템 탈착시 stats에 아이템 buff값 제거
        private void OnUnEquipItem(ItemSlot itemSlot)
        {
            //빈 슬롯 체크
            if (itemSlot.ItemObject == null)
                return;

            //장착 장비 아이템 체크
            if (itemSlot.parents.inventoryType != InventoryType.Equipment)
                return;

            //아이템 buff값 추가
            foreach (var buff in itemSlot.item.buffs)
            {
                foreach (var attribute in statsObejct.attributes)
                {
                    if (buff.stat == attribute.type)
                    {
                        attribute.value.RemoveModifier(buff);
                    }
                }
            }
        }
        #endregion
    }
}
