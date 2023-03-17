using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

/// <summary>
/// Simulates fixed update inside of EditorWindow scripts. Call Enable() inside of OnEnable() and Disable() inside of OnDisable()
/// of targeted EditorWindow script.
/// </summary>
public class FixedUpdate : EditorWindow
{
    /// <summary>
    /// List of actions that will be executed once per _fixedUpdateTime interval.
    /// </summary>
    private List<Action> _actions = new List<Action>();
    /// <summary>
    /// Fixed interval tick frequency. 1.0f == 1s.
    /// </summary>
    private float _tickFrequency = 1.0f;
    /// <summary>
    /// Last update time.
    /// </summary>
    private float _lastUpdateTime = 0.0f;
    /// <summary>
    /// Modified once per _fixedUpdateTime interval. If _increment set to true _counter will increment once per _fixedUpdateTime interval, 
    /// otherwise it will decrement once per _fixedUpdateTime interval.
    /// </summary>
    private int _counter = 0;
    /// <summary>
    /// Decides if _counter will increment or decrement.
    /// </summary>
    private bool _increment = true;
    /// <summary>
    /// Enables or disables logs.
    /// </summary>
    private bool _log = false;

    public List<Action> Actions
    {
        get { return _actions; }
        set { this._actions = value; }
    }

    public float TickFrequency
    {
        get { return _tickFrequency; }
        set { this._tickFrequency = value; }
    }

    public int Counter
    {
        get { return _counter; }
        set { this._counter = value; }
    }

    /// <summary>
    /// Enable in OnEnable() of targeted editor window.
    /// </summary>
    public void Enable()
    {
        EditorApplication.update += this.EditorUpdate;
    }

    /// <summary>
    /// Disable in OnDisable() of targeted editor window.
    /// </summary>
    public void Disable()
    {
        EditorApplication.update -= this.EditorUpdate;
    }

    public void EditorUpdate()
    {
        if (EditorApplication.timeSinceStartup > this._lastUpdateTime + this._tickFrequency)
        {
            this._lastUpdateTime = (float)EditorApplication.timeSinceStartup;
            ExecuteUpdate(() =>
            {
                Execute();
            });
        }
    }

    /// <summary>
    /// Executes every Action in list of Actions (_actions) once.
    /// </summary>
    private void Execute()
    {
        for (int i = 0; i < this._actions.Count; i++)
        {
            this._actions[i]?.Invoke();

            Log("Executed function: " + ActionName(i));
        }
    }

    /// <summary>
    /// Logs message if _log flag is set to true.
    /// </summary>
    /// <param name="message"></param>
    private void Log(string message)
    {
        if (this._log)
        {
            Debug.Log(message);
        }
    }

    /// <summary>
    /// Returns Action name.
    /// </summary>
    /// <param name="i">Action index.</param>
    private string ActionName(int index)
    {
        Action name = this._actions[index];

        return nameof(name);
    }

    /// <summary>
    /// Will execute callback function once per _fixedUpdateTime interval.
    /// </summary>
    /// <param name="callback">Callback function.</param>
    /// <returns></returns>
    public void ExecuteUpdate(Action callback)
    {
        if (_increment)
        {
            this._counter++;
        }
        else
        {
            this._counter--;
        }

        callback?.Invoke();
    }

    /// <summary>
    /// Enables logs.
    /// </summary>
    public void EnableLogs()
    {
        this._log = true;
    }

    /// <summary>
    /// Disables logs.
    /// </summary>
    public void DisableLogs()
    {
        this._log = false;
    }

    /// <summary>
    /// Counter will increment from now on.
    /// </summary>
    public void Increment()
    {
        this._increment = true;
    }

    /// <summary>
    /// Counter will decrement from now on.
    /// </summary>
    public void Decrement()
    {
        this._increment = false;
    }

    /// <summary>
    /// Resets _counter value to 0.
    /// </summary>
    public void ResetCounter()
    {
        this._counter = 0;
    }

    /// <summary>
    /// Gets action under targeted index.
    /// </summary>
    /// <param name="index">Target action index.</param>
    /// <returns>Action under the index.</returns>
    public Action GetAction(int index)
    {
        return this._actions[index];
    }

    /// <summary>
    /// Adds Action to the end of the list of actions.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public FixedUpdate Push(Action action)
    {
        this._actions.Add(action);

        return this;
    }

    /// <summary>
    /// Gets last action in the list of actions, and removes it from the list.
    /// </summary>
    public Action Pop()
    {
        Action action = this._actions[this._actions.Count - 1];
        this._actions.RemoveAt(this._actions.Count - 1);

        return action;
    }

    /// <summary>
    /// Gets the first action from the list of actions and removes it from the list.
    /// </summary>
    /// <returns></returns>
    public Action PopFirst()
    {
        Action action = this._actions[0];
        this._actions.RemoveAt(0);

        return action;
    }
}