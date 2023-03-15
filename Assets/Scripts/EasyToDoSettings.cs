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
        backgroundColor = new Color(0.211f, 0.207f, 0.42f, 4);
        navbarColor = new Color(0.149f, 0.145f, 0.360f, 4);
    }
}