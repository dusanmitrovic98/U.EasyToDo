using UnityEngine;
/// <summary>
/// Represents a single task and stores its name and completion status.
/// </summary>
[System.Serializable]
public class Task
{
    [SerializeField] private string _name;
    [SerializeField] private bool _completed;

    public string Name
    {
        get { return this._name; }
        set { this._name = value; }
    }

    public bool Completed
    {
        get { return this._completed; }
        set { this._completed = value; }
    }

    public Task()
    {
        this._name = "";
        this._completed = false;
    }

    public Task(string name)
    {
        this._name = name;
        this._completed = false;
    }

    public Task(bool completed)
    {
        this._name = "";
        this._completed = completed;
    }

    public Task(string name, bool completed)
    {
        this._name = name;
        this._completed = completed;
    }
}