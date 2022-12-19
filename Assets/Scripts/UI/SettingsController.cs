using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    private Slider _slider;

    [SerializeField]
    private TMP_Text _textField;
    // Start is called before the first frame update
    void Start()
    {
        _slider = GetComponentInChildren<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        SetValueInTextField(Mathf.FloorToInt(_slider.value));
    }

    void SetValueInTextField(int value)
    {
        _textField.text = value.ToString();
    }
    
}
