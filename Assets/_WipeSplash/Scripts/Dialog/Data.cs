using System;
using UnityEngine;

    [Serializable]
    public class Data
    {
        [SerializeField] public string speakername = "Speaker Name";
        [SerializeField, TextArea] public string dialog = "Speaker is speaking.";
        [SerializeField] public Sprite speakerSprite;
        [SerializeField] public Color color = Color.white;
        [SerializeField] public float interval = 0.1f;
        [SerializeField] public float fontSize = 54f;
        [SerializeField] public bool isChangeBackground;
    }