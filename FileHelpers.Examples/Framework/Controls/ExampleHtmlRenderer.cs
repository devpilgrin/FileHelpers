﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ColorCode;


namespace ExamplesFramework
{
    public partial class ExampleHtmlRenderer : UserControl
    {
        public ExampleHtmlRenderer()
        {
            InitializeComponent();
            //DoubleBuffered = true;
        }

        private ExampleCode mExample;
        public ExampleCode Example
        {
            get { return mExample; }
            set
            {
                if (mExample == value)
                    return;

                mExample = value;
                RenderExample();
            }
        }

        private void RenderExample()
        {
            this.SuspendLayout();
            
            Clear();

            //lblDescription.Text = Example.Description;
            cmdRunDemo.Visible = Example.Runnable;
            browserExample.DocumentText = ExampleToHtml();
            this.ResumeLayout();

            if (Example.AutoRun)
                RunExample();
        }

        private void RunExample()
        {
            if (Example == null)
                return;
            try
            {
                Example.ConsoleChanged += Example_ConsoleChanged;
                Example.AddedFile += FileHandler;
                Example.RunExample();
            }
            finally  
            {
                Example.AddedFile -= FileHandler;
                Example.ConsoleChanged -= Example_ConsoleChanged;
            }}

        void Example_ConsoleChanged(object sender, EventArgs e)
        {
            var consoleRes = new StringBuilder();

            AddHeader(consoleRes);

            consoleRes.AppendLine(@"<body style=""margin:0px;background-color:#000;color:#DDD;"">");

//            consoleRes.AppendLine(@"<div class=""FileLeft"">&nbsp;</div>
//<div class=""FileMiddle"">Console</div>
//<div class=""FileRight"">&nbsp;</div><br/>
//<div style=""clear:both""></div>");

consoleRes.AppendLine(@"<div id=""consola""><pre style=""background-color:#000;color:#DDD;border: 0px;"">" + Example.Example.Console.Output + "</pre></div></body>");
            browserOutput.DocumentText = consoleRes.ToString();

            splitContainer1.Panel2Collapsed = false;

            //var console = browserOutput.Document.GetElementById("consola");

            //if (console != null)
            //    console.InnerHtml = "<pre>"+  Example.Example.Console.Output + "</pre>";
        }

