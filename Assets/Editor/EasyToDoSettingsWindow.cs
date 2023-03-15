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
    private const string MENU_PATH_CLOSE = "Window/EasyToDo/Close Settings";
    private const string WINDOW_KEY_SETTINGS_OPEN = "%&w";
    private const string WINDOW_KEY_SETTINGS_CLOSE = "%&q";
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
        _settings.backgroundColor = EditorGUILayout.ColorField("Background Color", _settings.backgroundColor);
        _settings.navbarColor = EditorGUILayout.ColorField("Navbar Color", _settings.navbarColor);
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

            Debug.Log("Loaded Settings: " + json); // ! remove
        }
        else
        {
            _settings = new EasyToDoSettings();

            Debug.Log("New Settings!"); // ! remove
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

        Debug.Log("Saved Settings: " + json); // ! remove
    }
}