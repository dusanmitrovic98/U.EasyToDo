using UnityEditor;
using UnityEngine;
using System.IO;
using System;
using System.Collections;

/// <summary>
/// Main editor window for EasyToDo. 
/// </summary>

public class EasyToDoWindow : EditorWindow
{
    private const string DATA_FILE_NAME = "/EasyToDoData.json";
    private const string MENU_PATH_OPEN = "Window/EasyToDo/Open";
    private const string MENU_PATH_CLOSE = "Window/EasyToDo/Close";
    private const string MENU_PATH_DELETE_DATA_FILE = "Window/EasyToDo/Delete Data File";
    private const string WINDOW_KEY_OPEN = "%#w";
    private const string WINDOW_KEY_CLOSE = "%#q";
    private const string WINDOW_KEY_DELETE_DATA_FILE = "%#d";
    private const string TITLE_CONTENT = "EasyToDo";
    private const string _placeholderText = "Add Task ...";
    private const float WIDTH = 350f;
    private const float HEIGHT = 600f;
    private const float NAVBAR_HEIGHT = 40f;
    private const float LISTS_CARDS_BEGINNING = NAVBAR_HEIGHT - 18f;
    private const float LISTS_CARDS_OFFSET = 10f;
    private const float TASK_LIST_OFFSET = 5f;
    private const float LIST_VIEW_ANIMATION_FREQUENCY = 1f;
    private static float _listViewHeight = 0f;
    private static float _footerPosition = NAVBAR_HEIGHT;
    private static float _footerHeight = NAVBAR_HEIGHT;
    private EasyToDoSettings _settings;
    private static Texture2D _boxTexture;
    private static Texture2D _buttonIconRounded;
    private static Texture2D _circleTexture;
    private static Texture2D _checkmarkTexture;
    private static Texture2D _menuIconTexture;
    private static Texture2D _menuIconTexture90;
    private static Texture2D _plusIconTexture;
    private static Texture2D _minusIconTexture;
    private static string _newTaskName = "";
    private static Rect _menuButtonPosition = new Rect(10f, 8f, 25f, 25f);
    private static Rect _addTaskButtonPosition = new Rect(8f, 50f, 40f, 40f);
    private static Rect _addTaskButtonIconPosition = new Rect(24f, 78f, 16f, -17f);
    private static Rect _newTaskFormPosition = new Rect(48f, 50f, 56f, 40f);
    private static ToDoManager _manager = new ToDoManager();
    private static int _indexTaskToDelete = -1;
    private static bool _toggleListMenu = false;
    public Vector2 scrollPosition = Vector2.zero;
    private static FixedUpdate _fixedUpdate;

    [MenuItem(MENU_PATH_OPEN + " " + WINDOW_KEY_OPEN)]
    public static void OpenWindow()
    {
        EasyToDoSettingsWindow.CloseWindow();
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
        // Fixed Update
        _fixedUpdate = ScriptableObject.CreateInstance<FixedUpdate>();
        _fixedUpdate.Enable();
        // Textures
        _boxTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/Box.PNG");
        _buttonIconRounded = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/ButtonIconRounded.PNG");
        _circleTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/CircleIcon.PNG");
        _checkmarkTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/CheckmarkIcon.PNG");
        _menuIconTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/MenuIconLight.PNG");
        _menuIconTexture90 = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/MenuIconLight90.PNG");
        _plusIconTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/PlusIcon.PNG");
        _minusIconTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/MinusIcon.PNG");
        _settings = EasyToDoSettingsWindow.LoadSettings();

        // Persist
        LoadData();

        if (_settings.EnableLogging)
        {
            Logger.EnableLogging();
        }

        // Fixed Update Actions
        _fixedUpdate.Push(() =>
        {
            Debug.Log("Hello world!");
        });
    }

    private void OnDisable()
    {
        // Fixed Update
        _fixedUpdate.Disable();
        // Persist
        EasyToDoSettingsWindow.SaveSettings();
        SaveData();
    }

    private void OnGUI()
    {
        var window = GetCurrentWindow();
        // Background
        DrawBackground(window);
        // Navbar
        DrawNavbar(window);
        // New Task Form
        DrawNewTaskFormInputGroup(window);
        // Add Task Button
        DrawAddTaskButton();
        // Task List
        DrawTaskList(window);
        // Lists View
        DrawListsView(window);

        Utility.RemoveFocus();
    }

    private static EasyToDoWindow GetCurrentWindow()
    {
        return EditorWindow.GetWindow<EasyToDoWindow>();
    }

    /// <summary>
    /// Toggles between ToDo lists and selected list views.
    /// </summary>
    private void ToggleListMenu()
    {
        if (_toggleListMenu)
        {
            _toggleListMenu = false;

            return;
        }

        _toggleListMenu = true;
    }

