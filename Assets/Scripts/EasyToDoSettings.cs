using UnityEngine;

/// <summary>
/// Class that represents the EasyToDo settings.
/// </summary>
public class EasyToDoSettings
{
    public bool showCompletedTasks;
    public Color backgroundColor;

    public EasyToDoSettings()
    {
        showCompletedTasks = true;
        backgroundColor = Color.yellow;
    }
}