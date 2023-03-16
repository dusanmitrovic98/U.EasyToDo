using UnityEngine;

public static class Logger
{
    private static bool _log = false;

    public static void EnableLogging()
    {
        _log = true;
    }

    /// <summary>
    /// Logs a message to the Unity console with the specified prefix, message, and suffix.
    /// </summary>
    /// <param name="prefix">The prefix to prepend to the message.</param>
    /// <param name="message">The message to log.</param>
    /// <param name="suffix">The suffix to append to the message.</param>
    /// <param name="color">The color to use for the message text.</param>
    public static void Log(string prefix, string message, string suffix, Color color)
    {
        string formattedMessage = prefix + message + suffix;

        if (_log)
        {
            Debug.Log(formattedMessage);
        }
    }

    /// <summary>
    /// Logs a message to the Unity console with the specified message and color.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="color">The color to use for the message text.</param>
    public static void Log(string message, Color color)
    {
        Log("", message, "", color);
    }

    /// <summary>
    /// Logs a message to the Unity console with the specified message and default color.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public static void Log(string message)
    {
        Log("", message, "", Color.white);
    }
}
