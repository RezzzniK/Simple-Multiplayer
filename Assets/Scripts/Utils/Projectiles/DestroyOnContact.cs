using UnityEngine;

public class DestroyOnContact : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
       
        if (!this.gameObject) return;
        Destroy(this.gameObject);
   }
}
