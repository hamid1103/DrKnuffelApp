using UnityEngine;
using System.Globalization;
using System;
using TMPro;
using UnityEditor;

namespace DefaultNamespace
{
    public class Calendar : MonoBehaviour
    {
        public GameManager _gameManager;
        private double days;
        public TMP_Text text;
        public TMP_Text nameText;
        public bool testingPurposes = false;

        void Start()
        {
            //For testing purposes------
#if UNITY_EDITOR
            if (EditorApplication.isPlaying)
            {
                testingPurposes = true;
            }
#endif
            if ((testingPurposes || Debug.isDebugBuild) && string.IsNullOrEmpty(_gameManager.user.AppointmentDate))
            {
                _gameManager.user = new User();
                _gameManager.user.AppointmentDate = "03/04/2026";
                _gameManager.LoggedIn = true;
            }
            //------For testing purposes

            if (_gameManager.LoggedIn)
            {
                //No need for validation, should've been done in the registration page.
                string input = _gameManager.user.AppointmentDate ; // DD/MM/YY

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

                text.text = $"Afspraak \n Over {days} dagen";     
            }
            else
            {
                text.text = ":D";
                nameText.text = "";
            }
           

        }

        private void FixedUpdate()
        {
            if (string.IsNullOrEmpty(nameText.text) && !string.IsNullOrEmpty(text.text))
            {
                nameText.text = _gameManager.UserName;
            }
        }
    }
}