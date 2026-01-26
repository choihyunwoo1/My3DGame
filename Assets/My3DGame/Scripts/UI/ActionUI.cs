using UnityEngine;
using TMPro;

namespace My3DGame.UI
{
    /// <summary>
    /// 액션UI를 관리하는 클래스
    /// </summary>
    public class ActionUI : MonoBehaviour
    {
        #region Variables
        public TextMeshProUGUI actionText;
        #endregion

        #region Custom Method
        public void SetActionUI(string action)
        {
            actionText.text = action;
        }
        #endregion
    }
}