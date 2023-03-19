using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Manages all the ToDo lists and provides methods to add, remove, and serialize/deserialize ToDo lists.
/// </summary>
[System.Serializable]
public class ToDoManager
{
    [SerializeField] private List<ToDoList> _lists;

    public List<ToDoList> Lists
    {
        get { return this._lists; }
        set { this._lists = value; }
    }

    public ToDoManager()
    {
        _lists = new List<ToDoList>();
        AddList("Default");
    }

    /// <summary>
    /// Gives count of all lists managed by this manager.
    /// </summary>
    /// <returns>Lists count.</returns>
    public int Count()
    {
        return _lists.Count;
    }

    /// <summary>
    /// Count of tasks of specific list under desired index.
    /// </summary>
    /// <param name="index">Index of needed list.</param>
    /// <returns>Targeted list tasks count. If index is out of bounds -1.</returns>
    public int CountByIndex(int index)
    {
        if (index >= 0 && index < _lists.Count)
        {
            return _lists[index].Tasks.Count;
        }

        return -1;
    }

    /// <summary>
    /// Add a new ToDo list
    /// </summary>
    public void AddList()
    {
        _lists.Add(new ToDoList());

    }
    /// <summary>
    /// Adds new named ToDo list.
    /// </summary>
    /// <param name="name">List name.</param>
    /// 
    public void AddList(string name)
    {
        _lists.Add(new ToDoList(name));
    }

    /// <summary>
    /// Remove a ToDo list at the given index.
    /// </summary>
    /// <param name="index">Index of list to be removed</param>
    public void RemoveList(int index)
    {
        if (index >= 0 && index < _lists.Count)
        {
            Debug.Log(index);
            _lists.RemoveAt(index);
        }
    }

    /// <summary>
    /// Retrieves task from targeted list with targeted index.
    /// </summary>
    /// <param name="listIndex">Targeted list index.</param>
    /// <param name="taskIndex">Targeted task index.</param>
    /// <returns></returns>
    public Task GetTask(int listIndex, int taskIndex)
    {
        return _lists[listIndex].Tasks[taskIndex];
    }

    /// <summary>
    /// Add a new task to the given ToDo list
    /// </summary>
    /// <param name="listIndex">Index of the list to which task will be added to</param>
    /// <param name="taskName">New task name</param>
    public void AddTask(int listIndex, string taskName)
    {
        if (listIndex >= 0 && listIndex < _lists.Count)
        {
            _lists[listIndex].Tasks.Add(new Task(taskName, false));
        }
    }

    /// <summary>
    /// Remove a task at the given index from the given ToDo list
    /// </summary>
    /// <param name="listIndex">Index of the list from which task will be removed</param>
    /// <param name="taskIndex">Index of the task which will be removed</param>
    public void RemoveTask(int listIndex, int taskIndex)
    {
        if (listIndex >= 0 && listIndex < _lists.Count && taskIndex >= 0 && taskIndex < _lists[listIndex].Tasks.Count)
        {
            _lists[listIndex].Tasks.RemoveAt(taskIndex);
        }
    }

    /// <summary>
    /// Serialize the ToDo lists to a JSON string
    /// </summary>
    /// <returns>Json object with all lists managed by the manager</returns>
    public string Serialize()
    {
        return JsonUtility.ToJson(this);
    }

    /// <summary>
    /// Deserialize the ToDo lists from a JSON string
    /// </summary>
    /// <param name="json">json object for deserialization</param>
    public void Deserialize(string json)
    {
        JsonUtility.FromJsonOverwrite(json, this);
    }

    /// <summary>
    /// Save the ToDo lists to a file
    /// </summary>
    /// <param name="filePath">File path for local data</param>
    public void SaveToFile(string filePath)
    {
        string json = Serialize();
        File.WriteAllText(filePath, json);

        Logger.Log("Saved Data: " + json);
    }

    /// <summary>
    /// Load the ToDo lists from a file
    /// </summary>
    /// <param name="filePath">File path from which to load local data</param>
    public ToDoManager LoadFromFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Deserialize(json);

            Logger.Log("Loaded Data: " + json);

            return this;
        }
        else
        {
            Logger.Log("Generated New Data File.");

            var manager = new ToDoManager();
            SaveToFile(filePath);

            return manager;
        }
    }
}