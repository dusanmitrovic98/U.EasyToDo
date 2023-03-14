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
    private const float WIDTH = 400f;
    private const float HEIGHT = 600f;
    private static EditorWindow _window;

    private static void InitializeWindow()
    {
        // Set window properties
        _window.titleContent = new GUIContent("EasyToDo");
        Utility.CenterWindow(_window, WIDTH, HEIGHT);

        // Register event handlers
        // ...
    }

    private void OnEnable()
    {
        // Initialize data and resources
        // ...
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

    [MenuItem(MENU_PATH_OPEN + " " + WINDOW_KEY_OPEN)]
    public static void ShowWindow()
    {
        _window = GetWindow<EasyToDoWindow>();
        InitializeWindow();
        _window.Show();
    }

    [MenuItem(MENU_PATH_CLOSE + " " + WINDOW_KEY_CLOSE)]
    public static void CloseWindow()
    {
        if (_window != null)
        {
            _window.Close();
        }
    }
}
