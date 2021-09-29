using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RadiusTextUpdater : MonoBehaviour
{
    public TMP_Text RadiusText;

    public void UpdateText(float radius)
    {
        int intRadius = (int) radius;
        RadiusText.SetText("Radius: " + intRadius);
    }
}
