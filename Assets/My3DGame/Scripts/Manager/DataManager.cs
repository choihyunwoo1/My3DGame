using UnityEngine;
using My3DGame.GameData;

namespace My3DGame
{
    /// <summary>
    /// 게임 데이터를 관리하는 클래스
    /// </summary>
    public class DataManager : MonoBehaviour
    {
        #region Variables
        private static EffectData effectData = null;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //데이터 읽어오기
            GetEffectData();
        }
        #endregion

        #region Custom Method
        //이펙트 데이터 얻어오기
        public static EffectData GetEffectData()
        {
            if(effectData == null)
            {
                effectData = ScriptableObject.CreateInstance<EffectData>();
                effectData.LoadData();
            }

            return effectData;
        }
        #endregion
    }
}