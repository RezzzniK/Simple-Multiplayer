using Unity.Netcode.Components;
using UnityEngine;

public class ClientNetworkTransform : NetworkTransform//MonoBehaviour
{
    //No need in Unity 6:
    // public override void OnNetworkSpawn()
    // {
    //     base.OnNetworkSpawn();
    //     //ooverride part :
    //     CanCommitToTransform = IsOwner;//so when we spawn our player, we assign our ownership
    //                                    //to whoever they belong to(client to client)
    //                                   //so if this our player we can commit by ourself its transform(update pos/rotation and scale)
    // }
  
    protected override bool OnIsServerAuthoritative()
    {
        return false;//so now whenever in buil it code for the network transform,
                     //they check whether it's server authoritative, it will return false
    }
}
