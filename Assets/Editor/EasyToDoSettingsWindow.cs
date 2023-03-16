using UnityEditor;
using UnityEngine;
using System.IO;

/// <summary>
/// Editor window for editing EasyToDo settings.
/// </summary>
public class EasyToDoSettingsWindow : EditorWindow
{
    private const string SETTINGS_FILE_NAME = "/EasyToDoSettings.json";
    private const string MENU_PATH_OPEN = "Window/EasyToDo/Settings";
    private const string MENU_PATH_DELETE_SETTINGS_FILE = "Window/EasyToDo/Delete Settings File";
    private const string MENU_PATH_CLOSE = "Window/EasyToDo/Close Settings";
    private const string WINDOW_KEY_SETTINGS_OPEN = "%&w";
    private const string WINDOW_KEY_SETTINGS_CLOSE = "%&q";
    private const string WINDOW_KEY_DELETE_SETTINGS_FILE = "%&d";
    private const string TITLE_CONTENT = "EasyToDo Settings";
    private const float WIDTH = 400f;
    private const float HEIGHT = 600f;
    private static EasyToDoSettings _settings;
    private static string _settingsPath;
    private static Texture2D _boxTexture;


    [MenuItem(MENU_PATH_OPEN + " " + WINDOW_KEY_SETTINGS_OPEN)]
    public static void OpenWindow()
    {
        EasyToDoWindow.CloseWindow();
        var window = GetWindow<EasyToDoSettingsWindow>();
        window.titleContent = new GUIContent(TITLE_CONTENT);
        Utility.CenterWindow(window, WIDTH, HEIGHT);
    }

    [MenuItem(MENU_PATH_CLOSE + " " + WINDOW_KEY_SETTINGS_CLOSE)]
    public static void CloseWindow()
    {
        EasyToDoSettingsWindow window = EditorWindow.GetWindow<EasyToDoSettingsWindow>();
        window.Close();
    }

    private void OnEnable()
    {
        _boxTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/box.PNG");
        LoadSettings();
    }

    private void OnDisable()
    {
        SaveSettings();
    }

    private void OnGUI()
    {
        GUILayout.Space(10f);
        _settings.BackgroundColor = EditorGUILayout.ColorField("Background Color:", _settings.BackgroundColor);
        GUILayout.Space(10f);
        _settings.NavbarColor = EditorGUILayout.ColorField("Navbar Color:", _settings.NavbarColor);
        GUILayout.Space(10f);
        _settings.MenuIconColor = EditorGUILayout.ColorField("Menu Icon Color", _settings.MenuIconColor);
        GUILayout.Space(10f);
        _settings.NewTaskFormTextColorNormal = EditorGUILayout.ColorField("Form Text Normal Color:", _settings.NewTaskFormTextColorNormal);
        GUILayout.Space(10f);
        _settings.NewTaskFormTextColorFocused = EditorGUILayout.ColorField("Form Text Focused Color:", _settings.NewTaskFormTextColorFocused);
        GUILayout.Space(10f);
        _settings.NewTaskFormBackgroundColor = EditorGUILayout.ColorField("Form Background Color:", _settings.NewTaskFormBackgroundColor);
        GUILayout.Space(10f);
        _settings.NewTaskFormBackgroundIconColor = EditorGUILayout.ColorField("Add Task Icon Color:", _settings.NewTaskFormBackgroundIconColor);
        GUILayout.Space(10f);
        _settings.NewTaskPlaceholderColor = EditorGUILayout.ColorField("Placeholder Color:", _settings.NewTaskPlaceholderColor);
        GUILayout.Space(10f);
    }

    private static EasyToDoSettingsWindow GetCurrentWindow()
    {
        return EditorWindow.GetWindow<EasyToDoSettingsWindow>();
    }


    /// <summary>
    /// Load settings from local data file.
    /// </summary>
    /// <returns>Settings object.</returns>
    public static EasyToDoSettings LoadSettings()
    {
        _settingsPath = Application.dataPath + SETTINGS_FILE_NAME;

        if (File.Exists(_settingsPath))
        {
            string json = File.ReadAllText(_settingsPath);
            _settings = JsonUtility.FromJson<EasyToDoSettings>(json);

            Logger.Log("Loaded Settings: " + json, Color.red);
        }
        else
        {
            _settings = new EasyToDoSettings();

            Logger.Log("Generated New Settings File.");
        }

        return _settings;
    }

    /// <summary>
    /// Save settings to local data file.
    /// </summary>
    public static void SaveSettings()
    {
        if (_settings == null)
        {
            return;
        }

        string json = JsonUtility.ToJson(_settings, true);

        File.WriteAllText(_settingsPath, json);

        AssetDatabase.Refresh();

        Logger.Log("Saved Settings: " + json);
    }

    /// <summary>
    /// Deletes the EasyToDoSettings.json file if it exists.
    /// </summary>
    [MenuItem(MENU_PATH_DELETE_SETTINGS_FILE + " " + WINDOW_KEY_DELETE_SETTINGS_FILE)]
    public static void DeleteSettingsFile()
    {
        string filePath = Application.dataPath + SETTINGS_FILE_NAME;
        string fileMetaPath = Application.dataPath + SETTINGS_FILE_NAME + ".meta";

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        if (File.Exists(fileMetaPath))
        {
            File.Delete(fileMetaPath);
        }

        AssetDatabase.Refresh();

        Logger.Log("Deleted Settings File.");
    }
}