using UnityEngine;

namespace States
{
    public class MoveState : State
    {
        private static readonly int MoveHash = Animator.StringToHash("Move");

        protected override void Tick()
        {
        }

        public override void OnEnter() => Animator.SetBool(MoveHash, true);

        public override void OnExit() => Animator.SetBool(MoveHash, false);

        public MoveState(Animator animator) : base(animator)
        {
        }
    }
}