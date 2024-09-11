using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Druid;
using Deal.Env;

namespace Deal
{
    public enum CharacterState
    {
        Idle, Moving, Pause
    }

    /// <summary>
    /// 角色控制器
    /// </summary>
    public class CharacterController : MonoBehaviour
    {
        private Hero _hero;
        public Hero Hero { get => _hero; set => _hero = value; }

        [Tooltip("速度")]
        public float speed;
        public VariableJoystick variableJoystick;
        public Rigidbody2D rigidbody2d;

        private CharacterState _moveState = CharacterState.Idle;
        private Vector2 _nextMoveCommand;

        // 动画时间
        private float _aniInterval = 0.0f;
        private float _aniSpeed = 1.0f;
        private bool _aniPlay = false;

        private float _stepImterval = 0;


        public void PlayAnimation(WorkshopToolEnum tool)
        {
            this._aniPlay = true;
            this._aniInterval = 0;

            float inverval = 0.5f;
            if (tool == WorkshopToolEnum.AttackWeapon)
            {
                inverval = 0.5f;
            }
            else if (tool == WorkshopToolEnum.FishingRod)
            {
                inverval = 0.5f;
            }
            this._aniSpeed = inverval;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsIdleNoAni()
        {
            return this._aniPlay == false && this._moveState == CharacterState.Idle;
        }


        public CharacterState GetMoveState()
        {
            return this._moveState;
        }


        public bool IsInAni()
        {
            return this._aniPlay == true;
        }

        public bool IsPause()
        {
            return this._moveState == CharacterState.Pause;
        }

        public bool IsIdle()
        {
            return this._moveState == CharacterState.Idle;
        }


        public void SetStatePause()
        {
            this._nextMoveCommand = new Vector2(0, 0);
            this._moveState = CharacterState.Pause;
        }

        public void SetStateIdle()
        {
            this._aniPlay = false;
            this._nextMoveCommand = new Vector2(0, 0);
            this._moveState = CharacterState.Idle;
        }

        void Update()
        {
            UpdateMoveState();
            UpdateAnimation();
        }

        void UpdateMoveState()
        {
            if (this._moveState == CharacterState.Pause) return;

            this._nextMoveCommand = new Vector2(0, 0);

            if (variableJoystick == null)
            {
                return;
            }

            float horizontal, vertical;
            if (variableJoystick.Background.gameObject.activeInHierarchy == true)
            {
                // 摇杆
                Vector2 move = variableJoystick.Direction.normalized;
                horizontal = move.x;
                vertical = move.y;

                //Debug.Log("horizontal" + horizontal);
                //Debug.Log("vertical" + vertical);
            }
            else
            {
                // 电脑
                horizontal = Input.GetAxis("Horizontal");
                vertical = Input.GetAxis("Vertical");
            }

            this._nextMoveCommand = new Vector2(horizontal, vertical);

            this._moveState = this._nextMoveCommand == Vector2.zero ? CharacterState.Idle : CharacterState.Moving;


            this.Hero.LookDir(this._nextMoveCommand);
        }


        void FixedUpdate()
        {
            Vector2 position = rigidbody2d.position;
            position = position + _nextMoveCommand * speed * Time.deltaTime;
            rigidbody2d.MovePosition(position);
        }


        void UpdateAnimation()
        {

            if (this._aniPlay == true)
            {
                this._aniInterval += Time.deltaTime;
                if (this._aniInterval >= this._aniSpeed)
                {
                    this._aniPlay = false;
                }
            }
            else
            {
                if (this._moveState == CharacterState.Idle)
                {
                    this.Hero.PlayIdle();
                }
                else if (this._moveState == CharacterState.Moving)
                {
                    this.Hero.PlayRun();

                    this._stepImterval += Time.deltaTime;

                    if (this._stepImterval >= 0.4f)
                    {
                        this._stepImterval = 0;
                        SoundManager.I.playEffect(AddressbalePathEnum.WAV_step_single);

                    }
                }
            }
        }
    }
}
