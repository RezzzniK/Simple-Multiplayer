using Unity.Netcode;
using UnityEngine;

public class JoinServer : MonoBehaviour
{
    public void Join()
    {
        //SO WHEN WE WILL PRESS BUTTON
        //IT WILL CONNECT TO HOST ANOTHER CLIENT
        NetworkManager.Singleton/*is the network manager in our scene*/.StartClient();
    }
}
