using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class RegisterPagina : MonoBehaviour
    {
        public GameManager _gameManager;
        public TMP_InputField UsernameInput;
        
        private void Start()
        {
            //Should be filled in if following designed user flow, but better add a check just in case.
            if (!string.IsNullOrEmpty(_gameManager.UserName))
            {
                UsernameInput.text = _gameManager.UserName;
            }
        }

    }
}