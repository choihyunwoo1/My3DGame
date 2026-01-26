using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 퀘스트를 주는 Npc를 관리하는 클래스
    /// </summary>
    public class QuestGiverNpc : PickupNpc
    {

        #region Variables
        #endregion

        #region Custom Method
        protected override void DoAction()
        {
            //픽업 체크
            if (isPickup == false)
                return;

            int randNum = Random.Range(0, 3);
            _StartDialogEvent.RaisedEvent(randNum);

        }
        #endregion
    }
}