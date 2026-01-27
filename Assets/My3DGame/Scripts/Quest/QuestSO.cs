using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 모든 퀘스트 데이터를 저장하는 데이터베이스 스크립터블 오브젝트 클래스
    /// </summary>
    [CreateAssetMenu(fileName = "Quest Object", menuName = "Quest System/Quest")]
    public class QuestSO : ScriptableObject
    {
        #region Variables
        public List<QuestData> database;        //모든 퀘스트 데이터 목록 리스트

        //데이터 경로
        public string dataPath = "Data/QuestData";
        #endregion


        #region Custom Method
        [ContextMenu("Load")]
        public void Load()
        {
            TextAsset asset = ResourcesManager.Load<TextAsset>(dataPath);
            if (asset == null || asset.text == null)
            {
                Debug.Log("Not found Data File");
                return;
            }

            using (XmlTextReader reader = new XmlTextReader(new StringReader(asset.text)))
            {
                var xs = new XmlSerializer(typeof(List<QuestData>));
                database = (List<QuestData>)xs.Deserialize(reader);
            }
        }

        [ContextMenu("Clear")]
        public void Clear()
        {
            database.Clear();
        }
        #endregion
    }
}