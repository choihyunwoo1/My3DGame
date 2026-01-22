using UnityEngine;
using System;
using System.Collections.Generic;

namespace My3DGame
{
    /// <summary>
    /// 캐릭터 속성 value(값)을 관리하는 클래스
    /// </summary>
    [Serializable]
    public class ModifiableInt
    {
        #region Variables
        [NonSerialized] private int baseValue;      //기본(초기) 값
        [SerializeField] private int modifedValue;   //수정된 값, 최종값

        //modifedValue 값 변경시 등록된 함수 실행
        private event Action<ModifiableInt> _OnModifedValue;

        //modifedValue 값 계산시 추가할 ItemBuff 값들을 저장한 리스트
        private List<IModifier> modifiers = new List<IModifier>();
        #endregion

        #region Property
        public int BaseValue
        {
            get { return baseValue; }
            set {
                baseValue = value;
                UpdateModifedValue();
            }
        }

        public int ModifedValue
        {
            get { return modifedValue; }
            set { modifedValue = value; }
        }
        #endregion

        #region Constructor
        public ModifiableInt(Action<ModifiableInt> method = null)
        {
            modifedValue = baseValue;
            RegisterModEvent(method);
        }
        #endregion

        #region Custom Method
        //Action<ModifiableInt> 함수 등록
        public void RegisterModEvent(Action<ModifiableInt> method)
        { 
            if(method != null)
            {
                _OnModifedValue += method;
            }
        }

        //Action<ModifiableInt> 함수 해제
        public void UnRegisterModEvent(Action<ModifiableInt> method)
        {
            if (method != null)
            {
                _OnModifedValue -= method;
            }
        }

        //modifedValue 값 구하기, 값 변경시 등록된 함수 호출
        private void UpdateModifedValue()
        {
            int valueToAdd = 0;
            foreach(var modifier in modifiers)
            {
                modifier.AddValue(ref valueToAdd);
            }
            modifedValue = baseValue + valueToAdd;

            if(_OnModifedValue != null)
            {
                _OnModifedValue.Invoke(this);
            }
        }

        //ItemBuff 값들을 저장한 리스트에 추가
        public void AddModifier(IModifier modifier)
        {
            modifiers.Add(modifier);
            UpdateModifedValue();
        }


        //ItemBuff 값들을 저장한 리스트에서 제거
        public void RemoveModifier(IModifier modifier)
        {
            modifiers.Remove(modifier);
            UpdateModifedValue();
        }
        #endregion
    }
}
