using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class SubtitlerClass : MonoBehaviour
    {
        private GameManager _gameManager;
        private TMP_Text _text;
        public string CurrentString;
        public List<string> script;
        public int ScriptIndex = 0;
        
        void Start()
        {
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            
            //For development purposes
            if (string.IsNullOrEmpty(_gameManager.UserName))
            {
                _gameManager.UserName = PlayerPrefs.GetString("UserName");
                if (string.IsNullOrEmpty(_gameManager.UserName))
                {
                    _gameManager.UserName = "Klaas";
                }
            }
            _text = gameObject.GetComponent<TMP_Text>();
            if (script.Count > 0)
            {
                CurrentString = script[ScriptIndex];
            }
            SetText(CurrentString);
        }

        public void ResetScriptIndex()
        {
            ScriptIndex = 0;
        }

        public void AdvanceScript(int amount = 1)
        {
            ScriptIndex += amount;
            if (ScriptIndex < script.Count)
            {
                SetText(script[ScriptIndex]);
            }
        }
        
        public void SetText(string input)
        {
            _text.text = ParseString(input);
        }

        private string ParseString(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            return input.Replace("{{User}}", _gameManager.UserName);
        }
    }
}