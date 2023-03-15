using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Manages all the ToDo lists and provides methods to add, remove, and serialize/deserialize ToDo lists.
/// </summary>
public class ToDoManager : MonoBehaviour
{
    public List<ToDoList> lists;

    /// <summary>
    /// Add a new ToDo list
    /// </summary>
    public void AddList()
    {
        lists.Add(new ToDoList());
    }

    /// <summary>
    /// Remove a ToDo list at the given index
    /// </summary>
    /// <param name="index">Index of list to be removed</param>
    public void RemoveList(int index)
    {
        if (index >= 0 && index < lists.Count)
        {
            lists.RemoveAt(index);
        }
    }

    /// <summary>
    /// Add a new task to the given ToDo list
    /// </summary>
    /// <param name="listIndex">Index of the list to which task will be added to</param>
    /// <param name="taskName">New task name</param>
    public void AddTask(int listIndex, string taskName)
    {
        if (listIndex >= 0 && listIndex < lists.Count)
        {
            lists[listIndex].tasks.Add(new Task(taskName, false));
        }
    }

    /// <summary>
    /// Remove a task at the given index from the given ToDo list
    /// </summary>
    /// <param name="listIndex">Index of the list from which task will be removed</param>
    /// <param name="taskIndex">Index of the task which will be removed</param>
    public void RemoveTask(int listIndex, int taskIndex)
    {
        if (listIndex >= 0 && listIndex < lists.Count && taskIndex >= 0 && taskIndex < lists[listIndex].tasks.Count)
        {
            lists[listIndex].tasks.RemoveAt(taskIndex);
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
    }

    /// <summary>
    /// Load the ToDo lists from a file
    /// </summary>
    /// <param name="filePath">File path from which to load local data</param>
    public void LoadFromFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Deserialize(json);
        }
    }
}