using UnityEngine;
using UnityEngine.AI;

public class UnitFollowState : StateMachineBehaviour
{
    AttackController attackController;
    NavMeshAgent agent;
    public float attackingDistance = 1f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackController = animator.GetComponent<AttackController>();
        agent = animator.GetComponent<NavMeshAgent>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (attackController.targetToAttack == null)
        {
            // --- Transition to Idle State --- //
            animator.SetBool("isFollowing", false);
            return;
        }

        // --- Move towards the target --- //
        agent.SetDestination(attackController.targetToAttack.position);
        animator.transform.LookAt(attackController.targetToAttack);

        // --- Check if the target is within attacking distance --- //
        float distance = Vector3.Distance(animator.transform.position, attackController.targetToAttack.position);
        if (distance <= attackingDistance)
        {
            // --- Transition to Attack State --- //
            animator.SetBool("isAttacking", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position);
    }
}
