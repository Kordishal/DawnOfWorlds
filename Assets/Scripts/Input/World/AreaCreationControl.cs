using System;
using Model.Geo.Organization;
using UnityEngine;
using UnityEngine.UI;

namespace Input.World
{
    public class AreaCreationControl : MonoBehaviour
    {

        public InputField nameInputField;
        
        public Button saveButton;
        public Button cancelButton;

        private AreaBuilder _areaBuilder;


        private void Start()
        {
            
            nameInputField.onEndEdit.AddListener(SaveName);   
            saveButton.onClick.AddListener(Save);
            cancelButton.onClick.AddListener(Cancel);
        }

        private void SaveName(string text)
        {
            _areaBuilder.AddName(text);
        }

        private void Cancel()
        {
            gameObject.SetActive(false);
        }

        private void Save()
        {
            if (!_areaBuilder.IsValid()) return;
            
            
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            _areaBuilder = null;
        }

        private void OnEnable()
        {
            _areaBuilder = new AreaBuilder();
        }
    }
}