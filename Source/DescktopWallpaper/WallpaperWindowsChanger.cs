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
using System.ServiceProcess;
using System.Diagnostics;
using System.Windows.Forms;

namespace SGCombo.Wallpaper.Changer
{

    public class WallpaperWindowsChanger 
    {


        private System.ComponentModel.Container components = null;

        public WallpaperWindowsChanger()
        {

            InitializeComponent();

        }

        [STAThread]
        static void Main(string[] args)
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Show the system tray icon.					
            using (ProcessIcon pi = new ProcessIcon())
            {
                pi.Display();

                 OnStart();

                // Make sure the application runs!
                Application.Run();
                OnStop();
            }

        }

        private void InitializeComponent()
        {

            String s = System.AppDomain.CurrentDomain.BaseDirectory;
            System.IO.Directory.SetCurrentDirectory(s);

        }

        private static Object wallpaperServiceStart = null;

        public  static void OnStart()
        {

            wallpaperServiceStart = new WallpaperChangerStart();
            ((WallpaperChangerStart)wallpaperServiceStart).OnStart();
        }

        public static void OnStop()
        {

            ((WallpaperChangerStart)wallpaperServiceStart).OnStop();

        }
    }
}
