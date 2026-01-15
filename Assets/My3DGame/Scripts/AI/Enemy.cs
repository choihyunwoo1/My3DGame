using UnityEngine;

namespace My3DGame.AI
{
    /// <summary>
    /// 적을 관리하는 베이스 클래스, 모든 적들의 부모 클래스
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        #region Variables
        //참조
        protected DetectionModule m_DetectionMoudle;
        public MeleeWeapon meleeWeapon;

        //상태 머신
        protected StateMachine stateMachine;

        //공격 범위
        [SerializeField] protected float attackRange = 2.0f;
        //공격 딜레이 타임
        [SerializeField] protected float attackDelayTime = 1f;
        #endregion

        #region Property
        public Transform Target => m_DetectionMoudle.Target;
        public float AttackRange => attackRange;
        public float AttackDelayTime => attackDelayTime;
        //공격 가능 여부 체크
        public bool IsAttackable
        {
            get
            {
                if(Target)
                {
                    return (m_DetectionMoudle.DistanceToTarget <= attackRange);
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        #region Unity Event Method
        protected virtual void Awake()
        {
            //참조
            m_DetectionMoudle = GetComponent<DetectionModule>();
        }

        protected virtual void Start()
        {
            //상태머신 객체 생성 및 상태 생성해도 등록
            stateMachine = new StateMachine(this, new IdleState());
            stateMachine.RegisterState(new WalkState());
            stateMachine.RegisterState(new AttackState());
            stateMachine.RegisterState(new DeathState());
            //상속 받은 후 추가로 새로운 상태를 등록 진행

        }

        protected virtual void Update()
        {
            //상태머신의 업데이트 : 현재상태의 업데이트를 매 프레임마다 실행
            stateMachine.Update(Time.deltaTime);

        }
        #endregion

        #region Custom Method
        //상태 변경
        public State ChangeState(State newState)
        {
            return stateMachine.ChangeState(newState);
        }

        public void CheckDamage()
        {
            Debug.Log("CheckDamage");
        }

        public void MeleeAttackStart(int throwing = 0)
        {
            meleeWeapon.StartAttack(throwing != 0);
        }

        public void MeleeAttackEnd()
        {
            meleeWeapon.EndAttack();
        }
        #endregion
    }
}