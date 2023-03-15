using UnityEditor;
using UnityEngine;

/// <summary>
/// Utility class with various helper functions.
/// </summary>
public class Utility
{
    /// <summary>
    /// Positions window at the center of the screen.
    /// </summary>
    /// <param name="window">Editor window to be positioned.</param>
    /// <param name="width">New window width.</param>
    /// <param name="height">New window height.</param>
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

    /// <summary>
    /// Sets background color for specified editor window.
    /// </summary>
    /// <param name="window">Editor window which background color will be changed.</param>
    /// <param name="color">Color to be set.</param>
    public static void SetBackgroundColor(EditorWindow window, Color color)
    {
        if (window == null)
        {
            return;
        }

        Color oldColor = GUI.backgroundColor;
        GUI.backgroundColor = color;

        GUILayout.BeginArea(new Rect(0, 0, window.position.width, window.position.height));
        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        GUILayout.EndArea();

        GUI.backgroundColor = oldColor;
    }

    /// <summary>
    /// Draw box UI.
    /// </summary>
    /// <param name="rect">Box position.</param>
    public static void Box(Rect rect)
    {
        GUI.Box(rect, GUIContent.none);
    }

    /// <summary>
    /// Draw box UI.
    /// </summary>
    /// <param name="rect">Box position.</param>
    /// <param name="color">Box color.</param>
    public static void Box(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.Box(rect, GUIContent.none);
        GUI.color = Color.white;
    }

    /// <summary>
    /// Draw box UI.
    /// </summary>
    /// <param name="rect">Box position.</param>
    /// <param name="text">Box text.</param>
    public static void Box(Rect rect, string text)
    {
        GUI.Box(rect, text);
    }

    /// <summary>
    /// Draw box UI.
    /// </summary>
    /// <param name="rect">Box position.</param>
    /// <param name="text">Box text.</param>
    /// <param name="color">Box color.</param>
    public static void Box(Rect rect, string text, Color color)
    {
        GUI.color = color;
        GUI.Box(rect, text);
        GUI.color = Color.white;
    }

    /// <summary>
    /// Draw box UI.
    /// </summary>
    /// <param name="rect">Box position.</param>
    /// <param name="content">Box content.</param>
    public static void Box(Rect rect, GUIContent content)
    {
        GUI.Box(rect, content);
    }

    /// <summary>
    /// Draw box UI.
    /// </summary>
    /// <param name="rect">Box position.</param>
    /// <param name="content">Box content.</param>
    /// <param name="color">Box color.</param>
    public static void Box(Rect rect, GUIContent content, Color color)
    {
        GUI.color = color;
        GUI.Box(rect, content);
        GUI.color = Color.white;
    }

    /// <summary>
    /// Draw textured box UI.
    /// </summary>
    /// <param name="position">Textured box position.</param>
    /// <param name="texture">Textured box texture.</param>
    public static void Box(Rect position, Texture2D texture)
    {
        GUI.DrawTexture(position, texture, ScaleMode.StretchToFill, true);
    }

    /// <summary>
    /// Draw textured box UI.
    /// </summary>
    /// <param name="rect">Textured box position.</param>
    /// <param name="texture">Textured box texture.</param>
    /// <param name="color">Textured box color.</param>
    public static void Box(Rect rect, Texture2D texture, Color color)
    {
        if (texture != null)
        {
            GUI.color = color;
            GUI.DrawTexture(rect, texture);
            GUI.color = Color.white;
        }
    }

}