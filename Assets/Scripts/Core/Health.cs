using System;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>();
    [field: SerializeField] public int MaxHealth { get; private set; } = 100;//we want this to be availible to get but set must be private
    private bool isDead;//default false
    public Action<Health> OnDie;//event that will be triggered when player dies, and who is listenting for this event will now 
                                // exactly which player is died
                                //next we check if we are the server to modify the currentHealth
    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        CurrentHealth.Value = MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        ModifyHealth(-damage);
    }

    public void RestoreHealth(int restoreValue)
    {
        ModifyHealth(restoreValue);

    }

    private void ModifyHealth(int modifyValue)
    {
        if(isDead) return;
        // if (!IsServer) return;
        CurrentHealth.Value = Mathf.Clamp(CurrentHealth.Value + modifyValue, 0, MaxHealth);
        if (CurrentHealth.Value == 0)
        {
            OnDie?.Invoke(this);
            isDead=true;
        }
        
    }
}
