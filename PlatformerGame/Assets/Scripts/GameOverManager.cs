using UnityEngine;
using UnityEngine.InputSystem;

public class GameOverManager : MonoBehaviour
{
    private InputAction submitAction;

    void Start()
    {
        submitAction = InputSystem.actions.FindAction("Submit");
    }

    void Update()
    {
        if (submitAction.WasPressedThisFrame())
            GameManager.Instance.ResetGame();
    }
}