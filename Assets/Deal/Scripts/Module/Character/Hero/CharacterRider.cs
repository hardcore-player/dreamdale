using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deal.Env;
using Deal.Data;
using Druid;

namespace Deal
{
    /// <summary>
    /// 角色的农场行为
    /// </summary>
    public class CharacterRider : MonoBehaviour
    {
        private Hero _hero;
        public Hero Hero { get => _hero; set => _hero = value; }

        public Transform playerNode;
        public Transform riderNode;
        public Transform riderFollowNode;

        public Animator riderAni;

        private bool _isRider = false;


        public void SetRider(bool isRider)
        {
            this._isRider = isRider;

            if (isRider)
            {
                this.riderNode.gameObject.SetActive(true);
            }
            else
            {
                playerNode.transform.localPosition = new Vector3(0, 0.493f, 0);
                this.riderNode.gameObject.SetActive(false);
            }

        }


        private void Update()
        {
            if (this._isRider)
            {
                this.playerNode.transform.position = riderFollowNode.transform.position;

                if (Hero.Controller.IsIdle())
                {
                    riderAni.Play("ani_ride_dragon_idle", 0);
                }
                else
                {
                    riderAni.Play("ani_ride_dragon_run", 0);
                }
            }
        }

    }

}
