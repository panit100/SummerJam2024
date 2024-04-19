using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialog", menuName = "Create.../Dialog")]
public class Dialog : ScriptableObject
{
    [SerializeField] public Sprite backgroundSprite;
    [SerializeField] public List<Data> dialogList;
}