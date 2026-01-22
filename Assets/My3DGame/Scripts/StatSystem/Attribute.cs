using UnityEngine;
using System;

namespace My3DGame
{
    /// <summary>
    /// 캐릭터의 속성 값을 관리하는 클래스
    /// 속성: 속성 타입, 값
    /// </summary>
    [Serializable]
    public class Attribute
    {
        public CharacterAttribute type;
        public ModifiableInt value;
    }
}
