using UnityEditor;

public static class BuildApp 
{
    [MenuItem("Build/BuildApp")]
    public static void Build()
    {
        // Build Settings に登録されているシーンを取得
        string[] scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);

        // ビルド設定を反映してビルドを実行
        BuildPipeline.BuildPlayer(
            scenes,  // Build Settings のシーンリスト
            "Builds/App/TestBuildApp.exe",  // 出力先のパス
            BuildTarget.StandaloneWindows64,  // ターゲットプラットフォーム
            BuildOptions.None  // ビルドオプション
        );
    }
}