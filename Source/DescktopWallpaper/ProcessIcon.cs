////////////////////////////////////////////////////////////////////////////
//	Copyright 2014 : Vladimir Novick    https://www.linkedin.com/in/vladimirnovick/  
//
//    NO WARRANTIES ARE EXTENDED. USE AT YOUR OWN RISK. 
//
//      Available under the BSD and MIT licenses
//
// To contact the author with suggestions or comments, use  :vlad.novick@gmail.com
//
////////////////////////////////////////////////////////////////////////////
using System;
using System.Diagnostics;
using System.Windows.Forms;
using SGCombo.Wallpaper.Changer.Properties;

namespace SGCombo.Wallpaper.Changer
{

	class ProcessIcon : IDisposable
	{

		NotifyIcon ni;


		public ProcessIcon()
		{

			ni = new NotifyIcon();
		}


		public void Display()
		{
		
			ni.MouseClick += new MouseEventHandler(ni_MouseClick);
			ni.Icon = Resource.mainicon;
			ni.Text = "Wallpaper changer";
			ni.Visible = true;


			ni.ContextMenuStrip = new ContextMenus().Create();
		}


		public void Dispose()
		{

			ni.Dispose();
		}


		void ni_MouseClick(object sender, MouseEventArgs e)
		{
			// Handle mouse button clicks.
			if (e.Button == MouseButtons.Left)
			{
                if (!ContextMenus.isAboutLoaded)
                {
                    ContextMenus.isAboutLoaded = true;
                    new AboutBox().ShowDialog();
                    ContextMenus.isAboutLoaded = false;
                }
            }
		}
	}
}