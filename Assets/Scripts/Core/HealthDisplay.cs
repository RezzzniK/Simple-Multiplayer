using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : NetworkBehaviour
{
    //we will need refs for health and Image that is displaying our health:
    [SerializeField] Image healthDisplay;
    [SerializeField] Health healthObj;

    //lets override Network Spawn:
    public override void OnNetworkSpawn()
    {
        if (!IsClient) return;
        healthObj.CurrentHealth.OnValueChanged += HandleHealthChange;
        HandleHealthChange(0,healthObj.CurrentHealth.Value);//in case if health is changed before we subscribed
    }
    public override void OnNetworkDespawn()
    {
        if (!IsClient) return;
        healthObj.CurrentHealth.OnValueChanged -= HandleHealthChange;
    }

    private void HandleHealthChange(int oldHealth, int newHealth)
    {
        healthDisplay.fillAmount =(float)newHealth/ healthObj.MaxHealth ;
    }
    
}
