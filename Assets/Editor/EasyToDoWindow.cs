using UnityEditor;
using UnityEngine;
using System.IO;
using System;

/// <summary>
/// Main editor window for EasyToDo.
/// </summary>

public class EasyToDoWindow : EditorWindow
{
    private const string DATA_FILE_NAME = "/EasyToDoData.json";
    private const string MENU_PATH_OPEN = "Window/EasyToDo/Open";
    private const string MENU_PATH_CLOSE = "Window/EasyToDo/Close";
    private const string MENU_PATH_TOGGLE = "Window/EasyToDo/Toggle Menu";
    private const string MENU_PATH_DELETE_DATA_FILE = "Window/EasyToDo/Delete Data File";
    private const string WINDOW_KEY_OPEN = "%#w";
    private const string WINDOW_KEY_CLOSE = "%#q";
    private const string WINDOW_KEY_TOGGLE_MENU = "%m";
    private const string WINDOW_KEY_DELETE_DATA_FILE = "%#d";
    private const string TITLE_CONTENT = "EasyToDo";
    private const string _placeholderText = "Add Task ...";
    private const string _placeholderListsText = "Add List ...";
    private const float WIDTH = 350f;
    private const float HEIGHT = 600f;
    private const float NAVBAR_HEIGHT = 40f;
    private const float LISTS_CARDS_BEGINNING = NAVBAR_HEIGHT;
    private const float LISTS_CARDS_OFFSET = 10f;
    private const float TASK_LIST_OFFSET = 5f;
    private const float LISTS_OFFSET = 5f;
    private const float LIST_VIEW_ANIMATION_FREQUENCY = 1f;
    private const float MENU_TOGGLE_VIEW_ANIMATION_SPEED = 0.0005f;
    private static float _listViewHeight = 0f;
    private static float _footerPosition = 0;
    private static float _footerHeight = NAVBAR_HEIGHT;
    private static float _taskNameOpacity = 0.8f;
    private static float _listNameOpacity = 0.7f;
    private static EasyToDoSettings _settings;
    private static Texture2D _boxTexture;
    private static Texture2D _cardIconWideTexture;
    private static Texture2D _buttonIconRounded;
    private static Texture2D _buttonIconRoundedOneSide;
    private static Texture2D _circleTexture;
    private static Texture2D _checkmarkTexture;
    private static Texture2D _menuIconTexture;
    private static Texture2D _menuIconTexture90;
    private static Texture2D _plusIconTexture;
    private static Texture2D _minusIconTexture;
    private static string _newTaskName = "";
    private static string _newListName = "";
    private static Rect _menuButtonPosition = new Rect(10f, 8f, 25f, 25f);
    private static Rect _addButtonPosition = new Rect(8f, 50f, 40f, 40f);
    private static Rect _addButtonIconPosition = new Rect(24f, 78f, 16f, -17f);
    private static Rect _newFormPosition = new Rect(48f, 50f, 56f, 40f);
    private static ToDoManager _manager = new ToDoManager();
    private static int _indexTaskToDelete = -1;
    private static bool _toggleListMenu = false;
    public static Vector2 _scrollPosition = Vector2.zero;
    // public static Vector2 _listsScrollPosition = Vector2.zero;
    private static FixedUpdate _listsViewFixedUpdate;

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

    [MenuItem(MENU_PATH_TOGGLE + " " + WINDOW_KEY_TOGGLE_MENU)]
    public static void ToggleMenu()
    {
        ToggleListMenu();
    }

    private void OnEnable()
    {
        // Fixed Update
        _listsViewFixedUpdate = ScriptableObject.CreateInstance<FixedUpdate>();
        _listsViewFixedUpdate.Enable();
        // Textures
        _boxTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/Box.PNG");
        _cardIconWideTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/CardIconWide.PNG");
        _buttonIconRounded = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/ButtonIconRounded.PNG");
        _buttonIconRoundedOneSide = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/ButtonIconRoundedOneSide.PNG");
        _circleTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/CircleIcon.PNG");
        _checkmarkTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/CheckmarkIcon.PNG");
        _menuIconTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/MenuIconLight.PNG");
        _menuIconTexture90 = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/MenuIconLight90.PNG");
        _plusIconTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/PlusIcon.PNG");
        _minusIconTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/MinusIcon.PNG");

        // Persist
        LoadData();

        if (_settings.EnableLogging)
        {
            Logger.EnableLogging();
        }

        // Fixed Update Actions
        _listsViewFixedUpdate.TickFrequency = MENU_TOGGLE_VIEW_ANIMATION_SPEED;

        _listsViewFixedUpdate.Push(() =>
        {
            UpdateListsViewPositions(GetCurrentWindow());
        });
    }

