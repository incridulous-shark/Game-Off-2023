using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public UIGroup activeLayer;

    public Action<UIGroup> onUIGroupChanged;

    void Start()
    {
        ChangeActiveUILayers(1);

        PlayerInputs.instance.onNumSelectInput += ChangeActiveUILayers;
    }

    private void OnDestroy()
    {
        PlayerInputs.instance.onNumSelectInput -= ChangeActiveUILayers;
    }

    public void ChangeActiveUILayers(List<UIGroup> newLayers)
    {
        activeLayer = UIGroup.Zero;
        foreach (var layer in newLayers)
        {
            activeLayer = activeLayer | layer;
        }
        onUIGroupChanged?.Invoke(activeLayer);

        Debug.Log($"New UI Layer: {activeLayer.ToString()}");
    }

    public void ChangeActiveUILayers(int layerMap)
    {
        activeLayer = (UIGroup)layerMap;
        onUIGroupChanged?.Invoke(activeLayer);

        Debug.Log($"New UI Layer: {activeLayer.ToString()}");
    }
}

[Flags]
public enum UIGroup
{
    Zero = 0,
    One = 1,
    Two = 2,
    Three = 4,
    Four = 8,
    Five = 16
}