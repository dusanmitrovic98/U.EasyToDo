using UnityEditor;
using UnityEngine;
using System;

/// <summary>
/// Utility class with various helper functions.
/// </summary>
public class Utility
{
    /// <summary>
    /// Remove focus when focused on GUI element.
    /// </summary>
    public static void RemoveFocus()
    {
        if (Event.current.type == EventType.MouseDown)
        {
            GUI.FocusControl(null);
        }
    }

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
    /// Draws label UI element.
    /// </summary>
    /// <param name="position">Label position.</param>
    /// <param name="text">Label content.</param>
    public static void Label(Rect position, string text)
    {
        GUI.Label(position, text);
    }

    /// <summary>
    /// Draws label UI element.
    /// </summary>
    /// <param name="position">Label position.</param>
    /// <param name="color">Label text color.</param>
    /// <param name="text">Label content.</param>
    public static void Label(Rect position, Color color, string text)
    {
        var defaultColor = GUI.color;
        GUI.color = color;
        GUI.Label(position, text);
        GUI.color = defaultColor;
    }

    /// <summary>
    /// Draws label UI element.
    /// </summary>
    /// <param name="position">Label position.</param>
    /// <param name="style">Label style.</param>
    /// <param name="color">Label text color.</param>
    /// <param name="text">Label content.</param>
    public static void Label(Rect position, GUIStyle style, Color color, string text)
    {
        var defaultColor = GUI.color;
        GUI.color = color;
        GUI.Label(position, text, style);
        GUI.color = defaultColor;
    }

    /// <summary>
    /// Draw box UI element.
    /// </summary>
    /// <param name="rect">Box position.</param>
    public static void Box(Rect rect)
    {
        GUI.Box(rect, GUIContent.none);
    }

    /// <summary>
    /// Draw box UI element.
    /// </summary>
    /// <param name="rect">Box position.</param>
    /// <param name="color">Box color.</param>
    public static void Box(Rect rect, Color color)
    {
        var defaultColor = GUI.color;
        GUI.color = color;
        GUI.Box(rect, GUIContent.none);
        GUI.color = defaultColor;
    }

    /// <summary>
    /// Draw box UI element.
    /// </summary>
    /// <param name="rect">Box position.</param>
    /// <param name="text">Box text.</param>
    public static void Box(Rect rect, string text)
    {
        GUI.Box(rect, text);
    }

    /// <summary>
    /// Draw box UI element.
    /// </summary>
    /// <param name="rect">Box position.</param>
    /// <param name="text">Box text.</param>
    /// <param name="color">Box color.</param>
    public static void Box(Rect rect, string text, Color color)
    {
        var defaultColor = GUI.color;
        GUI.color = color;
        GUI.Box(rect, text);
        GUI.color = defaultColor;
    }

    /// <summary>
    /// Draw box UI element.
    /// </summary>
    /// <param name="rect">Box position.</param>
    /// <param name="content">Box content.</param>
    public static void Box(Rect rect, GUIContent content)
    {
        GUI.Box(rect, content);
    }

    /// <summary>
    /// Draw box UI element.
    /// </summary>
    /// <param name="rect">Box position.</param>
    /// <param name="content">Box content.</param>
    /// <param name="color">Box color.</param>
    public static void Box(Rect rect, GUIContent content, Color color)
    {
        var defaultColor = GUI.color;
        GUI.color = color;
        GUI.Box(rect, content);
        GUI.color = defaultColor;
    }

    /// <summary>
    /// Draw textured box UI element.
    /// </summary>
    /// <param name="position">Textured box position.</param>
    /// <param name="texture">Textured box texture.</param>
    public static void Box(Rect position, Texture2D texture)
    {
        GUI.DrawTexture(position, texture, ScaleMode.StretchToFill, true);
    }

    /// <summary>
    /// Draw textured box UI element.
    /// </summary>
    /// <param name="rect">Textured box position.</param>
    /// <param name="texture">Textured box texture.</param>
    /// <param name="color">Textured box color.</param>
    public static void Box(Rect rect, Texture2D texture, Color color)
    {
        if (texture != null)
        {
            var defaultColor = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(rect, texture);
            GUI.color = defaultColor;
        }
    }

    /// <summary>
    /// Draw button with text and executes the given callback function when the button is clicked.
    /// </summary>
    /// <param name="rect">The position and size of the button.</param>
    /// <param name="text">The text to use as the button content.</param>
    /// <param name="callback">The function to execute when the button is clicked.</param>
    public static bool Button(Rect rect, string text, Action callback)
    {
        if (GUI.Button(rect, text))
        {
            callback?.Invoke();

            return true;
        }

        return false;
    }

    /// <summary>
    /// Draws a GUI button with texture and executes the given callback function when the button is clicked.
    /// </summary>
    /// <param name="rect">The position and size of the button.</param>
    /// <param name="texture">The texture to use as the button background.</param>
    /// <param name="callback">The function to execute when the button is clicked.</param>
    public static bool Button(Rect rect, Texture2D texture, Color color, Action callback)
    {
        if (GUI.Button(rect, GUIContent.none, GUIStyle.none))
        {
            callback.Invoke();

            return true;
        }

        var defaultColor = GUI.color;
        GUI.color = color;
        GUI.DrawTexture(rect, texture);
        GUI.color = defaultColor;

        return false;
    }

