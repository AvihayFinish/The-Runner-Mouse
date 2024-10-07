using UnityEngine;

public class AmadeoClient_2 : MonoBehaviour
{
    [SerializeField] private AmadeoClient amadeoClient;  // Reference to the AmadeoClient script

    public void start()
    {
        amadeoClient.StartReceiveData();
    }
}