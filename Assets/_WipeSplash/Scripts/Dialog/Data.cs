using System;
using UnityEngine;
using Sirenix.OdinInspector;

    [Serializable]
    public class Data
    {
        [SerializeField] public string speakername = "Speaker Name";
        [SerializeField, TextArea(4, 10)] public string dialog = "Speaker is speaking.";
        [SerializeField] public Sprite speakerSprite;
        [SerializeField] public Color color = Color.white;
        [SerializeField] public float fontSize = 54f;
        [SerializeField] public bool isChangeBackground;
    }