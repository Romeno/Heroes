using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class PositiveIntegerInputField : MonoBehaviour
{
    void Start()
    {
        GetComponent<InputField>().onValidateInput += (string input, int charIndex, char ch) =>
        {
            if (input == "0")
            {
                return '\0';
            }

            if (ch >= '0' && ch <= '9')
            {
                return ch;
            }
            else
            {
                return '\0';
            }
        };
    }

    void Update()
    {
        
    }
}
