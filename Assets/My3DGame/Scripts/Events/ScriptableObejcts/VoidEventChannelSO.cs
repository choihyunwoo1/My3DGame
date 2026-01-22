using UnityEngine;
using UnityEngine.Events;

namespace My3DGame
{
    /// <summary>
    /// 매개변수가 없는 함수를 등록해서 호출해주는 이벤트 채널 스크립터블 오브젝트 클래스
    /// </summary>
    [CreateAssetMenu(fileName = "VoidEventChannel", menuName = "Events/Void Event Channel")]
    public class VoidEventChannelSO : ScriptableObject
    {
        //매개변수가 없는 이벤트 함수
        public UnityAction OnEventRaised;

        //매개변수가 없는 이벤트 함수 호출
        public void RaisedEvent()
        {
            if(OnEventRaised != null)
                OnEventRaised.Invoke();
        }
    }
}