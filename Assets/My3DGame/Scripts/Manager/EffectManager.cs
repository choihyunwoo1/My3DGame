using UnityEngine;
using My3DGame.GameData;
using My3DGame.Utillity;

namespace My3DGame
{
    public class EffectManager : Singleton<EffectManager>
    {
        #region Variables
        private Transform effectRoot = null;    //생성하는 이펙트 게임오브젝트의 부모 오브젝트
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //effectRoot 생성
            if (effectRoot == null)
            {
                effectRoot = new GameObject("Effect Root").transform;
                effectRoot.SetParent(this.transform);
            }

            //테스트
            //EffectOneShot((int)EffectList.EffectCube, new Vector3(-120, 1, 70));
            //EffectOneShot((int)EffectList.EffectSphere, new Vector3(-122, 1, 70));
        }
        #endregion

        #region Custom Method
        //이펙트 데이터 있는 이펙트를 불러와서 이펙트 생성하기
        public GameObject EffectOneShot(int index, Vector3 position)
        {
            EffectClip clip = DataManager.GetEffectData().GetClip(index);
            GameObject effectInstance = clip.Instantiate(position);
            effectInstance.SetActive(true);
            return effectInstance;
        }
        #endregion

    }
}