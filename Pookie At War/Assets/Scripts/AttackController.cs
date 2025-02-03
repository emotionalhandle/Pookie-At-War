using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Transform targetToAttack;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && targetToAttack == null)
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

}
