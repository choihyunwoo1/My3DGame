using System;

namespace My3DGame
{
    /// <summary>
    /// 대화 데이터 모델 클래스
    /// </summary>
    [Serializable]
    public class Dialog
    {
        public int number;
        public int character;
        public string name;
        public string sentence;
        public DialogType type;
    }
}