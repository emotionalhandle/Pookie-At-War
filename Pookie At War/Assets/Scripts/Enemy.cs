using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    internal void ReceiveDamage(int damage)
    {
        health -= damage;
    }
}
