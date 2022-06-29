using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class PlayableEditor : MonoBehaviour
{
    [MenuItem("Tools/Create Playable Asset")]
    public static void CreatePlayableAsset()
    {
        //var asset = new NewPlayableAsset() { data = 2 };
        var scriptAsset = ScriptableObject.CreateInstance<NewPlayableAsset>();
        scriptAsset.data = 2;
        AssetDatabase.CreateAsset(scriptAsset, "Assets/Arts/Playable/NewPlayableAsset.asset");
    }
}
