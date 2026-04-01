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
        
        void Start()
        {
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            _text = gameObject.GetComponent<TMP_Text>();
            _text.text = CurrentString;
        }

        public void SetText(string input)
        {
            
        }

        private string ParseString(string input)
        {

            List<string> splits = input.Split().ToList();
            for (int i = 0; i < splits.Count-1; i++)
            {
                string text = splits[i];
                if (text.Contains("{{User}}"))
                {
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
            return "";
        }
    }
}