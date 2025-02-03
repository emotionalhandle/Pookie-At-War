using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Transform targetToAttack;

    public Material idleStateMaterial;
    public Material attackStateMaterial;
    public Material followStateMaterial;
    public int unitDamage;
    public bool isPlayer;
    
    private void OnTriggerEnter(Collider other)

    {
        if (isPlayer && other.gameObject.CompareTag("Enemy") && targetToAttack == null)
        {
            targetToAttack = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && targetToAttack != null)
        {
            targetToAttack = null;
        }
    }

    public void SetIdleStateMaterial()
    {
        GetComponent<Renderer>().material = idleStateMaterial;
    }

    public void SetAttackStateMaterial()
    {
        GetComponent<Renderer>().material = attackStateMaterial;
    }

    public void SetFollowStateMaterial()
    {
        GetComponent<Renderer>().material = followStateMaterial;
    }

    private void OnDrawGizmos()
    {
        Animator animator = GetComponent<Animator>();
        if (animator == null) return;

        // Get the Attack State behaviour
        UnitAttackState attackState = animator.GetBehaviour<UnitAttackState>();
        if (attackState != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackState.stopAttackingDistance);
        }

        // Get the Follow State behaviour
        UnitFollowState followState = animator.GetBehaviour<UnitFollowState>();
        if (followState != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, followState.attackingDistance);
        }
    }
}
