using UnityEngine;

namespace States
{
    public class IdleState : State
    {
        private static readonly int IdleHash = Animator.StringToHash("Idle");

        protected override void Tick()
        {
        }

        public override void OnEnter() => Animator.SetBool(IdleHash, true);


        public override void OnExit() => Animator.SetBool(IdleHash, false);

        public IdleState(Animator animator) : base(animator)
        {
        }
    }
}