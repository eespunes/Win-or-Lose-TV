 using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SeasonGenerator))]
public class SeasonGeneratorEdit : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SeasonGenerator lSeasonGenerator = (SeasonGenerator)target;
        if(GUILayout.Button("Generate"))
        {
            lSeasonGenerator.Generate();
        }
        if(GUILayout.Button("Reset"))
        {
            lSeasonGenerator.Reset();
        }
    }
}
