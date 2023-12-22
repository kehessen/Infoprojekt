using UnityEngine;

namespace Entities
{
    public abstract class Entity : MonoBehaviour
    {
        protected readonly Inventory Inventory = new();
        private bool IsInvincible { get; }
        private float Health { get; set; }
        private float MaxHealth { get; }

        private void Start()
        {
            Health = MaxHealth;
            // TODO: spawn animation
        }

        private static void OnDeath()
        {
            // implement in child class
        }

        public void OnDespawn()
        {
            // TODO: despawn behaviour
        }

        private void Die()
        {
            Health = 0;
            OnDeath();
        }

        public void Heal(float amount)
        {
            Health += amount;
            if (Health > MaxHealth) Health = MaxHealth;
        }

        public void TakeDamage(float amount)
        {
            if (IsInvincible) return;
            // TODO: damage animation

            Health -= amount;
            if (Health <= 0) Die();
        }
    }
}