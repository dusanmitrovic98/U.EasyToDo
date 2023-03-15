using UnityEditor;
using UnityEngine;

/// <summary>
/// Main editor window for EasyToDo.  
/// </summary>

public class EasyToDoWindow : EditorWindow
{
    private const string MENU_PATH_OPEN = "Window/EasyToDo/Open";
    private const string MENU_PATH_CLOSE = "Window/EasyToDo/Close";
    private const string WINDOW_KEY_OPEN = "%#w";
    private const string WINDOW_KEY_CLOSE = "%#q";
    private const string TITLE_CONTENT = "EasyToDo";
    private const float WIDTH = 350f;
    private const float HEIGHT = 600f;
    private EasyToDoSettings _settings;


    [MenuItem(MENU_PATH_OPEN + " " + WINDOW_KEY_OPEN)]
    public static void OpenWindow()
    {
        var window = GetWindow<EasyToDoWindow>();
        window.titleContent = new GUIContent(TITLE_CONTENT);
        Utility.CenterWindow(window, WIDTH, HEIGHT);
    }

    [MenuItem(MENU_PATH_CLOSE + " " + WINDOW_KEY_CLOSE)]
    public static void CloseWindow()
    {
        EasyToDoWindow window = EditorWindow.GetWindow<EasyToDoWindow>();
        window.Close();
    }

    private void OnEnable()
    {
        _settings = EasyToDoSettingsWindow.LoadSettings();
    }

    private void OnDisable()
    {
        EasyToDoSettingsWindow.SaveSettings();
    }

    private void OnGUI()
    {
        Utility.SetBackgroundColor(EditorWindow.GetWindow<EasyToDoWindow>(), _settings.backgroundColor);
    }
}
