using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxUI : MonoBehaviour
{
    public void ClickBack()
    {
        GarageManager.instance?.ReturnBox();
    }
}
