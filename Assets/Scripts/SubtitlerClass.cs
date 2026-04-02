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
            _text = gameObject.GetComponent<TMP_Text>();
            _text.text = CurrentString;
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

            List<string> splits = input.Split().ToList();
            for (int i = 0; i < splits.Count-1; i++)
            {
                string text = splits[i];
                if (text.Contains("{{User}}"))
                {
                    //No need to do complex string manipulation right now. Don't have the time for that.
                    if (text.Last() == Convert.ToChar(","))
                    {
                        splits[i] = $"{_gameManager.UserName},";
                    }
                    else
                    {
                        splits[i] = $"{_gameManager.UserName}";
                    }
                }
            }
            return string.Join(" ", splits);
        }
    }
}