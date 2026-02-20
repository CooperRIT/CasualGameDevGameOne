using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerable
{
    public void OnTrigger();
}

public class OnTriggerButton : MonoBehaviour, ITriggerable
{
    [SerializeField] private GameObject associatedGate;
    bool triggered;
    public void OnTrigger()
    {
        if(triggered)
        {
            return;
        }
        triggered = true;
        PlayerSpawnManager.Instance.DisableGateClientRpc(associatedGate.name);
    }
}