    private void OnDisable()
    {
        // Fixed Update
        _listsViewFixedUpdate.Disable();
        // Persist
        SaveData();
    }

    private void OnGUI()
    {
        var window = GetCurrentWindow();

        // Background
        DrawBackground(window);
        if (_listViewHeight <= 100f)
        {
            // New Task Form
            DrawFormInputGroup(window, ref _newTaskName, _settings.NewTaskFormTextColorNormal, _settings.NewTaskFormTextColorFocused,
            _settings.NewTaskPlaceholderColor, _placeholderText, _settings.NewTaskFormBackgroundColor);
            // Add Task Button
            DrawAddTaskButton();
        }
        // Task List
        DrawTaskList(window);
        // Lists View
        DrawListsView(window);
        // Navbar
        DrawNavbar(window);

        Utility.RemoveFocus();
    }

    /// <summary>
    /// Handles enter key press while input form field is focused.
    /// </summary>
    private static void EnterKeyHandler()
    {
        if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Return)
        {
            if (_toggleListMenu)
            {
                AddNewList();

                Logger.Log("List created.");
            }
            else
            {
                AddNewTask();

                Logger.Log("Task created.");
            }
        }
    }

    private static EasyToDoWindow GetCurrentWindow()
    {
        return EditorWindow.GetWindow<EasyToDoWindow>();
    }

    /// <summary>
    /// Toggles between ToDo lists and selected list views.
    /// </summary>
    private static void ToggleListMenu()
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
    private static void AddNewTask()
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
    private static void DrawBackground(EasyToDoWindow window)
    {
        Utility.Box(new Rect(0, 0, window.position.width, window.position.height), _boxTexture, _settings.BackgroundColor);
    }

    /// <summary>
    /// Draws navbar UI element.
    /// </summary>
    /// <param name="window">Parent editor window.</param>
    private static void DrawNavbar(EditorWindow window)
    {
        Utility.Box(new Rect(0, 0, window.position.width, NAVBAR_HEIGHT), _boxTexture, _settings.NavbarColor);

        // Menu Button
        DrawMenuButton();
    }

    /// <summary>
    /// Draws menu button UI element.
    /// </summary>
    private static void DrawMenuButton()
    {
        var texture = GetMenuButtonTexture();

        Utility.Button(_menuButtonPosition, texture, _settings.MenuIconColor, ToggleListMenu);
    }

    /// <summary>
    /// Gets menu button texture depending if menu is open (button clicked) or closed.
    /// </summary>
    /// <returns></returns>
    private static Texture2D GetMenuButtonTexture()
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
    private static void DrawFormInputGroup(EasyToDoWindow window, ref string textInput, Color normal, Color focused, Color placeholderColor, string placeholderText, Color formBackgroundColor)
    {
        var formStyle = Utility.DefaultLabelStyle();
        Utility.StyleTextColors(formStyle, normal, focused);
        textInput = DrawFormInput(_newFormPosition, window, formStyle, formBackgroundColor, ref textInput);
        DrawFormPlaceholder(window, formStyle, _settings, placeholderColor, placeholderText, textInput);
    }

    /// <summary>
    /// Draws add task button UI element.
    /// </summary>
    private static void DrawAddTaskButton()
    {
        EnterKeyHandler();

        Utility.Button(_addButtonPosition, _boxTexture, _settings.NewTaskFormBackgroundColor, AddNewTask);
        Utility.Button(_addButtonIconPosition, _plusIconTexture, _settings.NewTaskFormBackgroundIconColor, () => { });
    }

    /// <summary>
    /// Draws add task button UI element.
    /// </summary>
    private static void DrawAddListButton()
    {
        EnterKeyHandler();

        Utility.Button(_addButtonPosition, _boxTexture, _settings.NewListFormBackgroundColor, AddNewList);
        Utility.Button(_addButtonIconPosition, _plusIconTexture, _settings.NewListFormBackgroundIconColor, () => { });
    }

    /// <summary>
    /// Add new task to currently selected ToDo list.
    /// </summary>
    private static void AddNewList()
    {
        Logger.Log("List created with name: " + _newListName);
        _manager.AddList(_newListName);
        _manager.Lists[_manager.Lists.Count - 1].Name = _newListName;
        _settings.ListsColors.Add(UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f));
        ClearListForm();
        SaveData();
    }

    private static void ClearListForm()
    {
        _newListName = "";
    }

    /// <summary>
    /// Draws new task input form UI element.
    /// </summary>
    /// <param name="window">Parent window.</param>
    /// <param name="newTaskFormStyle">Form style.</param>
    /// <returns>Input value.</returns>
    private static string DrawFormInput(Rect formPosition, EasyToDoWindow window, GUIStyle formStyle, Color color, ref string textInput)
    {
        Rect position = new Rect(formPosition.x, formPosition.y, window.position.width - formPosition.width, formPosition.height);
        Vector4 margins = new Vector4(5f, 5f, 2f, 0f);

        return Utility.TexturedStringField(color, position, _boxTexture, formStyle, margins, textInput);
    }

    /// <summary>
    /// Draws new task form placeholder.
    /// </summary>
    /// <param name="window">Parent window.</param>
    /// <param name="newTaskFormStyle">Form style.</param>
    /// <param name="settings">EasyToDo settings object.</param>
    private static void DrawFormPlaceholder(EditorWindow window, GUIStyle formStyle, EasyToDoSettings settings, Color color, string text, string textInput)
    {
        if (textInput != "")
        {
            return;
        }

        Rect placeholderLabelPosition = new Rect(56f, 52f, window.position.width - 56f, 40f);
        Rect correctionPlaceholderCoverBoxPosition = new Rect(window.position.width - 8f, 50f, 8f, 40f);

        Utility.Label(placeholderLabelPosition, formStyle, color, text);
        Utility.Box(correctionPlaceholderCoverBoxPosition, _boxTexture, settings.BackgroundColor);
    }

    /// <summary>
    /// Draws current task list UI element.
    /// </summary>
    private static void DrawTaskList(EditorWindow window)
    {
        // Debug.Log(_settings.CurrentListIndex);
        GUILayout.Space(100f);
        _scrollPosition = GUILayout.BeginScrollView(
            _scrollPosition, GUILayout.Height(window.position.height - 100f));

        if (_settings.CurrentListIndex >= 0 && _settings.CurrentListIndex < _manager.Lists.Count)
        {
            if (_manager.Lists[_settings.CurrentListIndex].Tasks.Count == 0)
            {
                Utility.Label(new Rect((window.position.width / 2) - 30f, NAVBAR_HEIGHT, 50f, 30f), "Empty ...");
            }
        }

        for (int i = 0; i < _manager.CountByIndex(_settings.CurrentListIndex); i++)
        {
            if (_footerPosition < (NAVBAR_HEIGHT + 50f + (i * 40f)))
            {
                DrawTask(window, i);
            }

            GUILayout.Space(40f);
        }
        GUILayout.EndScrollView();
    }

    /// <summary>
    /// Completed tasks have their line go out of bounds because CalcSize calculates it wrongly. This is helper function to hide that overflow.
    /// </summary>
    private static void DrawTaskListCorrectionBox(EditorWindow window)
    {
        Utility.Box(new Rect(window.position.width - 50f, TASK_LIST_OFFSET, 50f, window.position.height - TASK_LIST_OFFSET), _boxTexture, _settings.BackgroundColor);
    }

    /// <summary>h
    /// Draws specific task wit specific index.
    /// </summary>
    /// <param name="i">Index of task to be drawn.</param>
    private static void DrawTask(EditorWindow window, int index)
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
    private static void DrawTaskStatus(EditorWindow window, int index)
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
    private static void DrawTaskName(EditorWindow window, int index)
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

        Color defaultColor = GUI.color;
        GUI.color = new Color(1, 1, 1, _taskNameOpacity);
        GetTask(index).Name = Utility.TexturedStringField(_settings.BackgroundColor, position, _boxTexture, style, margins, text);
        GUI.color = defaultColor;

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
    /// Draws, specified by index, task name.
    /// </summary>
    /// <param name="window">Parent editor window.</param>
    /// <param name="index">>Targeted task index.</param>
    private static void DrawListName(EditorWindow window, int index)
    {
        GUIStyle style = Utility.DefaultLabelStyle();
        style.fontSize = 15;
        style.fontStyle = FontStyle.Bold;
        // float x = 28f;
        float x = 30f;
        float y = (LISTS_CARDS_BEGINNING + 66f) + (index * 50f);
        // float width = window.position.width - 102f;
        float width = 248;
        float height = 32f;
        Rect position = new Rect(x, y, width, height);
        var margins = new Vector4(10, 0, 0, 0);

        DrawColorBox(position, index);

        if (index >= 0 && index < _manager.Lists.Count)
        {
            if (index != 0)
            {
                DrawListDeleteButton(position, index);
            }

            if (index >= 0 && index < _manager.Lists.Count)
            {
                Color defaultColor = GUI.color;
                if (index == 0)
                {
                    GUI.color = new Color(1, 1, 1, 0.5f);
                }
                else
                {
                    GUI.color = new Color(1, 1, 1, _listNameOpacity);
                }

                var color = _settings.BackgroundColor;
                color.a = 1f;
                Utility.Box(position, _cardIconWideTexture, color);
                var defaultPosition = position;
                position.x = GetCurrentWindow().position.width - defaultPosition.width - (defaultPosition.x * 2);

                if (GetCurrentWindow().position.width > 335)
                {
                    Utility.Box(position, _cardIconWideTexture, color);
                }

                position = defaultPosition;
                position.x = defaultPosition.x + 20f;
                position.width = GetCurrentWindow().position.width - 120f;

                if (GetCurrentWindow().position.width <= 335f)
                {
                    position.width = GetCurrentWindow().position.width - 60f;
                }

                _manager.Lists[index].Name = Utility.TexturedStringField(color, position, _boxTexture, style, margins, _manager.Lists[index].Name);
                GUI.color = defaultColor;
            }
        }
    }

    /// <summary>
    /// Draws select list UI element.
    /// </summary>
    /// <param name="position">List name position.</param>
    /// <param name="index">Targeted list index.</param>
    private static void DrawColorBox(Rect position, int index)
    {
        position.x = 20f;
        position.width = 156f;
        if (_settings.ShowListsUnderline)
        {
            position.height = 32.5f;
        }

        Color color;

        if (index >= 0 && index < _settings.ListsColors.Count)
        {
            color = _settings.ListsColors[index];
        }
        else
        {
            color = Color.grey;
        }

        color.a = 0.5f;
        // position.x = 10f;
        // position.width = GetCurrentWindow().position.width - 20f;
        Utility.Box(position, _buttonIconRounded, color);
    }

    private static void SelectList(int index)
    {
        _settings.CurrentListIndex = index;
        ToggleListMenu();
    }

    /// <summary>
    /// Draws list delete button UI element.
    /// </summary>
    /// <param name="position">List Name UI element position.</param>
    /// <param name="index">Targeted list index.</param>
    private static void DrawListDeleteButton(Rect position, int index)
    {
        position.x = GetCurrentWindow().position.width - 58f;
        position.width = 40f;

        if (GetCurrentWindow().position.width > 335f)
        {
            Utility.Button(position, _buttonIconRoundedOneSide, _settings.BackgroundColor, () =>
            {
                RemoveList(index);
            });

            Utility.Box(position, _minusIconTexture, Color.white);
        }
    }

    private static void RemoveList(int index)
    {
        var name = _manager.Lists[index].Name;
        _manager.RemoveList(index);

        if (index >= 0 && index < _settings.ListsColors.Count)
        {
            Logger.Log("List removed: " + name);
        }

        SaveData();
    }

    /// <summary>
    /// Draws, specified by index, task corresponding delete button.
    /// </summary>
    /// <param name="window">Parent editor window.</param>
    /// <param name="index">>Targeted task index.</param>
    private static void DrawTaskDeleteButton(EditorWindow window, int index)
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
    private static void DrawListsView(EasyToDoWindow window)
    {
        if (_listViewHeight >= 0)
        {
            Utility.Box(new Rect(0, NAVBAR_HEIGHT, window.position.width, _listViewHeight), _boxTexture, _settings.BackgroundColor);
            // New List Input Form
            DrawFormInputGroup(window, ref _newListName, _settings.NewTaskFormTextColorNormal, _settings.NewTaskFormTextColorFocused,
            _settings.NewTaskPlaceholderColor, _placeholderListsText, _settings.NewTaskFormBackgroundColor);
            // Add List Button 
            DrawAddListButton();
            DrawToDoListsSelectionView(window);
            Utility.Box(new Rect(0, _footerPosition, window.position.width, _footerHeight), _boxTexture, _settings.NavbarColor);
        }
    }

    /// <summary>
    /// Updates the lists view position variables.
    /// </summary>
    /// <param name="window">Parent window.</param>
    private static void UpdateListsViewPositions(EditorWindow window)
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

        if (_footerPosition > GetCurrentWindow().position.height - 40f)
        {
            _listViewHeight -= LIST_VIEW_ANIMATION_FREQUENCY;

            _footerPosition -= LIST_VIEW_ANIMATION_FREQUENCY;
        }
    }

    /// <summary>
    /// Draws ToDoLists selection view.
    /// </summary>
    /// <param name="window"></param>
    private static void DrawToDoListsSelectionView(EditorWindow window)
    {
        var position = new Rect(0f, 0f, window.position.width, 40f);

        DrawLists(position);
    }

    /// <summary>
    /// Draws lists UI element.
    /// </summary>
    /// <param name="position">Lists</param>
    private static void DrawLists(Rect position)
    {
        // GUILayout.Space(100f);
        // _listsScrollPosition = GUILayout.BeginScrollView(
        //     _listsScrollPosition, GUILayout.Height(GetCurrentWindow().position.height - 100f));
        var viewIndex = 0;

        for (int i = 0; i < _manager.Lists.Count; i++)
        {
            position.y = (LISTS_CARDS_BEGINNING + 12f) + ((i + 1) * (position.height + LISTS_CARDS_OFFSET));

            if (_listViewHeight >= position.y)
            {
                // Color color = Color.grey;
                // Utility.Box(new Rect(10f, position.y, 248f, 40f), _buttonIconRounded, color);
                // Utility.Box(new Rect(50f, position.y, GetCurrentWindow().position.width - 100f, 40f), _boxTexture, color);
                // Utility.Box(new Rect(GetCurrentWindow().position.width - 258f, position.y, 248f, 40f), _buttonIconRounded, color);

                Utility.DrawCard(position, _boxTexture, new Color(0.5f, 0.5f, 0.5f, 0.1f), () =>
                {
                    DrawListsCardContent(position, i);
                });

                // GUILayout.Space(40f);
            }
            // else
            // {
            //     GUILayout.Space(-40f);
            // }
        }


        // var prev = new Rect(GetCurrentWindow().position.width - 50f, 10f, 20f, 20f);
        // var next = new Rect(GetCurrentWindow().position.width - 30f, 10f, 20f, 20f);

        // Utility.Button(prev, _boxTexture, _settings.BackgroundColor, () =>
        // {

        // });

        // Utility.Button(next, _boxTexture, _settings.BackgroundColor, () =>
        // {

        // });


        // GUILayout.EndScrollView();
    }

    /// <summary>
    /// Draws lists card content.
    /// </summary>
    /// <param name="position">Card position.</param>
    /// <param name="index">List index.</param>
    private static void DrawListsCardContent(Rect position, int index)
    {
        DrawListName(GetCurrentWindow(), index);
    }

    /// <summary>
    /// Removes task with index of value _indexTaskToDelete. If it's value is equal to -1 does nothing.
    /// </summary>
    private static void DeleteTaskByIndex()
    {
        if (_indexTaskToDelete == -1)
        {
            return;
        }

        _manager.RemoveTask(_settings.CurrentListIndex, _indexTaskToDelete);
        Logger.Log("Task deleted.");
        _indexTaskToDelete = -1;
        SaveData();
    }

    /// <summary>
    /// Returns task under targeted index of currently selected list. If index out of bounds returns null.
    /// </summary>
    /// <param name="index">Targeted task index.</param>
    /// <returns>Task under targeted index of currently selected project.</returns>
    private static Task GetTask(int index)
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
    private static bool? GetTaskStatus(int index)
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

        if (_manager.Lists.Count == 1 && _settings.ListsColors.Count == 0)
        {
            _settings.ListsColors.Add(Color.grey);
            EasyToDoSettingsWindow.SaveSettings();
        }

        _settings = EasyToDoSettingsWindow.LoadSettings();
    }

    /// <summary>
    /// Save to local data.
    /// </summary>
    private static void SaveData()
    {
        _manager.SaveToFile(Application.dataPath + DATA_FILE_NAME);
        EasyToDoSettingsWindow.SaveSettings();

        AssetDatabase.Refresh();
    }
}
