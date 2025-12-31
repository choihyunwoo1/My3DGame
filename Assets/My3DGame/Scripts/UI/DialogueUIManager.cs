using UnityEngine;

namespace My3DGame.UI
{
    /// <summary>
    /// DialogueUI를 관리하는 클래스
    /// 대화창 UI 셋팅하기
    /// </summary>
    public class DialogueUIManager : MonoBehaviour
    {
        #region Variables
        #endregion

        #region Custom Method
        //매개변수로 받은 대화로 UI 셋팅
        public void SetDialogue(Dialog dialog)
        {
            Debug.Log(dialog.sentence);
        }
        #endregion
    }
}