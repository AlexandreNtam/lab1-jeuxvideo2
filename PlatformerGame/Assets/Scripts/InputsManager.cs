using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InputsManager : MonoBehaviour
{
    private InputField inputField;
    private Text playerNameText;
    private Text placeHolderText;
    private Toggle toggle;
    private Slider slider;

    private InputAction submitAction;

    void Start()
    {
        //Si ce script pourrait être utilisé dans d'autres contextes et que les noms ne seraient pas 
        //nécessairement ceux-ci alors des serialized field pourraient être mieux.
        inputField = GameObject.Find("InputField").GetComponent<InputField>();
        playerNameText = GameObject.Find("InputField").transform.GetChild(1).GetComponent<Text>();
        placeHolderText = GameObject.Find("InputField").transform.GetChild(0).GetComponent<Text>();
        toggle = GameObject.Find("Toggle").GetComponent<Toggle>();
        slider = GameObject.Find("Slider").GetComponent<Slider>();

        submitAction = InputSystem.actions.FindAction("Submit");
    }

    void Update()
    {
        if (submitAction.WasPressedThisFrame())
        {
            LaunchGame();
        }
    }

    public void ButtonPressed()
    {
        Debug.Log("Le bouton a été appuyé.  La valeur du slider est de " + slider.value + " et le toggle est à " + toggle.isOn.ToString());
        LaunchGame();
    }

    public void ToggleToggled()
    {
        Debug.Log("Le toggle est maintenant à " + toggle.isOn.ToString());
    }

    public void SliderSlided(float value)
    {
        Debug.Log("Le slider est maintenant à " + value.ToString());
    }

    public void LaunchGame()
    {
        if (playerNameText.text.Length < 3)
        {
            inputField.Select();
            inputField.text = "";
            placeHolderText.text = "Le nom doit avoir au moins 3 caractères";
        }
        else
        {
            GameManager.Instance.SetPlayerName(playerNameText.text);
            GameManager.Instance.StartNextlevel(0.2f);
        }
    }
}