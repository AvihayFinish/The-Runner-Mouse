using UnityEngine;

public class AmadeoClient_2 : MonoBehaviour
{
    [SerializeField] private AmadeoClient amadeoClient;  // Reference to the AmadeoClient script
    private InputMover im;  // Reference to the PlayerMovement script

    public void start()
    {
        im = GetComponent<InputMover>();  // Get the PlayerMovement script component

        AmadeoClient.Instance.StartReceiveData();
    }
}