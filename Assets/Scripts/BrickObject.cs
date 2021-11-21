using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Brick")]

public class BrickObject : ScriptableObject
{
    public BrickType brickType;

    public AudioClip hitAudio;

    public Sprite[] textures = new Sprite[6];

}
