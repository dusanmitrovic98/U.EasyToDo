using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a list of ToDo tasks and stores a list of ToDo objects.
/// </summary>
[System.Serializable]
public class ToDoList
{
    [SerializeField] private List<Task> _tasks;

    public List<Task> Tasks
    {
        get { return _tasks; }
        set { _tasks = value; }
    }

    public ToDoList()
    {
        this._tasks = new List<Task>();
    }
}