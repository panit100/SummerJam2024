using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour
{
   const string hoverSFX = "SFX_UI_Hover";
   const string clickSFX = "SFX_UI_Click";
      
   public void ButtonHover(BaseEventData data)
   {
      SoundManager.Instance.PlaySFX(hoverSFX);
   }
   public void ButtonClick(BaseEventData data)
   {
      SoundManager.Instance.PlaySFX(clickSFX);
   }
}
