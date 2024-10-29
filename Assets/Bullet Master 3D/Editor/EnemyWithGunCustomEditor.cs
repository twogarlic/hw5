﻿using Bullet_Master_3D.Scripts.Game;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Bullet_Master_3D.Editor
{
    [CustomEditor(typeof(EnemyWithGun))] [CanEditMultipleObjects]
    public class EnemyWithGunCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            var script = (EnemyWithGun) target;

            if (GUILayout.Button("Find all rigidbodies auto")) 
            {
                script.AllRigidbodies = script.gameObject.GetComponentsInChildren<Rigidbody>();
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(script);
                EditorSceneManager.MarkSceneDirty(script.gameObject.scene);
            }
        }
    }
}