    public static bool Button(Rect rect, Texture2D texture, GUIStyle style, Action callback)
    {
        if (GUI.Button(rect, GUIContent.none, style))
        {
            callback.Invoke();

            return true;
        }

        GUI.DrawTexture(rect, texture);

        return false;
    }

    /// <summary>
    /// Draws a GUI string input field without a label and without a default value.
    /// </summary>
    /// <param name="rect">The position and size of the input field.</param>
    /// <returns>The value entered by the user.</returns>
    public static string StringField(Rect rect)
    {
        return GUI.TextField(rect, "");
    }

    /// <summary>
    /// Draws a GUI string input field without a label but with a default value.
    /// </summary>
    /// <param name="rect">The position and size of the input field.</param>
    /// <param name="value">The default value to display in the input field.</param>
    /// <returns>The value entered by the user.</returns>
    public static string StringField(Rect rect, string value)
    {
        return GUI.TextField(rect, value);
    }

    /// <summary>
    /// Draws a GUI string input field with a label but without a default value.
    /// </summary>
    /// <param name="rect">The position and size of the input field.</param>
    /// <param name="label">The label to display before the input field.</param>
    /// <returns>The value entered by the user.</returns>
    public static string LabeledStringField(Rect rect, string label)
    {
        GUI.Label(rect, label);
        return GUI.TextField(rect, "");
    }

    /// <summary>
    /// Draws a GUI string input field with a label and default value.
    /// </summary>
    /// <param name="rect">The position and size of the input field.</param>
    /// <param name="label">The label to display before the input field.</param>
    /// <param name="value">The default value to display in the input field.</param>
    /// <returns>The value entered by the user.</returns>
    public static string StringField(Rect rect, string label, string value)
    {
        GUI.Label(rect, label);
        return GUI.TextField(rect, value);
    }

    /// <summary>
    /// Draws textured string field UI.
    /// </summary>
    /// <param name="position">Position.</param>
    /// <param name="text">Text content.</param>
    /// <param name="texture">background texture.</param>
    /// <param name="style">Style.</param>
    /// <param name="fieldMargins">Text margins. Order Left, Right, Top and Bottom.</param>
    /// <returns></returns>
    public static string TexturedStringField(Color backgroundColor, Rect position, Texture2D texture, GUIStyle style, Vector4 fieldMargins, string text = "")
    {
        float textureSize = position.height * 0.8f;
        Rect textureRect = new Rect(position.x, position.y, position.width, position.height);

        var defaultColor = GUI.color;
        GUI.color = backgroundColor;
        GUI.DrawTexture(textureRect, texture);
        GUI.color = defaultColor;

        Rect textFieldRect = new Rect(position.x + fieldMargins.x, position.y + fieldMargins.z, position.width - (fieldMargins.y + fieldMargins.x), position.height - (fieldMargins.w + fieldMargins.z));

        return GUI.TextField(textFieldRect, text, style);
    }

    /// <summary>
    /// Generates default label style.
    /// </summary>
    /// <param name="fontSize">Font size.</param>
    /// <param name="fontStyle">Font style.</param>
    /// <param name="wordWrap">Word wrap.</param>
    /// <param name="textAnchor">Text anchor.</param>
    /// <returns></returns>
    public static GUIStyle DefaultLabelStyle(int fontSize = 14, FontStyle fontStyle = FontStyle.Normal, bool wordWrap = false, TextAnchor textAnchor = TextAnchor.LowerLeft)
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);

        style.normal.textColor = Color.white;
        style.focused.textColor = Color.white;
        style.fontSize = fontSize;
        style.fontStyle = fontStyle;
        style.wordWrap = wordWrap;
        style.alignment = TextAnchor.MiddleLeft;

        return style;
    }

    /// <summary>
    /// Style text colors of the style.
    /// </summary>
    /// <param name="style">Style to be modified.</param>
    /// <param name="normalColor">Normal color.</param>
    /// <param name="focusedBackground">Color used on focus.</param>
    /// <returns></returns>
    public static GUIStyle StyleTextColors(GUIStyle style, Color normalColor, Color focusedBackground)
    {
        style.normal.textColor = normalColor;
        style.focused.textColor = focusedBackground;

        return style;
    }

    /// <summary>
    /// Sets normal texture of the style.
    /// </summary>
    /// <param name="style">Style to be modified.</param>
    /// <param name="texture">Normal texture.</param>
    private static void StyleNormalTexture(GUIStyle style, Texture2D texture)
    {
        if (texture == null)
        {
            return;
        }

        style.normal.background = texture;
    }

    /// <summary>
    /// Sets focused texture of the style.
    /// </summary>
    /// <param name="style">Style to be modified.</param>
    /// <param name="texture">Texture used on focus.</param>
    private static void StyleFocusedTexture(GUIStyle style, Texture2D texture)
    {
        if (texture == null)
        {
            return;
        }

        style.focused.background = texture;
    }

    /// <summary>
    /// Changes normal texture and focused texture of the style.
    /// </summary>
    /// <param name="style">Style to be modified.</param>
    /// <param name="normalTexture">Normal texture.</param>
    /// <param name="focusedTexture">Texture used on focus.</param>
    /// <returns></returns>
    public static GUIStyle StyleBackgroundColors(GUIStyle style, Texture2D normalTexture = null, Texture2D focusedTexture = null)
    {
        StyleNormalTexture(style, normalTexture);

        StyleFocusedTexture(style, focusedTexture);

        return style;
    }
}