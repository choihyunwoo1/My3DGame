using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 게임에서 사용하는 다이알로그 들을 관리하는 스크립터블 오브젝트 클래스
    /// </summary>
    [CreateAssetMenu(fileName = "Dialog Object", menuName = "Dialog System/Dialog")]
    public class DialogSO : ScriptableObject
    {
        #region Variables
        public List<Dialog> database;

        //데이터 경로
        public string dataPath = "Data/DialogData";
        #endregion

        #region Unity Event Method
        /*private void OnEnable()
        {
            //대화 데이터 읽어오기
            Load();
        }*/
        #endregion

        #region Custom Method
        [ContextMenu("Load")]
        public void Load()
        {
            TextAsset asset = ResourcesManager.Load<TextAsset>(dataPath);
            if(asset == null || asset.text == null)
            {
                Debug.Log("Not found Data File");
                return;
            }

            using (XmlTextReader reader = new XmlTextReader(new StringReader(asset.text)))
            {
                var xs = new XmlSerializer(typeof(List<Dialog>));
                database = (List<Dialog>)xs.Deserialize(reader);
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