using UnityEngine;
using TMPro;

namespace My3DGame.UI
{
    /// <summary>
    /// 인벤토리 UI에서 선택된 슬롯의 아이템 정보를 보여주는 창을 관리하는 클래스
    /// </summary>
    public class ItemInfoUI : MonoBehaviour
    {
        #region Variables
        public TextMeshProUGUI itemName;
        public TextMeshProUGUI itemDescription;

        //아이템 능력치
        public TextMeshProUGUI[] attributes = new TextMeshProUGUI[3];
        public TextMeshProUGUI[] value = new TextMeshProUGUI[3];
        //아이템 판매 가격
        public TextMeshProUGUI sellPrice;

        //버튼
        public GameObject useButtton;
        public GameObject equipButton;
        public GameObject sellButton;
        public GameObject unEquipButton;
        #endregion

        #region Custom Method
        //선택시 슬롯의 아이템 정보를 셋팅한다
        public void SetItemInfoUI(ItemSlot itemSlot, bool isEquipInven)
        {
            //정보창 셋팅
            itemName.text = itemSlot.ItemObject.name;
            itemDescription.text = itemSlot.ItemObject.description;

            for (int i = 0; i < attributes.Length; i++)
            {
                if(i < itemSlot.item.buffs.Length)
                {
                    attributes[i].text = itemSlot.item.buffs[i].stat.ToString();
                    value[i].text = itemSlot.item.buffs[i].value.ToString();
                }
                else
                {
                    attributes[i].text = string.Empty;
                    value[i].text = string.Empty;
                }
            }

            //상점 판매가는 구매가의 반값
            sellPrice.text = ((int)(itemSlot.ItemObject.shopPrice / 2)).ToString();

            //버튼 셋팅
            ResetButtons();
            if(isEquipInven)
            {
                unEquipButton.SetActive(true);
            }
            else
            {
                if(itemSlot.ItemObject.itemType == ItemType.Food ||
                    itemSlot.ItemObject.itemType == ItemType.Default)
                {
                    useButtton.SetActive(true);
                }
                else
                {
                    equipButton.SetActive(true);
                }
                sellButton.SetActive(true);
            }
        }

        private void ResetButtons()
        {
            useButtton.SetActive(false);
            equipButton.SetActive(false);
            sellButton.SetActive(false);
            unEquipButton.SetActive(false);
        }

        public void OpenItemInfoUI()
        {
            this.gameObject.SetActive(true);
        }

        public void CloseItemInfoUI()
        {
            ResetButtons();
            this.gameObject.SetActive(false);
        }
        #endregion
    }
}