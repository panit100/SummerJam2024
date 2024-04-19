using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialog", menuName = "Create.../Dialog")]
public class Dialog : ScriptableObject
{
    public enum NextState
    {
        DIALOG, BATTLE, ENDGAME
    }
    [SerializeField] public NextState nextState;
    [SerializeField] public Sprite backgroundSprite;
    [SerializeField] public string bgmName;
    [SerializeField] public List<Data> dialogList;
}