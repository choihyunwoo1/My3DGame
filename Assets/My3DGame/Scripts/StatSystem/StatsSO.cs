using UnityEngine;
using System;
using System.IO;

namespace My3DGame
{
    /// <summary>
    /// 캐릭터 속성 값들을 저장하고 관리하는 스크립터블 오브젝트
    /// </summary>
    [CreateAssetMenu(fileName = "Stats Object", menuName = "Stat System/Stats Object")]
    public class StatsSO : ScriptableObject
    {
        #region Variables
        [SerializeField] private UserData userData;

        public Attribute[] attributes;      //캐릭터 속성 값들

        //스탯 변경시 등록되어 있는 함수를 호출하는 이벤트 함수
        public Action OnChangedStats;

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

        public float HealthRaito => Health / (float)MaxHealth;
        public float ManaRaito => Mana / (float)MaxMana;
        #endregion

        #region Unity Event Method
        private void OnEnable()
        {
            //최초 1회만 실행
            InitializeAttributes();
        }
        #endregion

        #region Custom Attributes Method
        private void InitializeAttributes()
        {
            if(inInitialized)
                return;

            inInitialized = true;

            attributes = new Attribute[6];
            for (int i = 0; i < attributes.Length; i++)
            {
                CharacterAttribute type = (CharacterAttribute)i;
                attributes[i] = new Attribute(type);
                //attribute의 value의 객체 생성
                attributes[i].value = new ModifiableInt(OnMoidifiedValue);
            }

            SetBaseValue(CharacterAttribute.Agility, 100);
            SetBaseValue(CharacterAttribute.Intellect, 100);
            SetBaseValue(CharacterAttribute.Stamina, 100);
            SetBaseValue(CharacterAttribute.Strength, 100);
            SetBaseValue(CharacterAttribute.Health, 100);
            SetBaseValue(CharacterAttribute.Mana, 100);

            Level = 1;
            Exp = 0;
            Gold = 1000;
            Health = MaxHealth;
            Mana = MaxMana;
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

        //최종 속성값 가져오기
        public int GetModifiredValue(CharacterAttribute type)
        {
            foreach (var attribute in attributes)
            {
                if (attribute.type == type)
                {
                    return attribute.value.ModifedValue;
                }
            }

            return -1;
        }

        //attribute value값이 변경되면 호출되는 함수
        private void OnMoidifiedValue(ModifiableInt value)
        {
            if(OnChangedStats != null)
                OnChangedStats.Invoke();
        }
        #endregion

        #region Custom UserData Method
        public void AddGold(int amount)
        {
            Gold += amount;

            //스탯 변경시 등록된 함수 호출
            if(OnChangedStats != null)
                OnChangedStats.Invoke();
        }

        public bool UseGold(int amount)
        {
            if (Gold < amount)
                return false;

            Gold -= amount;

            //스탯 변경시 등록된 함수 호출
            if (OnChangedStats != null)
                OnChangedStats.Invoke();

            return true;
        }

        public bool EnoughGold(int amount)
        {
            return Gold >= amount;
        }

        //경험치 추가 및 레벨업 적용
        public bool AddExp(int amount)
        {
            bool isLevelup = false;

            Exp += amount;

            int nowLevel = Level;
            //레벨업 체크
            if(Exp >= GetExpForLevelup(nowLevel))
            {
                Exp -= GetExpForLevelup(nowLevel);

                Level++;
                isLevelup = true;

                //레벨업 보상 처리
            }

            return isLevelup;
        }

        //매개변수로 들어온 레벨에서 다음 레벨업에 필요한 경험치량 구하기
        public int GetExpForLevelup(int _level)
        {
            //경험치 공식 또는 경험치 테이블에서 데이터 가져오기
            return _level * 100;
        }

        public void SetCurrentHealth(int _health)
        {
            Health = _health;

            //스탯 변경시 등록된 함수 호출
            if (OnChangedStats != null)
                OnChangedStats.Invoke();
        }
        #endregion

        //인벤토리 데이터를 json파일 저장하기, 로드하기
        #region Save/Load Methods
        public string filePath = "/UserData.json";

        [ContextMenu("Save")]
        public void Save()
        {
            //디바이스 저장 경로
            string path = Application.persistentDataPath + filePath;
            string jsonOutput = JsonUtility.ToJson(userData, true);
            Debug.Log(jsonOutput);
            File.WriteAllText(path, jsonOutput);
        }

        [ContextMenu("Load")]
        public void Load()
        {
            //디바이스 저장 경로
            string path = Application.persistentDataPath + filePath;
            if (File.Exists(path))
            {
                string jsonInput = File.ReadAllText(path);
                JsonUtility.FromJsonOverwrite(jsonInput, userData);
            }
        }

        //인벤토리 비우기
        [ContextMenu("Clear")]
        public void Clear()
        {
            Level = 1;
            Exp = 0;
            Gold = 1000;
            Health = MaxHealth;
            Mana = MaxMana;
        }
        #endregion
    }
}