using AjaxControlToolkit.HTMLEditor;

namespace Arcomage.Server
{

    

    public class CustomEditor : Editor
    {

        protected override void FillTopToolbar()

        {

            TopToolbar.Buttons.Add(new AjaxControlToolkit.HTMLEditor.ToolbarButton.Bold());

            TopToolbar.Buttons.Add(new AjaxControlToolkit.HTMLEditor.ToolbarButton.Italic());
            TopToolbar.Buttons.Add(new AjaxControlToolkit.HTMLEditor.ToolbarButton.Underline());

            TopToolbar.Buttons.Add(new AjaxControlToolkit.HTMLEditor.ToolbarButton.Undo());
            TopToolbar.Buttons.Add(new AjaxControlToolkit.HTMLEditor.ToolbarButton.Redo());

            TopToolbar.Buttons.Add(new AjaxControlToolkit.HTMLEditor.ToolbarButton.OrderedList());
            TopToolbar.Buttons.Add(new AjaxControlToolkit.HTMLEditor.ToolbarButton.BulletedList());

            TopToolbar.Buttons.Add(new AjaxControlToolkit.HTMLEditor.ToolbarButton.FontName());
            TopToolbar.Buttons.Add(new AjaxControlToolkit.HTMLEditor.ToolbarButton.FontSize());

        }

 

        protected override void FillBottomToolbar()

        {

            //BottomToolbar.Buttons.Add(new AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignMode());

           // BottomToolbar.Buttons.Add(new AjaxControlToolkit.HTMLEditor.ToolbarButton.PreviewMode());       

        }

    }
}