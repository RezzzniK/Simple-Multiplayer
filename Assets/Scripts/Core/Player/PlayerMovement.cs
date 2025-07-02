using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] InputReaderSO inputReaderSO;
    [SerializeField] Transform tracks;
    [SerializeField] Transform body;
    [SerializeField] Rigidbody2D rigidbody2D;
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float rotationSpeed = 8f;
    Vector2 perviousMovementInput;
   
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        inputReaderSO.MovingEvent += HandleMove;
       
    }

    

    void Update()
    {
        if (!IsOwner) return;
        TracksAndBodyMovement();
    }

    private void TracksAndBodyMovement()
    {
        tracks.Rotate(0f, 0f, perviousMovementInput.x * (-rotationSpeed) * Time.deltaTime);//by pressing a/d we are getting +- turning vector

        body.Rotate(0f, 0f, perviousMovementInput.x * (-rotationSpeed) * Time.deltaTime);
    }

    void FixedUpdate()
    {
        if (!IsOwner) return;
        rigidbody2D.linearVelocity = (Vector2)tracks.transform.up * perviousMovementInput.y * moveSpeed;//because we in fixed upd no need to multiply
                                                                                                        //by Time.fixedDeltaTime
    }
    void HandleMove(Vector2 vector)
    {
       // Debug.Log("" + vector);
        perviousMovementInput=vector;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;
        inputReaderSO.MovingEvent -= HandleMove;
    }
}
