using UnityEngine;
using UnityEngine.AI;

namespace States
{
    public class FleeState : State
    {
        private static readonly int FleeHash = Animator.StringToHash("Flee");

        private const float RANGE = 10.0f;

        private static bool RandomPoint(Vector3 center, float range, out Vector3 result)
        {
            for (var i = 0; i < 30; i++)
            {
                var randomPoint = center + Random.insideUnitSphere * range;
                if (!NavMesh.SamplePosition(randomPoint, out var hit, 1.0f, NavMesh.AllAreas))
                    continue;
                result = hit.position;
                return true;
            }

            result = Vector3.zero;
            return false;
        }

        protected override void Tick()
        {
            if (NavMeshAgent.remainingDistance >= 2f) return;
            if (!RandomPoint(NpcTransform.position, RANGE, out var point)) return;

            Debug.DrawRay(point, 2 * Vector3.up, Color.blue, 1.0f);
            NavMeshAgent.SetDestination(point);
        }

        public override void OnEnter()
        {
            // NavMeshAgent.enabled = true;
            NavMeshAgent.isStopped = false;
            Animator.SetBool(FleeHash, true);
        }

        public override void OnExit()
        {
            // NavMeshAgent.enabled = false;
            NavMeshAgent.isStopped = true;
            Animator.SetBool(FleeHash, false);
        }

        public FleeState(Animator animator, NavMeshAgent navMeshAgent, Transform transform) : base(animator,
            navMeshAgent, transform)
        {
        }
    }
}