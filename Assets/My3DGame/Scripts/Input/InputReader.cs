using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace My3DGame
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Game/InputReader")]
    public class InputReader : ScriptableObject, MyInput.IGamePlayActions, MyInput.IMenuActions
    {
        #region Variables
        //참조
        private MyInput myInput;

        //GamePlayActions
        public event UnityAction<Vector2> MoveEvent = delegate { };
        public event UnityAction JumpEvent = delegate { };
        public event UnityAction JumpCanceledEvent = delegate { };

        //MenuActions
        public event UnityAction SubmitEvent = delegate { };
        public event UnityAction CancelEvent = delegate { };
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
        }

        //GamePlay 액션맵 활성화
        public void EnableGamePlayInput()
        {
            //활성화
            myInput.GamePlay.Enable();

            //나머지 액션맵들 비활성화
            myInput.Menu.Disable();
        }

        //Menu 액션맵 활성화
        public void EnableMenuInput()
        {
            //활성화
            myInput.Menu.Enable();

            //나머지 액션맵들 비활성화
            myInput.GamePlay.Disable();
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
            throw new System.NotImplementedException();
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
    }
}