using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace My3DGame.UI
{
    /// <summary>
    /// 플레이 화면의 플레이데이터 UI를 관리하는 클래스
    /// </summary>
    public class PlayerDataUI : MonoBehaviour
    {
        #region Variables
        public StatsSO statsObejct;

        //UI
        public Image healthBar;
        public Image manaBar;

        public TextMeshProUGUI levelText;
        public TextMeshProUGUI expText;
        public TextMeshProUGUI goldText;
        #endregion

        #region Unity Event Method
        private void OnEnable()
        {
            statsObejct.OnChangedStats += OnChangedStats;

            //UI Text 적용
            UpdatePlayerData();
        }

        private void OnDisable()
        {
            statsObejct.OnChangedStats -= OnChangedStats;
        }
        #endregion

        #region Custom Method
        private void UpdatePlayerData()
        {
            healthBar.fillAmount = statsObejct.HealthRaito;
            manaBar.fillAmount = statsObejct.ManaRaito;

            levelText.text = statsObejct.Level.ToString();
            int needForLevelup = statsObejct.GetExpForLevelup(statsObejct.Level);
            expText.text = statsObejct.Exp.ToString() + "/" + needForLevelup.ToString();
            goldText.text = statsObejct.Gold.ToString();
        }

        private void OnChangedStats()
        {
            UpdatePlayerData();
        }
        #endregion
    }
}