    /// <summary>
    /// Add new task to currently selected ToDo list.
    /// </summary>
    private void AddNewTask()
    {
        Logger.Log("Added task: " + _newTaskName);
        _manager.AddTask(_settings.CurrentListIndex, _newTaskName);
        ClearForm();
        SaveData();
    }

    /// <summary>
    /// Clears new task input form.
    /// </summary>
    private static void ClearForm()
    {
        _newTaskName = "";
    }

    /// <summary>
    /// Draws background.
    /// </summary>
    /// <param name="window"></param>
    private void DrawBackground(EasyToDoWindow window)
    {
        Utility.Box(new Rect(0, 0, window.position.width, window.position.height), _boxTexture, _settings.BackgroundColor);
    }

    /// <summary>
    /// Draws navbar UI element.
    /// </summary>
    /// <param name="window">Parent editor window.</param>
    private void DrawNavbar(EditorWindow window)
    {
        Utility.Box(new Rect(0, 0, window.position.width, NAVBAR_HEIGHT), _boxTexture, _settings.NavbarColor);

        // Menu Button
        DrawMenuButton();
    }

    /// <summary>
    /// Draws menu button UI element.
    /// </summary>
    private void DrawMenuButton()
    {
        var texture = GetMenuButtonTexture();

        Utility.Button(_menuButtonPosition, texture, _settings.MenuIconColor, ToggleListMenu);
    }

    /// <summary>
    /// Gets menu button texture depending if menu is open (button clicked) or closed.
    /// </summary>
    /// <returns></returns>
    private Texture2D GetMenuButtonTexture()
    {
        if (_toggleListMenu)
        {
            return _menuIconTexture90;
        }

        return _menuIconTexture;
    }

    /// <summary>
    /// Draws new task input form group UI element.
    /// </summary>
    /// <param name="window">Parent window.</param>
    private void DrawNewTaskFormInputGroup(EasyToDoWindow window)
    {
        var newTaskFormStyle = Utility.DefaultLabelStyle();
        Utility.StyleTextColors(newTaskFormStyle, _settings.NewTaskFormTextColorNormal, _settings.NewTaskFormTextColorFocused);
        _newTaskName = DrawNewTaskFormInput(window, newTaskFormStyle);
        DrawNewTaskFormPlaceholder(window, newTaskFormStyle, _settings);
    }

    /// <summary>
    /// Draws add task button UI element.
    /// </summary>
    private void DrawAddTaskButton()
    {
        if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Return)
        {
            AddNewTask();

            Logger.Log("Task created with name: " + _newTaskName);
        }

