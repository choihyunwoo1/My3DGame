using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 플레이어 캐릭터를 관리하는 클래스
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        //참조
        protected PlayerInput m_Input;
        protected CharacterController m_CharCtrl;
        protected Animator m_Animator;

        //Move, Jump
        [SerializeField] protected float maxForwardSpeed = 8f;     //최대 이동 속도
        [SerializeField] protected float gravity = 20f;                            //중력 값
        [SerializeField] protected float minTurnSpeed = 400f;
        [SerializeField] protected float maxTurnSpeed = 1200f;

        protected bool m_IsGrounded = false;
        protected float m_ForwardSpeed;
        protected float m_VerticalSpeed;
        protected float m_DesiredForwardSpeed;

        //회전
        protected Quaternion m_TargetRotation;        //회전할 값
        protected float m_AngleDiff;                //

        //상수 값
        const float k_GroundAcceleration = 20f;     //가속도 값
        const float k_GroundDeceleration = 25f;     //감속도 값

        //Animator Parameters Hash값
        readonly int m_HashForwardSpeed = Animator.StringToHash("ForwardSpeed");
        readonly int m_HashAirbornVerticalSpeed = Animator.StringToHash("AirbornVerticalSpeed");
        readonly int m_HashAngleDelatRad = Animator.StringToHash("AngleDelatRad");
        readonly int m_HashInputDetected = Animator.StringToHash("InputDetected");
        readonly int m_HashTimeoutToIlde = Animator.StringToHash("TimeoutToIlde");
        readonly int m_HashGrounded = Animator.StringToHash("Grounded");

        //Animator State Hash값
        readonly int m_HashLocomotion = Animator.StringToHash("Locomotion");
        readonly int m_HashAirborn = Animator.StringToHash("Airborn");
        readonly int m_HashLanding = Animator.StringToHash("Landing");

        //Animator State Tag Hash값
        readonly int m_HashBlockInput = Animator.StringToHash("BlockInput");
        #endregion

        #region Property
        //이동 입력 값 체크
        protected bool IsMoveInput
        {
            get { return !Mathf.Approximately(m_Input.Movement.sqrMagnitude, 0f); }
        }
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            m_Input = GetComponent<PlayerInput>();
            m_CharCtrl = GetComponent<CharacterController>();
            m_Animator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            CalculateForwardMovement();

            SetTargetRotaion();
            if (IsMoveInput)
            {
                UpdateRoation();
            }

            TimeOutToIdle();
        }
        #endregion

        #region Custom Method
        //이동
        private void CalculateForwardMovement()
        {
            Vector2 moveInput = m_Input.Movement;
            //moveInput이 1이상이 안되도록 한다
            if (moveInput.sqrMagnitude > 1f)
            {
                moveInput.Normalize();
            }

            //입력에 의한 이동값 얻어오기
            m_DesiredForwardSpeed = moveInput.magnitude * maxForwardSpeed;

            //가속도값 얻어오기
            float accelation = IsMoveInput ? k_GroundAcceleration : k_GroundDeceleration;
            //이동 속도
            m_ForwardSpeed = Mathf.MoveTowards(m_ForwardSpeed, m_DesiredForwardSpeed, accelation * Time.deltaTime);

            //애니메이터 적용
            m_Animator.SetFloat(m_HashForwardSpeed, m_ForwardSpeed);
        }

        //회전 값 얻어오기
        private void SetTargetRotaion()
        {
            Vector2 moveInput = m_Input.Movement;
            Vector3 localMovementDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

            //입력 값에 의한 앞방향 구하기
            Vector3 forward = Quaternion.Euler(localMovementDirection.x, localMovementDirection.y, 
                localMovementDirection.z) * Vector3.forward;
            forward.y = 0f;
            forward.Normalize();

            Quaternion targetRotaion;
            if(Mathf.Approximately(Vector3.Dot(localMovementDirection, Vector3.forward), -1.0f))
            {
                targetRotaion = Quaternion.LookRotation(-forward);
            }
            else
            {
                Quaternion cameraToInputOffset = Quaternion.FromToRotation(Vector3.forward,
                    localMovementDirection);
                targetRotaion = Quaternion.LookRotation(cameraToInputOffset * forward);
            }

            //
            Vector3 resultForword = targetRotaion * Vector3.forward;

            float angleCurrent = Mathf.Atan2(transform.forward.x, transform.forward.z) * Mathf.Rad2Deg;
            float angleTarget = Mathf.Atan2(resultForword.x, resultForword.z) * Mathf.Rad2Deg;

            m_AngleDiff = Mathf.DeltaAngle(angleCurrent, angleTarget);
            m_TargetRotation = targetRotaion;
        }

        private void UpdateRoation()
        {
            m_Animator.SetFloat(m_HashAngleDelatRad, m_AngleDiff * Mathf.Deg2Rad);

            transform.rotation = m_TargetRotation;
        }


        //대기 동작 처리
        private void TimeOutToIdle()
        {
            bool inputDetected = IsMoveInput;


            //애니메이션 처리
            m_Animator.SetBool(m_HashInputDetected, inputDetected);
        }
        #endregion
    }
}
