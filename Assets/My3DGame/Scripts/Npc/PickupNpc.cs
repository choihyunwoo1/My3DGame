using My3DGame;
using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// Npc의 인터택티브 기능을 관리하는 추상 클래스
    /// </summary>
    public abstract class PickupNpc : MonoBehaviour
    {
        #region abstract Method
        protected abstract void DoAction();         //인터랙티브 기능
        #endregion

        #region Variables
        public Npc npc;         //

        //참조
        public ZoneTriggerController zoneTriggerController;

        public string actionText = "Pickup";                //액션 텍스트

        [Header("Broadcasting On")]
        public StringEventChannelSO _ToggleActionUIEvent;
        public IntEventChannelSO _StartDialogEvent;         //대화 시작 이벤트

        [Header("Listening On")]
        public VoidEventChannelSO _InteractEvent;   //인터랙티브 기능

        protected bool isPickup = false;                      //픽업 여부 체크
        #endregion

        #region Unity Event Method
        private void OnEnable()
        {
            zoneTriggerController._EnterZone += ToggleActionUI;
            _InteractEvent.OnEventRaised += DoAction;
        }

        private void OnDisable()
        {
            zoneTriggerController._EnterZone -= ToggleActionUI;
            _InteractEvent.OnEventRaised += DoAction;
        }
        #endregion

        #region Custom Method
        protected virtual void ToggleActionUI(bool isShow)
        {
            isPickup = isShow;          //픽업 여부 저장

            if (isShow)
            {
                _ToggleActionUIEvent.RaisedEvent(actionText + " " + npc.name);
            }
            else
            {
                _ToggleActionUIEvent.RaisedEvent(string.Empty);
            }
        }
        #endregion

    }
}