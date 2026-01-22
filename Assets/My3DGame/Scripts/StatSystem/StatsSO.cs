using UnityEngine;
using System;

namespace My3DGame
{
    [CreateAssetMenu(fileName = "Stats Object", menuName = "Stat System/Stats Object")]
    public class StatsSO : ScriptableObject
    {
        #region Variables
        [SerializeField] private UserData userData;

        public Attribute[] attributes;      //캐릭터 속성 값들

        //스탯 변경시 등록되어 있는 함수를 호출하는 이벤트 함수
        public Action<StatsSO> OnChangedStats;

        //최초 1회 초기화 체크
        [NonSerialized]
        private bool inInitialized = false;
        #endregion

        #region Property
        public int Level
        {
            get => userData.level;
            set => userData.level = value;
        }

        public int Exp
        {
            get => userData.exp;
            set => userData.exp = value;
        }

        public int Health
        {
            get => userData.health;
            set => userData.health = value;
        }

        public int Mana
        {
            get => userData.mana;
            set => userData.mana = value;
        }

        public int Gold
        {
            get => userData.gold;
            set => userData.gold = value;
        }

        public int MaxHealth
        {
            get
            {
                int maxHealth = 0;
                foreach (var attribute in attributes)
                {
                    if (attribute.type == CharacterAttribute.Health)
                    {
                        maxHealth = attribute.value.ModifedValue;
                    }
                }
                return maxHealth;
            }
        }

        public int MaxMana
        {
            get
            {
                int maxMana = 0;
                foreach (var attribute in attributes)
                {
                    if (attribute.type == CharacterAttribute.Mana)
                    {
                        maxMana = attribute.value.ModifedValue;
                    }
                }
                return maxMana;
            }
        }
        #endregion

        #region Unity Event Method
        private void OnEnable()
        {
            //최초 1회만 실행
            InitializeAttributes();
        }
        #endregion

        #region Custom Method
        private void InitializeAttributes()
        {
            if(inInitialized)
                return;

            inInitialized = true;

            //Attributes 값 초기화
            foreach (var attribute in attributes)
            {
                //attribute의 value의 객체 생성
                attribute.value = new ModifiableInt(OnMoidifiedValue);
            }

            SetBaseValue(CharacterAttribute.Agility, 100);
            SetBaseValue(CharacterAttribute.Intellect, 100);
            SetBaseValue(CharacterAttribute.Stamina, 100);
            SetBaseValue(CharacterAttribute.Strength, 100);
            SetBaseValue(CharacterAttribute.Health, 100);
            SetBaseValue(CharacterAttribute.Mana, 100);

            Level = 1;
            Exp = 0;
            

        }

        //속성값 초기화
        public void SetBaseValue(CharacterAttribute type, int value)
        {
            foreach (var attribute in attributes)
            {
                if (attribute.type == type)
                {
                    attribute.value.BaseValue = value;
                }
            }
        }

        //속성 초기값 가져오기
        public int GetBaseValue(CharacterAttribute type)
        {
            foreach (var attribute in attributes)
            {
                if (attribute.type == type)
                {
                    return attribute.value.BaseValue;
                }
            }

            return -1;
        }

        //attribute value값이 변경되면 호출되는 함수
        private void OnMoidifiedValue(ModifiableInt value)
        {
            if(OnChangedStats != null)
                OnChangedStats.Invoke(this);
        }
        #endregion
    }
}