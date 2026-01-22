using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace My3DGame
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Game/InputReader")]
    public class InputReader : ScriptableObject, MyInput.IGamePlayActions, MyInput.IMenuActions, MyInput.IClickNMoveActions, MyInput.IHotKeyActions
    {
        #region Variables
        //참조
        private MyInput myInput;

        //GamePlayActions
        public event UnityAction<Vector2> MoveEvent = delegate { };
        public event UnityAction JumpEvent = delegate { };
        public event UnityAction JumpCanceledEvent = delegate { };
        public event UnityAction AttackEvent = delegate { };

        //MenuActions
        public event UnityAction SubmitEvent = delegate { };
        public event UnityAction CancelEvent = delegate { };

        //ClickNMoveActions
        public event UnityAction ClickEvent = delegate { };
        public event UnityAction<Vector2> MousePositionEvent = delegate { };

        //HotKey Actions
        [Header("BroadCasting On")]
        public VoidEventChannelSO _ToggleInventoryUIEvent;
        public VoidEventChannelSO _ToggleEquipmentUIEevent;
        #endregion

        #region Unity Event Mehtod
        private void OnEnable()
        {
            if(myInput == null)
            {
                myInput = new MyInput();

                //액션 맵 셋팅
                myInput.GamePlay.SetCallbacks(this);
                myInput.Menu.SetCallbacks(this);
                myInput.ClickNMove.SetCallbacks(this);
                myInput.HotKey.SetCallbacks(this);
            }
        }

        private void OnDisable()
        {
            DisableAllInput();
        }
        #endregion

        #region ActionMaps
        //모든 액션맵 비활성화
        public void DisableAllInput()
        {
            myInput.GamePlay.Disable();
            myInput.Menu.Disable();
            myInput.ClickNMove.Disable();
            myInput.HotKey.Disable();
        }

        //GamePlay 액션맵 활성화
        public void EnableGamePlayInput()
        {
            //나머지 액션맵들 비활성화
            DisableAllInput();

            //활성화
            myInput.GamePlay.Enable();
            myInput.HotKey.Enable();
        }

        //Menu 액션맵 활성화
        public void EnableMenuInput()
        {
            //나머지 액션맵들 비활성화
            DisableAllInput();

            //활성화
            myInput.Menu.Enable();
            myInput.HotKey.Enable();
        }

        //ClickNMove 액션 맵 활성화
        public void EnableClickNMoveInput()
        {
            //나머지 액션맵들 비활성화
            DisableAllInput();

            //활성화
            myInput.ClickNMove.Enable();
        }
        #endregion

        #region GamePlayActions
        public void OnMove(InputAction.CallbackContext context)
        {
            MoveEvent.Invoke(context.ReadValue<Vector2>());
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
                JumpEvent.Invoke();

            if (context.phase == InputActionPhase.Canceled)
                JumpCanceledEvent.Invoke();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                AttackEvent.Invoke();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region MenuActions
        public void OnSubmit(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
                SubmitEvent.Invoke();
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
                CancelEvent.Invoke();
        }
        #endregion

        #region ClickNMoveActions
        public void OnMouseClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
                ClickEvent.Invoke();
        }

        public void OnMousePosition(InputAction.CallbackContext context)
        {
            MousePositionEvent.Invoke(context.ReadValue<Vector2>());
        }
        #endregion

        #region HotKey Actions
        public void OnHotKey1(InputAction.CallbackContext context)
        {
            //플레이어 인벤토리 창을 연다
            if (context.phase == InputActionPhase.Performed)
                _ToggleInventoryUIEvent.RaisedEvent();
        }

        public void OnHotKey2(InputAction.CallbackContext context)
        {
            //플레이어 장비 장착창을 연다
            if (context.phase == InputActionPhase.Performed)
                _ToggleEquipmentUIEevent.RaisedEvent();
        }

        public void OnHotKey3(InputAction.CallbackContext context)
        {
            
        }
        #endregion
    }
}