using UnityEngine;
using UnityEngine.AI;

public class UnitAttackState : StateMachineBehaviour
{
    NavMeshAgent agent;
    AttackController attackController;
    public float stopAttackingDistance = 2f;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        attackController = animator.GetComponent<AttackController>();
        attackController.SetAttackStateMaterial();
    }


    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (attackController.targetToAttack != null && animator.GetComponent<UnitMovement>().isCommandedToMove == false)
        {
            LookAtTarget(animator);
            // Keep moving towards the target
            agent.SetDestination(attackController.targetToAttack.position);

            float distance = Vector3.Distance(animator.transform.position, attackController.targetToAttack.position);
            if (distance > stopAttackingDistance || attackController.targetToAttack == null)
            {
                // Transition to Follow State
                animator.SetBool("isAttacking", false);
                Debug.Log("Transitioning to Attack State");
            }
        }
    }

    public void LookAtTarget(Animator animator)
    {
        animator.transform.LookAt(attackController.targetToAttack);
    }
    
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

}
