using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILocalizationHelper
{
    public void SetLanguage(int index);
    public void UpdateLanguage();
    public void UpdateLanguageForSaveGameSlots(string[] slotInfo);
    public void UpdateLanguageForSaveSuccess(bool success);
}
