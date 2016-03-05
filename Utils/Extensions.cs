using System.ComponentModel;
using System.Drawing;

using Gtk;
using Gdk;

public static class MyExtensions
{
    public static Bitmap ToBitmap(this Pixbuf pix)
    {
        TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
        return (Bitmap)tc.ConvertFrom(pix.SaveToBuffer("png")); 
    }

    public static int GetWidth(this Gdk.Window gdkWindow)
    {
        int w, h;
        gdkWindow.GetSize(out w, out h);
        return w;
    }

    public static int GetHeight(this Gdk.Window gdkWindow)
    {
        int w, h;
        gdkWindow.GetSize(out w, out h);
        return h;
    }

    public static void ShowMessageDialog(string title, string message, MessageType msgType, ButtonsType btnType)
    {
        MessageDialog md = new MessageDialog(null, DialogFlags.Modal, msgType, btnType, message);
        md.Title = title;
        md.Run();
        md.Destroy();
    }
}