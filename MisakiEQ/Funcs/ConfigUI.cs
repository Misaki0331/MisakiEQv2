using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisakiEQ.Funcs
{
    internal class ConfigUI
    {
        public static void OpenLink(string url)
        {
            ProcessStartInfo pi = new()
            {
                FileName = url,
                UseShellExecute = true,
            };
            Process.Start(pi);
        }
        public class LinkButton
        {
            public string Name { get => button.Text; set => button.Text = value; }
            public Point Point { get => button.Location; set => button.Location = value; }
            public bool Enabled {  get=> button.Enabled; set => button.Enabled = value; }
            Action act;
            string url;
            readonly TabPage Par;
            readonly Button button;
            public LinkButton(TabPage par,Point point, string name, Action act)
            {
                this.act = act;
                Par = par;
                button = new()
                {
                    Size = new Size(88, 23),
                };
                Point = point;
                Name = name;
                par.Controls.Add(button);
                button.Click += Click;
                url = "";
            }
            public LinkButton(TabPage par, Point point, string name, string url)
            {
                act = new Action(() => { });
                Par = par;
                button = new()
                {
                    Size = new Size(88, 23),
                };
                Point = point;
                Name = name;
                par.Controls.Add(button);
                button.Click += Click;
                this.url=url;
            }
            public void ChangeFunction(Action action)
            {
                act = action;
            }
            public void Dispose()
            {
                Par.Controls.Remove(button);
                button.Dispose();
            }
            void Click(object? e,EventArgs args)
            {
                act();
                if (!string.IsNullOrEmpty(this.url))
                {
                    OpenLink(url);
                }
            }

        }
    }
}
