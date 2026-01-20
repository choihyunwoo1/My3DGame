using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace My3DGame.UI
{
    /// <summary>
    /// 아이템 슬롯 UI를 관리하는 클래스
    /// </summary>
    public class ItemSlotUI : MonoBehaviour
    {
        #region Variables
        public Image iconImage;
        public GameObject selectImage;
        public TextMeshProUGUI amountText;
        #endregion

        #region Custom Method
        //슬롯 UI 갱신
        public void UpdateSlot(ItemSlot slot)
        {
            //슬롯 체크
            if (slot == null)
                return;

            //슬롯 아이템 체크
            if(slot.item.id <= -1 || slot.item.name == null || slot.amount <= 0) //빈슬롯
            {
                iconImage.sprite = null;
                iconImage.gameObject.SetActive(false);
                amountText.text = string.Empty;
            }
            else
            {
                iconImage.sprite = slot.ItemObject.icon;
                iconImage.gameObject.SetActive(true);
                amountText.text = slot.amount == 1 ? string.Empty : slot.amount.ToString();
            }
        }

        //selectImage 활성화 여부
        public void SelectSlot(bool select)
        {
            selectImage.SetActive(select);
        }
        #endregion
    }
}