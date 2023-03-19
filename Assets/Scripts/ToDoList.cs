using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a list of ToDo tasks and stores a list of ToDo objects.
/// </summary>
[System.Serializable]
public class ToDoList
{
    [SerializeField] private string _name;
    [SerializeField] private List<Task> _tasks;

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public List<Task> Tasks
    {
        get { return _tasks; }
        set { _tasks = value; }
    }

    public ToDoList()
    {
        this._name = "";
        this._tasks = new List<Task>();
    }

    public ToDoList(string name)
    {
        this._name = name;
        this._tasks = new List<Task>();
    }
}