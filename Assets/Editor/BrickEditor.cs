using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(Brick))]
[CanEditMultipleObjects]
public class BrickEditor : Editor
{

    private Brick _brick;
    private SpriteRenderer _spriteRenderer;

    public void OnEnable()
    {

        _brick = (Brick) target;
        _spriteRenderer = _brick.GetComponent<SpriteRenderer>();

    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if (GUI.changed)
        {
            _brick.brickObject = _brick.SetBrickTypeEditor();
            _spriteRenderer.sprite = _brick.brickObject.textures[_brick.GetHealth() - 1];
        }
        

    }
}
