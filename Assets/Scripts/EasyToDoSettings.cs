using UnityEngine;

/// <summary>
/// Class that represents the EasyToDo settings. 
/// </summary>
public class EasyToDoSettings
{
    public bool showCompletedTasks;
    public Color backgroundColor;
    public Color navbarColor;

    public EasyToDoSettings()
    {
        showCompletedTasks = true;
        backgroundColor = new Color(38, 37, 92, 4);
        navbarColor = new Color(54, 53, 109, 4);
    }
}