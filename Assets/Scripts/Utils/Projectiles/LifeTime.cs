using UnityEngine;

public class LifeTime : MonoBehaviour
{
    [SerializeField] float lifeTime = 1f;


    void OnEnable()
    {
       Destroy(gameObject, lifeTime);
    }

   
}
