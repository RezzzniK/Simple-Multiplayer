using System;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] InputReaderSO testreader;
    void Start()
    {
        //first lets subscribe to event:
        testreader.MovingEvent += HandleMove;
    }

    private void HandleMove(Vector2 movement)
    {
        Debug.Log("MOVING EVENT:" + movement);
    }

    void OnDestroy()
    {
        //lets also unsubscribe from event:
        testreader.MovingEvent -= HandleMove;
    }


}
