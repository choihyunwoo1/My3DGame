using UnityEngine;
using UnityEngine.Events;

namespace My3DGame
{
    /// <summary>
    /// 매개변수가 Dialog인 함수를 등록해서 호출해주는 이벤트 채널 스크립터블 오브젝트 클래스
    /// </summary>
    [CreateAssetMenu(fileName = "DialogEventChannel", menuName = "Events/Dialog Event Channel")]
    public class DialogEventChannelSO : ScriptableObject
    {
        //매개변수가 Dialog인 이벤트 함수
        public UnityAction<Dialog> OnEventRaised;

        //매개변수가 bool인 이벤트 함수 호출
        public void RaisedEvent(Dialog value)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(value);
        }
    }
}