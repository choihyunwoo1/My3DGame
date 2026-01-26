using UnityEngine;
using UnityEngine.Events;

namespace My3DGame
{
    /// <summary>
    /// 플레이어가 트리거에 존에 들어왔는지, 나갔는지 체크
    /// </summary>
    public class ZoneTriggerController : MonoBehaviour
    {
        //존에 들어왔을때, 나갔는때 등록된 함수 호출 이벤트 함수
        public UnityAction<bool> _EnterZone;

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                //Debug.Log("플레이어가 트리거에 들어왔다");
                if(_EnterZone != null)
                {
                    _EnterZone.Invoke(true);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                //Debug.Log("플레이어가 트리거에 나갔다");
                if (_EnterZone != null)
                {
                    _EnterZone.Invoke(false);
                }
            }
        }
    }
}