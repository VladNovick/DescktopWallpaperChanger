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
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Windows.Forms;

namespace SGCombo.Wallpaper.Changer
{
	partial class AboutBox : Form
	{

        public const string ParameterNameStyle = "Style";

        public const string ParameterNameImageDirectory = "ImageDirectory";


        public const string ParameterNameTimeOut = "TimeOut";

        public AboutBox()
		{


            InitializeComponent();
			this.Text = String.Format("About {0}", AssemblyTitle);
			this.labelProductName.Text = AssemblyProduct;
			this.labelVersion.Text = String.Format("Version {0}", AssemblyVersion);
			this.labelCopyright.Text = AssemblyCopyright;
			this.labelCompanyName.Text = AssemblyCompany;
			this.textBoxDescription.Text = AssemblyDescription;

            this.textBoxImagesFolder.Text = ConfigurationManager.AppSettings[ParameterNameImageDirectory];
            this.textBoxImagesFolder.Select(0,0);

            this.comboBoxScaleType.Text = ConfigurationManager.AppSettings[ParameterNameStyle];
            this.comboBoxScaleType.Select(0, 0);
            currentStyle = this.comboBoxScaleType.Text;

            this.numericUpDownTimeOut.Text = ConfigurationManager.AppSettings[ParameterNameTimeOut];
            this.numericUpDownTimeOut.Select(0, 0);

        }

        private string currentStyle;

        #region Assembly Attribute Accessors

        public string AssemblyTitle
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
				if (attributes.Length > 0)
				{
					AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
					if (titleAttribute.Title != "")
					{
						return titleAttribute.Title;
					}
				}
				return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
			}
		}

		public string AssemblyVersion
		{
			get
			{
				return Assembly.GetExecutingAssembly().GetName().Version.ToString();
			}
		}

		public string AssemblyDescription
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyDescriptionAttribute)attributes[0]).Description;
			}
		}

		public string AssemblyProduct
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyProductAttribute)attributes[0]).Product;
			}
		}

		public string AssemblyCopyright
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
			}
		}

		public string AssemblyCompany
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyCompanyAttribute)attributes[0]).Company;
			}
		}

        #endregion

        private void buttonGetFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                if (!CheckImages()) return;

                this.textBoxImagesFolder.Text = folderBrowserDialog1.SelectedPath;
                this.textBoxImagesFolder.Select(0, 0);

                SaveParameter(ParameterNameImageDirectory, this.textBoxImagesFolder.Text);

            }
        }

        private void SaveParameter(string paramName,String value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings[paramName].Value = value;

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private Boolean CheckImages()
        {
            List<String> images = new List<string>();
            JobsWorker.getFileList(images, folderBrowserDialog1.SelectedPath);
            if (images.Count == 0)
            {
                MessageBox.Show("Please select dirctory with background pictuares","Background not exists");
                return false;
            }
            return true;
        }


        private void AboutBox_Load(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(buttonRefresh, "Set selected parameters ");
            toolTip1.SetToolTip(buttonGetFolder, "Choose folder");
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            WallpaperWindowsChanger.OnStop();
            WallpaperWindowsChanger.OnStart();
        }

        private void comboBoxScaleType_TextChanged_1(object sender, EventArgs e)
        {
            String selectedType = comboBoxScaleType.Text;
            SaveParameter(ParameterNameStyle, selectedType);

        }

        private void numericUpDownTimeOut_ValueChanged(object sender, EventArgs e)
        {
            String value = numericUpDownTimeOut.Value.ToString();
            SaveParameter(ParameterNameTimeOut, value);
        }
    }
}