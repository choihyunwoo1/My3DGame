using UnityEngine;
using UnityEngine.Events;
using System.Xml;
using System.Collections.Generic;

namespace My3DGame
{
    /// <summary>
    /// 대화를 관리하는 클래스
    /// </summary>
    public class DialogueManager : MonoBehaviour
    {
        #region Variables
        //대화 데이터
        [SerializeField] private string path = "Dialogue/Dialog";       //xml 파일 경로
        private XmlNodeList allNodes;                                   //xml 데이터

        private Queue<Dialog> _currentDialogs;                          //현재 진행하는 대화

        private int _next;                                              //다음 대화 진행

        //이벤트 함수
        public UnityAction<Dialog> openUIDialogEvent;
        public UnityAction closeUIDialogEvent;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //데이터 읽어오기
            LoadDialogXml();

            //데이터 초기화
            _currentDialogs = new Queue<Dialog>();
            InitDialogue();
        }
        #endregion

        #region Custom Method
        private void LoadDialogXml()
        {
            TextAsset asset = ResourcesManager.Load<TextAsset>(path);

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(asset.text);
            allNodes = xmlDocument.SelectNodes("root/dialog");
        }

        //다이알로드 초기화
        private void InitDialogue()
        {
            _currentDialogs.Clear();

            _next = -1;
        }

        //매개변수로 받은 대화 시작하기
        public void StartDialogue(int dialgoIndex)
        {
            //다이알로그 초기화
            InitDialogue();

            //_currentDialogs 셋팅
            foreach (XmlNode node in allNodes)
            {
                int num = int.Parse(node["number"].InnerText);
                if(num == dialgoIndex)
                {
                    Dialog dialog = new Dialog();
                    dialog.number = num;
                    dialog.character = int.Parse(node["character"].InnerText);
                    dialog.name = node["name"].InnerText;
                    dialog.sentence = node["sentence"].InnerText;
                    dialog.next = int.Parse(node["next"].InnerText);

                    _currentDialogs.Enqueue(dialog);    //큐에 대화 넣기
                }
            }

            //대화창 보여주기
            DisplayDialogueData();
        }

        //큐에서 현재 대화 꺼내어 보여준다
        public void DisplayDialogueData()
        {
            //_currentDialogs 체크
            if (_currentDialogs.Count <= 0)
            {
                if (_next >= 0)
                {
                    StartDialogue(_next);
                }
                else
                { 
                    DialogueEndAndCloseDialogueUI();
                }
                return;
            }

            //큐에서 현재 대화 꺼내기
            Dialog dialog = _currentDialogs.Dequeue();
            _next = dialog.next;

            DisplayDialogueLine(dialog);
        }

        //매개변수로 받은 대화 보여주기
        private void DisplayDialogueLine(Dialog dialog)
        {
            if(openUIDialogEvent != null)
            {
                openUIDialogEvent.Invoke(dialog);
            }
        }

        //대화 종료
        private void DialogueEndAndCloseDialogueUI()
        {
            //대화 초기화
            InitDialogue();

            //UI 닫기
            if (closeUIDialogEvent != null)
            { 
                closeUIDialogEvent.Invoke();
            }
        }
        #endregion
    }
}