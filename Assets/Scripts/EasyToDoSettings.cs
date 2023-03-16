using UnityEngine;

/// <summary>
/// Class that represents the EasyToDo settings. 
/// </summary>
public class EasyToDoSettings
{
    // Current list
    [SerializeField] private int _currentListIndex;
    // Task status
    [SerializeField] private bool _showCompletedTasks;
    // Console Logs
    [SerializeField] private bool _enableLogging;
    // Background
    [SerializeField] private Color _backgroundColor;
    // Navbar
    [SerializeField] private Color _navbarColor;
    // Menu Icon
    [SerializeField] private Color _menuIconColor;
    // New Task Form
    [SerializeField] private Color _newTaskFormTextColorNormal;
    [SerializeField] private Color _newTaskFormTextColorFocused;
    [SerializeField] private Color _newTaskFormBackgroundColor;
    [SerializeField] private Color _newTaskFormBackgroundIconColor;
    [SerializeField] private Color _newTaskPlaceholderColor;
    // Task View
    [SerializeField] private Color _taskStatusInactiveOuterCircleColor;
    [SerializeField] private Color _deleteTaskButtonColor;

    public int CurrentListIndex
    {
        get { return this._currentListIndex; }
        set { this._currentListIndex = value; }
    }

    public bool ShowCompletedTasks
    {
        get { return this._showCompletedTasks; }
        set { this._showCompletedTasks = value; }
    }

    public bool EnableLogging
    {
        get { return this._enableLogging; }
        set { this._enableLogging = value; }
    }

    public Color BackgroundColor
    {
        get { return this._backgroundColor; }
        set { this._backgroundColor = value; }
    }

    public Color NavbarColor
    {
        get { return this._navbarColor; }
        set { this._navbarColor = value; }
    }

    public Color MenuIconColor
    {
        get { return this._menuIconColor; }
        set { this._menuIconColor = value; }
    }

    public Color NewTaskFormTextColorNormal
    {
        get { return this._newTaskFormTextColorNormal; }
        set { this._newTaskFormTextColorNormal = value; }
    }

    public Color NewTaskFormTextColorFocused
    {
        get { return this._newTaskFormTextColorFocused; }
        set { this._newTaskFormTextColorFocused = value; }
    }

    public Color NewTaskFormBackgroundColor
    {
        get { return this._newTaskFormBackgroundColor; }
        set { this._newTaskFormBackgroundColor = value; }
    }

    public Color NewTaskFormBackgroundIconColor
    {
        get { return this._newTaskFormBackgroundIconColor; }
        set { this._newTaskFormBackgroundIconColor = value; }
    }

    public Color NewTaskPlaceholderColor
    {
        get { return this._newTaskPlaceholderColor; }
        set { this._newTaskPlaceholderColor = value; }
    }

    public Color TaskStatusInactiveOuterCircleColor
    {
        get { return this._taskStatusInactiveOuterCircleColor; }
        set { this._taskStatusInactiveOuterCircleColor = value; }
    }

    public Color DeleteTaskButtonColor
    {
        get { return this._deleteTaskButtonColor; }
        set { this._deleteTaskButtonColor = value; }
    }

    public EasyToDoSettings()
    {
        // List index
        _currentListIndex = 0;
        // Task Status
        _showCompletedTasks = true;
        // Console Logs
        _enableLogging = true;
        // Background
        _backgroundColor = new Color(0.211f, 0.207f, 0.42f, 4);
        // Navbar
        _navbarColor = new Color(0.149f, 0.145f, 0.360f, 4);
        // Menu Icon
        _menuIconColor = Color.white;
        // New Task Form
        _newTaskFormTextColorNormal = new Color(0.6f, 0.4f, 0.8f);
        _newTaskFormTextColorFocused = new Color(0.36f, 0.04f, 0.58f);
        _newTaskFormBackgroundColor = Color.white;
        _newTaskFormBackgroundIconColor = new Color(0.52f, 0.49f, 0.61f);
        _newTaskPlaceholderColor = new Color(0.56f, 0.53f, 0.65f);
        // Task View
        // _taskStatusInactiveOuterCircleColor = new Color(0.50f, 0.40f, 0.60f);
        _taskStatusInactiveOuterCircleColor = Color.white;
        _deleteTaskButtonColor = new Color(0.16f, 0.16f, 0.37f);
    }
}