        CodeColorizer mColorizer = new CodeColorizer();
        private string ExampleToHtml()
        {
          

            var res = new StringBuilder();

            AddHeader(res);

            res.AppendLine("<h2>" + Example.Name + "</h2>");

            res.AppendLine("<div class=\"DescriptionBox\"><div class=\"DescriptionIcon\">" + Example.Description + "</div></div>");

            res.AppendLine("<div style=\"margin-left:10px;\">");

            for (int i = 0; i < Example.Files.Count; i++)
            {
                var file = Example.Files[i];
                //res.AppendLine("<BR/>");

                res.AppendLine(@"
<div class=""FileLeft"">&nbsp;</div>
<div class=""FileMiddle"">" + file.Filename + @"</div>
<div class=""FileRight"">&nbsp;</div><br/>
<div style=""clear:both""></div>
");
                //mColorizer.Colorize()
                res.AppendLine(mColorizer.Colorize(file.Contents, Languages.CSharp));
                
                res.AppendLine("<BR/>");
            }
            res.AppendLine("</div>");
            return res.ToString();
        }

        private void AddHeader(StringBuilder res)
        {
            res.AppendLine(@"
<style type=""text/css"">


body, div, th, td, form {
font-family: Verdana, Helvetica, Arial;
font-size: 12px;
}
h2 {
font-size: 14pt;
font-weight: bold;
color: #07A;
}

pre {
background-color: #EEF3F9;
border: 1px dashed grey;
font-family: Consolas,""Courier New"",Courier,Monospace !important;
font-size: 12px !important;
margin-top: 0;
padding: 6px 6px 6px 6px;
height: auto;
overflow: auto;
width: 100% !important;
}

.FileLeft {
width: 20px;
height: 20px;
background-image:url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAMAAAC6V+0/AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAMAUExURTFyADR3ADd7ADp/AD2DAEqNDFCSEFOWEFaaEFmeEV2UMF+XMGSVP2KaMGCkF2irH2yiOnGnPXCyKHiwPni4MH61Q4C+OYS7SYjEQY/JSovAUJHFV5bOUqvWd7DafbrQqtzvxfL37/T48Pb68fb68v///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAO/tYtUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAXdEVYdFNvZnR3YXJlAFBhaW50Lk5FVCB2My41vUWjyQAAAH1JREFUKFNtykkWwjAMA1BByjwUAh0YS0J8/ytiKu/qv7GkZ3zezwm8blPoHWgduFIpD0t6EEmk3C3GiJpE5NtZrnEkHSU3VrCh/yj5woI1jaNkFqzIRhYsiY9nFixo3E5WUJFu6WC5QiDddhZDwJxS2lrSg5kDrr23Ds76AwmLFuNBIOa/AAAAAElFTkSuQmCC);
background-repeat: no-repeat;
float: left;}



.FileMiddle
{
font-family: Consolas,""Courier New"",Courier,Monospace !important;
font-size: 17px !important;
width: 250px;
height: 20px;
background-image:url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAAUCAMAAAB70KeTAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAMAUExURTFyADR3ADd7ADp/AD2DAEqNDFCSEFOWEFaaEFmeEWCkF2irH3CyKHi4MIC+OYjEQY/JSpbOUgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACQ6tVUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAXdEVYdFNvZnR3YXJlAFBhaW50Lk5FVCB2My41vUWjyQAAACdJREFUGFcdwYcNgCAAALAKMlWQ/58lobX8puHzenRNVWTJLQouxwYRFwCaak8OWAAAAABJRU5ErkJggg%3D%3D);
background-repeat: repeat-x;
float: left;
color: white;
text-align: left;}

.FileRight {
width: 20px;
height: 20px;
background-image:url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAMAAAC6V+0/AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAMAUExURTFyADR3ADd7ADp/AD2DAEqNDFCSEFOWEFaaEFmeEWSVP2CkF2irH3CyKHi4MIC+OYjEQY/JSpbOUqvWd7DafbrQqtzvxQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAFUnrx4AAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAXdEVYdFNvZnR3YXJlAFBhaW50Lk5FVCB2My41vUWjyQAAAD9JREFUKFONyscBgCAAwMAgFsSCff9RGYA8vPdxNZ6Ps3VzCIpgF2yCVbAIZkESTIJRMAh6QRR0giD4K0vMbwXF4QzGlCzu7wAAAABJRU5ErkJggg%3D%3D);
background-repeat: no-repeat;
float: left;
}


.DescriptionBox {
border: 1px solid #D4D4D4;
margin: 10px;
padding: 5px;
background-image:url(data:image/gif;base64,R0lGODlhAQAtANUAAAAAAP////n7/+fv/fD1/vP3/ujw/erx/ezz/uvy/e70/vH2/vf6/+bv/efw/enx/e30/u/1/vT4/vr8/+jx/ery/fD2/vL3/vj7/+71/vP4/vb6//X5/vH3/vf7//n8//v9//r9/////wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACH5BAEAACIALAAAAAABAC0AAAYqQBAoNJl8PgIMxsPYbDgciURTuHQWFkIko4BAEIhEonJ4PCgGg8MxaAQBADs%3D);
background-repeat: repeat-x;
background-position: bottom;
background-color: #FBFDFF;
}

.DescriptionIcon {
 min-height: 45px;
  height: auto !important;
  height: 45px;
vertical-align: middle;
padding: 5px;
padding-left: 60px;
background-image:url(data:image/png;base64,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%3D);
background-repeat: no-repeat;
float:left:
}

.DescriptionText {
/*width:90%;*/
/*margin-left:10px;*/
float:left:
}

</style>");
        }


        public void Clear()
        {
            cmdRunDemo.Visible = false;
            browserExample.DocumentText = string.Empty;
            browserOutput.DocumentText = string.Empty;
            splitContainer1.Panel2Collapsed = true;
        }

        private void cmdRunDemo_Click(object sender, EventArgs e)
        {
            RunExample();

        }

        private void FileHandler(object sender, ExampleCode.NewFileEventArgs e)
        {
            browserExample.DocumentText = ExampleToHtml();
        }



    }
}
