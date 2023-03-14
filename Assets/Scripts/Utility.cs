using UnityEditor;
using UnityEngine;

/// <summary>
/// Utility class with various helper functions.
/// </summary>
public class Utility
{
    /// <summary>
    /// Positions window at the center of the screen
    /// </summary>
    /// <param name="window">Editor window to be positioned</param>
    /// <param name="width">New window width</param>
    /// <param name="height">New window height</param>
    public static Rect CenterWindow(EditorWindow window, float width, float height)
    {
        {
            var center = new Vector2(Screen.currentResolution.width / 2f, Screen.currentResolution.height / 2f);
            var size = new Vector2(width, height);
            var position = new Rect(center - size / 2f, size);
            window.position = position;

            return position;
        }
    }
}