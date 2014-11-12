using System.Collections.Generic;
using System.Collections.ObjectModel;
using AjaxControlToolkit.HTMLEditor;
using AjaxControlToolkit.HTMLEditor.ToolbarButton;

namespace Arcomage.Server
{

    

    public class CustomEditor : Editor
    {

        protected override void FillTopToolbar()

        {

            TopToolbar.Buttons.Add(new Bold());

            TopToolbar.Buttons.Add(new Italic());
          //  TopToolbar.Buttons.Add(new Underline());

            TopToolbar.Buttons.Add(new Undo());
            TopToolbar.Buttons.Add(new Redo());

            TopToolbar.Buttons.Add(new ForeColor());
            TopToolbar.Buttons.Add(new ForeColorSelector());

         //   TopToolbar.Buttons.Add(new OrderedList());
         //   TopToolbar.Buttons.Add(new BulletedList());

           // TopToolbar.Buttons.Add(new AjaxControlToolkit.HTMLEditor.ToolbarButton.FontName());

            var fontSize = new FontSize();
            TopToolbar.Buttons.Add(fontSize);

            Collection<SelectOption> options = new Collection<SelectOption>();
           
            options = fontSize.Options;

            var option = new SelectOption();
            option.Value = "8pt";
            option.Text = "1 ( 8 pt)";
            options.Add(option);

            var option2 = new SelectOption();
            option2.Value = "9pt";
            option2.Text = "9 ( 9 pt)";
            options.Add(option2);

            var option3 = new SelectOption();
            option3.Value = "10pt";
            option3.Text = "10 ( 11 pt)";
            options.Add(option3);
            /*
            TopToolbar.FontSize fontSize = new AjaxControlToolkit.HTMLEditor.ToolbarButton.FontSize();
            TopToolbar.Buttons.Add(fontSize);
            options = fontSize.Options;
            option = new AjaxControlToolkit.HTMLEditor.ToolbarButton.SelectOption();
            option.Value = "8pt";
            option.Text = "1 ( 8 pt)";
            options.Add(option);

            TopToolbar.Buttons.Add(new AjaxControlToolkit.HTMLEditor.ToolbarButton.FontSize());*/

        }

 

        protected override void FillBottomToolbar()

        {

            //BottomToolbar.Buttons.Add(new AjaxControlToolkit.HTMLEditor.ToolbarButton.DesignMode());

           // BottomToolbar.Buttons.Add(new AjaxControlToolkit.HTMLEditor.ToolbarButton.PreviewMode());       

        }

    }
}