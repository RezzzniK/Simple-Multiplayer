using Unity.Netcode;
using UnityEngine;

public class PlayerAiming : NetworkBehaviour//executing the code only if you a an owner
{
    [SerializeField] Transform turret;
    [SerializeField] InputReaderSO inputReaderSO;


    //we will check every frame where is mouse pointer relative to where the turret is,
    // and then based on that, we'll rotate the turret to aim at the cursor
    /* Also we going to use late Update
        Why?
        Specifically here we got interpolated movement on the tank( smoothly reconstruct the gap between the movement which is dropped
        by frame change), its probably best to use late update so that the tank moves and then this(aiming) happens and 
        then when this happens, we'll rotate to face cursor.
        Otherwise you might get some weird jitter, you might have the barrel be slightly off from where the cursor is.
    */
    void LateUpdate()
    {
        if (!IsOwner) return;
        Vector2 aimCursorPos=inputReaderSO.aimPosition;//getting cursor pos
        Vector2 aimToGameWorldPos=Camera.main.ScreenToWorldPoint(aimCursorPos);
        turret.up=new Vector2(aimToGameWorldPos.x-turret.position.x,aimToGameWorldPos.y-turret.position.y);
    }
}
