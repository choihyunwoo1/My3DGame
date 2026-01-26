using MySample;
using System.Collections.Generic;
using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 대화를 관리하는 클래스
    /// </summary>
    public class DialogManager : MonoBehaviour
    {
        #region Variables
        public DialogSO dialogObject;           //대화 데이터 베이스

        private Queue<Dialog> _currentDialogs = new Queue<Dialog>();  //현재 진행하는 대화

        [Header("Listening On")]
        public IntEventChannelSO _StartDialogEvent;

        [Header("Broadcasting on")]
        public DialogEventChannelSO _ToggleDialogUIEvent;
        #endregion

        #region Unity Event Method
        private void OnEnable()
        {
            _StartDialogEvent.OnEventRaised += StartDialogue;
        }

        private void OnDisable()
        {
            _StartDialogEvent.OnEventRaised -= StartDialogue;
        }
        #endregion

        #region Custom Method
        //대화 초기화
        private void InitDialogue()
        {
            _currentDialogs.Clear();
        }

        //매개변수로 받은 대화 시작하기
        public void StartDialogue(int dialgoIndex)
        {
            //다이알로그 초기화
            InitDialogue();

            //현재 진행하는 대화를 큐에 넣어 가져오기
            foreach (var dialog in dialogObject.database)
            {
                if (dialog.number == dialgoIndex)
                {
                    _currentDialogs.Enqueue(dialog);
                }
            }

            //대화창 보여주기 - 첫번째 대화를 꺼내서 보여준다
            DisplayDialogueData();
        }

        //큐에서 현재 대화 꺼내어 보여준다
        public void DisplayDialogueData()
        {
            //_currentDialogs 체크
            if (_currentDialogs.Count <= 0)
            {
                DialogueEndedAndCloseDialogueUI();
                return;
            }

            //큐에서 현재 대화 꺼내기
            Dialog dialog = _currentDialogs.Dequeue();

            DisplayDialogueLine(dialog);
        }

        //매개변수로 받은 대화 보여주기
        private void DisplayDialogueLine(Dialog dialog)
        {
            //UI열고 대화 보여주기
            _ToggleDialogUIEvent.RaisedEvent(dialog);
        }

        //대화 종료
        private void DialogueEndedAndCloseDialogueUI()
        {
            //대화 초기화
            InitDialogue();

            //UI 닫기
            _ToggleDialogUIEvent.RaisedEvent(null);
        }
        #endregion
    }
}