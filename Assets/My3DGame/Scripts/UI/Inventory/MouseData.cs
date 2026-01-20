using UnityEngine;

namespace My3DGame.UI
{
    /// <summary>
    /// 인벤토리 시스템에서 마우스 움직임에 따른 오브젝트 정보를 관리하는 클래스
    /// </summary>
    public static class MouseData
    {
        public static InventoryUI inventoryUIMouseOver;     //마우스가 올라가 있는 인벤토리 UI
        public static GameObject slotObjectMouseOver;       //마우스가 올라가 있는 슬롯 오브젝트
        public static GameObject tempItemBeginDragged;      //마우스 드래그시 마우스를 따라 임시 아이템 오브젝트
    }
}