using UnityEngine;
using System.Globalization;
using System;
using TMPro;
using UnityEditor;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class Calendar : MonoBehaviour
    {
        public GameManager _gameManager;
        private double days;
        public TMP_Text text;
        public TMP_Text nameText;
        public bool testingPurposes = false;
        private Button _button;

        void Start()
        {
            _button = gameObject.GetComponent<Button>();
            if (_gameManager.LoggedIn && !string.IsNullOrEmpty(_gameManager.userData.AppointmentDate))
            {
                _button.interactable = false;
                //No need for validation, should've been done in the registration page.
                string input = _gameManager.userData.AppointmentDate ; // DD/MM/YY

                if (DateTime.TryParseExact(input, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out DateTime result))
                {
                    // If you only care about the date:
                    DateTime dateOnly = result.Date;
                    DateTime today = DateTime.Today.Date;

                    days = (dateOnly - today).TotalDays;
                }

                if (days < 2)
                {
                    text.text = $"Afspraak \n Over 1 dag";     
                }
                else
                {
                    text.text = $"Afspraak \n Over {days} dagen";     
                }
            }
            else
            {
                _button.interactable = true;
                text.text = "Er is nog niks hier.";
                nameText.text = "";
            }
           

        }

        private void FixedUpdate()
        {
            if (string.IsNullOrEmpty(nameText.text) && !string.IsNullOrEmpty(text.text))
            {
                nameText.text = _gameManager.UserName;
            }
            
            if (_gameManager.LoggedIn)
            {
                _button.interactable = false;
                //No need for validation, should've been done in the registration page.
                string input = _gameManager.userData.AppointmentDate ; // DD/MM/YY

                if (DateTime.TryParseExact(input, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out DateTime result))
                {
                    // If you only care about the date:
                    DateTime dateOnly = result.Date;
                    DateTime today = DateTime.Today.Date;

                    days = (dateOnly - today).TotalDays;
                }

                if (days < 2)
                {
                    text.text = $"Afspraak \n Over 1 dag";     
                }
                else
                {
                    text.text = $"Afspraak \n Over {days} dagen";     
                }
            }
            else
            {
                _button.interactable = true;
                text.text = "Er is nog niks hier.";
                nameText.text = "";
            }
        }
    }
}