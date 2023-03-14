using UnityEditor;
using UnityEngine;

/// <summary>
/// Editor window for editing EasyToDo settings.
/// </summary>
public class EasyToDoSettingsWindow : EditorWindow
{
    private const string MENU_PATH_OPEN = "Window/EasyToDo/Settings";
    private const string WINDOW_KEY_SETTINGS_OPEN = "%#e";
    private const float WIDTH = 400f;
    private const float HEIGHT = 600f;
    private static EditorWindow _window;
    private static EasyToDoSettings _settings;

    private static void InitializeWindow()
    {
        // Set window properties
        _window.titleContent = new GUIContent("EasyToDo Settings");
        Utility.CenterWindow(_window, WIDTH, HEIGHT);

        // Register event handlers
        // ...
    }

    private void OnEnable()
    {
        // Initialize data and resources
        // ...
    }

    private void Start()
    {
        _settings.backgroundColor = new Color(0.2f, 0.2f, 0.2f);
    }

    private void OnGUI()
    {
        // Add UI elements
        // ...
    }

    private void OnDisable()
    {
        // Clean up data and resources
        // ...
    }

    [MenuItem(MENU_PATH_OPEN + " " + WINDOW_KEY_SETTINGS_OPEN)]
    public static void ShowWindow()
    {
        _window = GetWindow<EasyToDoSettingsWindow>();
        InitializeWindow();
        _window.Show();
    }

    private void LoadSettings()
    {
        string json = PlayerPrefs.GetString("EasyToDoSettings", "{}");
        _settings = JsonUtility.FromJson<EasyToDoSettings>(json);
    }

    private void SaveSettings()
    {
        string json = JsonUtility.ToJson(_settings);
        PlayerPrefs.SetString("EasyToDoSettings", json);
    }
}