        Utility.Button(_addTaskButtonPosition, _boxTexture, _settings.NewTaskFormBackgroundColor, AddNewTask);
        Utility.Button(_addTaskButtonIconPosition, _plusIconTexture, _settings.NewTaskFormBackgroundIconColor, () => { });
    }

    /// <summary>
    /// Draws new task input form UI element.
    /// </summary>
    /// <param name="window">Parent window.</param>
    /// <param name="newTaskFormStyle">Form style.</param>
    /// <returns>Input value.</returns>
    private string DrawNewTaskFormInput(EasyToDoWindow window, GUIStyle newTaskFormStyle)
    {
        Rect position = new Rect(_newTaskFormPosition.x, _newTaskFormPosition.y, window.position.width - _newTaskFormPosition.width, _newTaskFormPosition.height);
        Vector4 margins = new Vector4(5f, 5f, 2f, 0f);

        return Utility.TexturedStringField(_settings.NewTaskFormBackgroundColor, position, _boxTexture, newTaskFormStyle, margins, _newTaskName);
    }

    /// <summary>
    /// Draws new task form placeholder.
    /// </summary>
    /// <param name="window">Parent window.</param>
    /// <param name="newTaskFormStyle">Form style.</param>
    /// <param name="settings">EasyToDo settings object.</param>
    private static void DrawNewTaskFormPlaceholder(EditorWindow window, GUIStyle newTaskFormStyle, EasyToDoSettings settings)
    {
        if (_newTaskName != "")
        {
            return;
        }

        Rect placeholderLabelPosition = new Rect(56f, 52f, window.position.width - 56f, 40f);
        Rect correctionPlaceholderCoverBoxPosition = new Rect(window.position.width - 8f, 50f, 8f, 40f);

        Utility.Label(placeholderLabelPosition, newTaskFormStyle, settings.NewTaskPlaceholderColor, _placeholderText);
        Utility.Box(correctionPlaceholderCoverBoxPosition, _boxTexture, settings.BackgroundColor);
    }

    /// <summary>
    /// Draws current task list UI element.
    /// </summary>
    private void DrawTaskList(EditorWindow window)
    {
        GUILayout.Space(100f);
        scrollPosition = GUILayout.BeginScrollView(
            scrollPosition, GUILayout.Height(window.position.height - 100f));
        for (int i = 0; i < _manager.CountByIndex(_settings.CurrentListIndex); i++)
        {
            DrawTask(window, i);
            GUILayout.Space(40f);
        }
        GUILayout.EndScrollView();
    }

    /// <summary>
    /// Completed tasks have their line go out of bounds because CalcSize calculates it wrongly. This is helper function to hide that overflow.
    /// </summary>
    private void DrawTaskListCorrectionBox(EditorWindow window)
    {
        Utility.Box(new Rect(window.position.width - 50f, TASK_LIST_OFFSET, 50f, window.position.height - TASK_LIST_OFFSET), _boxTexture, _settings.BackgroundColor);
    }

    /// <summary>h
    /// Draws specific task wit specific index.
    /// </summary>
    /// <param name="i">Index of task to be drawn.</param>
    private void DrawTask(EditorWindow window, int index)
    {
        DrawTaskStatus(window, index);
        DrawTaskName(window, index);
        DrawTaskDeleteButton(window, index);

    }

    /// <summary>
    /// Draws, specified by index, task completed status. 
    /// </summary>
    /// <param name="window">Parent editor window.</param>
    /// <param name="index">Targeted task index.</param>
    private void DrawTaskStatus(EditorWindow window, int index)
    {
        if (GetTaskStatus(index) == null)
        {
            return;
        }

        float x = 18f;
        float y = TASK_LIST_OFFSET + (index * 40f) + 6f;
        float width = 19f;
        float height = 19f;
        Rect position = new Rect(x, y, width, height);

        if ((bool)GetTaskStatus(index))
        {
            Utility.Button(position, _circleTexture, _settings.TaskStatusActiveOuterCircleColor, () =>
            {
                GetTask(index).Completed = false;
            });

            Utility.Box(position, _checkmarkTexture, _settings.TaskStatusActiveCheckmarkColor);

            return;
        }

        Utility.Button(position, _circleTexture, _settings.TaskStatusInactiveOuterCircleColor, () =>
        {
            GetTask(index).Completed = true;
        });
    }

    /// <summary>
    /// Draws, specified by index, task name.
    /// </summary>
    /// <param name="window">Parent editor window.</param>
    /// <param name="index">>Targeted task index.</param>
    private void DrawTaskName(EditorWindow window, int index)
    {
        GUIStyle style = Utility.DefaultLabelStyle();
        style.fontSize = 15;
        style.fontStyle = FontStyle.Bold;
        float x = 50f;
        float y = TASK_LIST_OFFSET + (index * 40f);
        float width = window.position.width - 100f;
        float height = 30f;
        Rect position = new Rect(x, y, width, height);
        var margins = new Vector4(0, 0, 0, 0);

        var text = GetTask(index).Name;

        if ((bool)GetTaskStatus(index))
        {
            GUI.enabled = false;
        }

        GetTask(index).Name = Utility.TexturedStringField(_settings.BackgroundColor, position, _boxTexture, style, margins, text);
        position.x = 52f;
        position.y = TASK_LIST_OFFSET + (index * 40f) + 16f;
        position.width = GUI.skin.textField.CalcSize(new GUIContent(text)).x * 1.2f;
        position.height = 1f;
        if ((bool)GetTaskStatus(index))
        {
            Utility.Box(position, _boxTexture, Color.grey);
            Utility.Box(new Rect(window.position.width - 50f, TASK_LIST_OFFSET + (index * 40f), 50f, 30f), _boxTexture, _settings.BackgroundColor);
            GUI.enabled = true;
        }
    }

    /// <summary>
    /// Draws, specified by index, task corresponding delete button.
    /// </summary>
    /// <param name="window">Parent editor window.</param>
    /// <param name="index">>Targeted task index.</param>
    private void DrawTaskDeleteButton(EditorWindow window, int index)
    {
        float x = window.position.width - 43f;
        float y = TASK_LIST_OFFSET + (index * 40f);
        float width = 29f;
        float height = 29f;
        Rect position = new Rect(x, y, width, height);

        _indexTaskToDelete = index;

        Utility.Button(position, _boxTexture, _settings.DeleteTaskButtonColor, DeleteTaskByIndex);

        position.y += 1f;
        Utility.Box(position, _minusIconTexture, Color.white);

        window.Repaint();
    }

    /// <summary>
    /// Draws ToDoLists view UI.
    /// </summary>
    /// <param name="window">Parent window.</param>
    private void DrawListsView(EasyToDoWindow window)
    {
        if (_toggleListMenu && _listViewHeight <= (window.position.height - 40f))
        {
            _listViewHeight += LIST_VIEW_ANIMATION_FREQUENCY;
            if (_footerPosition <= (window.position.height - 40f))
            {
                _footerPosition += LIST_VIEW_ANIMATION_FREQUENCY;
            }
        }
        else if (!_toggleListMenu && _listViewHeight >= 0)
        {
            _listViewHeight -= LIST_VIEW_ANIMATION_FREQUENCY;

            _footerPosition -= LIST_VIEW_ANIMATION_FREQUENCY;
        }

        if (_listViewHeight >= 0)
        {
            Utility.Box(new Rect(0, NAVBAR_HEIGHT, window.position.width, _listViewHeight), _boxTexture, _settings.BackgroundColor);
        }

        if (_listViewHeight > NAVBAR_HEIGHT)
        {
            Utility.Box(new Rect(0, _footerPosition, window.position.width, _footerHeight), _boxTexture, _settings.NavbarColor);
        }

        if (_toggleListMenu)
        {
            DrawToDoListsSelectionView(window);
        }
    }

    /// <summary>
    /// Draws ToDoLists selection view
    /// </summary>
    /// <param name="window"></param>
    private void DrawToDoListsSelectionView(EditorWindow window)
    {
        var position = new Rect(10f, 0f, window.position.width - 20f, 30f);

        for (int i = 0; i < /* _manager.Lists.Count */ 10; i++)
        {
            position.y = (LISTS_CARDS_BEGINNING) + ((i + 1) * (position.height + LISTS_CARDS_OFFSET));

            if (_listViewHeight >= position.y)
            {
                Utility.DrawCard(position, _boxTexture, Color.white, () =>
                {
                    DrawListsCardContent(position, i);
                });
            }
        }
    }

    /// <summary>
    /// Draws lists card content.
    /// </summary>
    /// <param name="position">Card position.</param>
    /// <param name="index">List index.</param>
    private void DrawListsCardContent(Rect position, int index)
    {
        // !
        // todo implement
    }

    /// <summary>
    /// Removes task with index of value _indexTaskToDelete. If it's value is equal to -1 does nothing.
    /// </summary>
    private void DeleteTaskByIndex()
    {
        if (_indexTaskToDelete == -1)
        {
            return;
        }

        _manager.RemoveTask(_settings.CurrentListIndex, _indexTaskToDelete);
        Logger.Log("Deleted task with index: " + _indexTaskToDelete);
        _indexTaskToDelete = -1;
    }

    /// <summary>
    /// Returns task under targeted index of currently selected list. If index out of bounds returns null.
    /// </summary>
    /// <param name="index">Targeted task index.</param>
    /// <returns>Task under targeted index of currently selected project.</returns>
    private Task GetTask(int index)
    {
        if (_settings.CurrentListIndex >= 0 && _settings.CurrentListIndex < _manager.Count())
        {
            return _manager.GetTask(_settings.CurrentListIndex, index);
        }

        return null;
    }

    /// <summary>
    /// Returns task status under targeted index of currently selected list. If index out of bounds returns null.
    /// </summary>
    /// <param name="index">Targeted task index.</param>
    /// <returns>Task status under targeted index of currently selected project.</returns>
    private bool? GetTaskStatus(int index)
    {
        if (_settings.CurrentListIndex >= 0 && _settings.CurrentListIndex < _manager.Count())
        {
            return _manager.GetTask(_settings.CurrentListIndex, index).Completed;
        }

        return null;
    }

    /// <summary>
    /// Deletes the EasyToDoSettings.json file if it exists.
    /// </summary>
    [MenuItem(MENU_PATH_DELETE_DATA_FILE + " " + WINDOW_KEY_DELETE_DATA_FILE)]
    public static void DeleteDataFile()
    {
        string filePath = Application.dataPath + DATA_FILE_NAME;
        string fileMetaPath = Application.dataPath + DATA_FILE_NAME + ".meta";

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        if (File.Exists(fileMetaPath))
        {
            File.Delete(fileMetaPath);
        }

        AssetDatabase.Refresh();

        Logger.Log("Deleted Data File.");
    }

    /// <summary>
    /// Load from local data.
    /// </summary>
    private static void LoadData()
    {
        _manager = _manager.LoadFromFile(Application.dataPath + DATA_FILE_NAME);
    }

    /// <summary>
    /// Save to local data.
    /// </summary>
    private static void SaveData()
    {
        _manager.SaveToFile(Application.dataPath + DATA_FILE_NAME);

        AssetDatabase.Refresh();
    }
}
