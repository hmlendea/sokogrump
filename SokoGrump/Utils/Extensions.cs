using System.ComponentModel;
using System.Drawing;

using Gtk;
using Gdk;

public static class MyExtensions
{
    /// <summary>
    /// Converts to bitmap.
    /// </summary>
    /// <returns>The bitmap.</returns>
    /// <param name="pix">Pix.</param>
    public static Bitmap ToBitmap(this Pixbuf pix)
    {
        TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
        return (Bitmap)tc.ConvertFrom(pix.SaveToBuffer("png")); 
    }

    /// <summary>
    /// Gets the width.
    /// </summary>
    /// <returns>The width.</returns>
    /// <param name="gdkWindow">Gdk window.</param>
    public static int GetWidth(this Gdk.Window gdkWindow)
    {
        int w, h;
        gdkWindow.GetSize(out w, out h);
        return w;
    }

    /// <summary>
    /// Gets the height.
    /// </summary>
    /// <returns>The height.</returns>
    /// <param name="gdkWindow">Gdk window.</param>
    public static int GetHeight(this Gdk.Window gdkWindow)
    {
        int w, h;
        gdkWindow.GetSize(out w, out h);
        return h;
    }

    /// <summary>
    /// Shows the message dialog.
    /// </summary>
    /// <param name="title">Title.</param>
    /// <param name="message">Message.</param>
    /// <param name="msgType">Message type.</param>
    /// <param name="btnType">Button type.</param>
    public static void ShowMessageDialog(string title, string message, MessageType msgType, ButtonsType btnType)
    {
        MessageDialog md = new MessageDialog(null, DialogFlags.Modal, msgType, btnType, message);
        md.Title = title;
        md.Run();
        md.Destroy();
    }
}