using DG.Tweening;
using Druid;
using UnityEngine;

namespace Deal.UI
{
    public class UIJoystick : UIBase
    {
        public VariableJoystick joystick;
        public GameObject goGuide;


        private void Start()
        {
            joystick.StartAction += OnStartAction;
            joystick.PointerDownAction += OnPointerDownAction;
            joystick.PointerUpAction += OnPointerUpAction;

            this.goGuide.SetActive(PlayManager.I.firstLoad);
        }

        private void OnStartAction()
        {
        }

        private void OnPointerDownAction()
        {
            if (this.goGuide.activeInHierarchy)
            {
                this.goGuide.SetActive(false);

                Sequence sequence = DOTween.Sequence();
                sequence.AppendInterval(1);
                sequence.AppendCallback(() =>
                {
                    TaskManager.I.LookAtGuideTarget(0, 0, false);
                    this.joystick.Background.gameObject.SetActive(false);
                });

                PlayManager.I.firstLoad = false;
            }
        }

        private void OnPointerUpAction()
        {

        }

    }

}
