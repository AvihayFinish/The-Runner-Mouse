using UnityEngine;

public class AmadeoClient_1 : MonoBehaviour
{
    [SerializeField] private AmadeoClient amadeoClient;  // Reference to the AmadeoClient script

    public void start()
    {
        AmadeoClient.Instance.StartZeroF();
    }
}