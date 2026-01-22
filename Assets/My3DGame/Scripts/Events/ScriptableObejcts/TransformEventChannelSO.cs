using UnityEngine;
using UnityEngine.Events;

namespace My3DGame
{
    /// <summary>
    /// 매개변수가 transform인 함수를 등록해서 호출해주는 이벤트 채널 스크립터블 오브젝트 클래스
    /// </summary>
    [CreateAssetMenu(fileName = "TransformEventChannel", menuName = "Events/Transform Event Channel")]
    public class TransformEventChannelSO : ScriptableObject
    {
        //매개변수가 Transform인 이벤트 함수
        public UnityAction<Transform> OnEventRaised;

        //매개변수가 Transform인 이벤트 함수 호출
        public void RaisedEvent(Transform value)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(value);
        }
    }
}