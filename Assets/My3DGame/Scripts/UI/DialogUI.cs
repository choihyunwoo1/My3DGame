using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace My3DGame
{
    /// <summary>
    /// DialogueUI를 관리하는 클래스
    /// 대화창 UI 셋팅하기
    /// </summary>
    public class DialogUI : MonoBehaviour
    {
        #region Variables
        public TextMeshProUGUI _lineText;           //대화 글
        public TextMeshProUGUI _actorName;          //대화 캐릭터 이름
        
        public GameObject _nextButton;              //다음 액션 버튼 - 다음 대화 보여주기

        //창 닫기
        public UnityAction _OnCloseUIEvent;
        #endregion

        #region Custom Method
        //매개변수로 받은 대화로 UI 셋팅
        public void SetDialogue(Dialog dialog)
        {
            //Actor 이름
            _actorName.text = dialog.name;

            //next 버튼
            _nextButton.SetActive(true);

            //대화 글
            _lineText.text = dialog.sentence;
        }
        #endregion
    }
}