using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerLinker : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.LinkText(GetComponent<Text>());
    }

    void Update() {}
}
