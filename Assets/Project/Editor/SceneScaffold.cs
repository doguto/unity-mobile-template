#if UNITY_EDITOR

using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Project.Scripts.Core.Constant;
using Project.Scripts.Core.Extension;

namespace Project.Editor
{
    [InitializeOnLoad]
    public class SceneScaffold : EditorWindow
    {
        const string PendingSceneSessionKey = "SceneScaffold.PendingLifetimeScopeScene";

        string sceneName = "";
        string lastMessage = "";
        MessageType lastMessageType = MessageType.None;

        static SceneScaffold()
        {
            CompilationPipeline.compilationFinished += OnCompilationFinished;
        }

        [MenuItem("Project/Scene Scaffold")]
        public static void ShowWindow()
        {
            var window = GetWindow<SceneScaffold>("Scene Scaffold");
            window.minSize = new Vector2(200, 140);
        }

        void OnGUI()
        {
            GUILayout.Label("Scene Scaffold", EditorStyles.boldLabel);
            GUILayout.Space(8);

            sceneName = EditorGUILayout.TextField("Scene Name", sceneName);

            GUILayout.Space(8);

            GUI.enabled = !string.IsNullOrWhiteSpace(sceneName);
            if (GUILayout.Button("Create Scene"))
            {
                var trimmed = sceneName.Trim();
                if (SceneAlreadyExists(trimmed))
                {
                    lastMessage = $"'{trimmed}' はすでに存在しています";
                    lastMessageType = MessageType.Warning;
                }
                else
                {
                    CreateSceneFolders(trimmed);
                    CreateLifetimeScopeScript(trimmed);
                    CreateSceneFile(trimmed);
                    SchedulePendingLifetimeScopeSetup(trimmed);
                    Close();
                    return;
                }
            }
            GUI.enabled = true;

            if (!lastMessage.IsNullOrEmpty())
            {
                GUILayout.Space(8);
                EditorGUILayout.HelpBox(lastMessage, lastMessageType);
            }
        }

        static bool SceneAlreadyExists(string sceneName)
        {
            return AssetDatabase.IsValidFolder($"{GamePath.Scenes}/{sceneName}")
                || File.Exists(ScenePath(sceneName));
        }

        static string ScenePath(string sceneName)
        {
            return $"{GamePath.Scenes}/{sceneName}.unity";
        }

        static void CreateSceneFile(string sceneName)
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Additive);
            EditorSceneManager.SaveScene(scene, ScenePath(sceneName));
            EditorSceneManager.CloseScene(scene, true);
        }

        static void CreateSceneFolders(string sceneName)
        {
            var basePath = $"{GamePath.Scenes}/{sceneName}/Scripts";

            var folders = new[]
            {
                (path: $"{basePath}/View",                        guid: "b34b4035941f46059728efaeac337b9e"),
                (path: $"{basePath}/Application/UseCase",         guid: "6ea6c61753b94b39a8359eb4980cde9b"),
                (path: $"{basePath}/Application/Service",         guid: "6ea6c61753b94b39a8359eb4980cde9b"),
                (path: $"{basePath}/Application/ViewModel",       guid: "f3a564df6d734d0d9ed686771d84b8b7"),
                (path: $"{basePath}/Domain",                      guid: "c07b194726f14d3d9eb648a5c2914073"),
                (path: $"{basePath}/Repository/EntityRepository", guid: "bc79a1547d5d44cfb433d4c076141d95"),
                (path: $"{basePath}/Repository/AssetRepository",  guid: "bc79a1547d5d44cfb433d4c076141d95"),
                (path: $"{basePath}/Scope",                       guid: "e7c716e1f42040249fd1d4934497af16"),
            };

            foreach (var (path, guid) in folders)
            {
                CreateFolderRecursive(path);
                CreateAsmref(path, guid);
            }

            AssetDatabase.Refresh();
        }

        static void CreateLifetimeScopeScript(string sceneName)
        {
            var folderPath = $"{GamePath.Scenes}/{sceneName}/Scripts/Scope";
            var relativePath = folderPath.Substring("Assets/".Length);
            var fullPath = Path.Combine(Application.dataPath, relativePath, $"{sceneName}SceneLifetimeScope.cs");
            var content = $@"using VContainer;
using VContainer.Unity;

namespace Project.Scenes.{sceneName}.Scripts.Scope
{{
    public class {sceneName}SceneLifetimeScope : LifetimeScope
    {{
        protected override void Configure(IContainerBuilder builder)
        {{
        }}
    }}
}}
";
            File.WriteAllText(fullPath, content);
        }

        // 新規生成した LifetimeScope クラスはこの時点ではまだコンパイルされておらず型として参照できない。
        // SessionState はドメインリロードを跨いで値を保持するため、コンパイル完了イベント側で続きの GameObject 配置を行う
        static void SchedulePendingLifetimeScopeSetup(string sceneName)
        {
            SessionState.SetString(PendingSceneSessionKey, sceneName);
            AssetDatabase.Refresh();
        }

        static void OnCompilationFinished(object context)
        {
            var pendingSceneName = SessionState.GetString(PendingSceneSessionKey, "");
            if (pendingSceneName.IsNullOrEmpty()) return;

            SessionState.EraseString(PendingSceneSessionKey);
            SetupLifetimeScopeGameObject(pendingSceneName);
        }

        static void SetupLifetimeScopeGameObject(string sceneName)
        {
            var typeName = $"Project.Scenes.{sceneName}.Scripts.Scope.{sceneName}SceneLifetimeScope";
            var lifetimeScopeType = FindType(typeName);
            if (lifetimeScopeType == null)
            {
                Debug.LogError($"SceneScaffold: {typeName} が見つかりませんでした。コンパイルエラーがないか確認してください");
                return;
            }

            var scene = EditorSceneManager.OpenScene(ScenePath(sceneName), OpenSceneMode.Additive);

            var lifetimeScopeGameObject = new GameObject($"{sceneName}SceneLifetimeScope");
            SceneManager.MoveGameObjectToScene(lifetimeScopeGameObject, scene);
            lifetimeScopeGameObject.AddComponent(lifetimeScopeType);

            EditorSceneManager.SaveScene(scene);
            EditorSceneManager.CloseScene(scene, true);

            Debug.Log($"SceneScaffold: '{sceneName}' に LifetimeScope を配置しました");
        }

        static Type FindType(string fullName)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Select(assembly => assembly.GetType(fullName))
                .FirstOrDefault(type => type != null);
        }

        static void CreateAsmref(string folderPath, string asmdefGuid)
        {
            var folderName = Path.GetFileName(folderPath);
            var relativePath = folderPath.Substring("Assets/".Length);
            var fullPath = Path.Combine(Application.dataPath, relativePath, $"{folderName}.asmref");
            var content = $"{{\n    \"reference\": \"GUID:{asmdefGuid}\"\n}}";
            File.WriteAllText(fullPath, content);
        }

        static void CreateFolderRecursive(string path)
        {
            if (AssetDatabase.IsValidFolder(path)) return;

            var parts = path.Split('/');
            var current = parts[0];
            for (var i = 1; i < parts.Length; i++)
            {
                var next = $"{current}/{parts[i]}";
                if (!AssetDatabase.IsValidFolder(next))
                {
                    AssetDatabase.CreateFolder(current, parts[i]);
                }
                current = next;
            }
        }
    }
}
#endif
