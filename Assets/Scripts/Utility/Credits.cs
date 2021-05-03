using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WordHoarder.Utility
{
    public class Credits : MonoBehaviour
    {
        InputField inputField;
        string defaultText =
            @"
License: https://unsplash.com/license

Kitchen 1 - https://unsplash.com/photos/MP0bgaS_d1c
Kitchen 2 - https://unsplash.com/photos/KiUg-4xmTwo
Living room 1 - https://unsplash.com/photos/paydk0JcIOQ
Living room 2 - https://unsplash.com/photos/sAtN5q4cJlM
Living room 3 - https://unsplash.com/photos/G3qlZQXsBOE
Bathroom - https://unsplash.com/photos/FX1EbT-jKBQ
Study - https://unsplash.com/photos/VtEYCUXZjRo
Bedroom - https://unsplash.com/photos/zwbHbzxd2lg";

        private void Awake()
        {
            inputField = GetComponent<InputField>();
            inputField.readOnly = true;
            inputField.interactable = true;
            inputField.text = defaultText;
        }
    }
}