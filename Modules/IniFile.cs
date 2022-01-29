using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

// Change this to match your program's normal namespace
namespace GTA
{
    public class IniFile   // revision 11
    {
        // Enabling/Disabling Keys
        public bool m_bShowDebugPanel = false;
        public Keys m_keyToggleDebug = Keys.NumPad9;

        // Enabling/Disabling Keys
        public Keys m_keyToggleMod = Keys.NumPad0;
        public Keys m_keyToggleInvisibiliy = Keys.Decimal; // Make Invisible Key

        // Invisibility Settings
        public int m_nPercentageOfInvisibility = 20;
        public bool m_bIncludePlayerAndPassengers = false;
        public bool m_bChameleonLikeTransparency = true;

        public string m_sLastError = "";

        private string EXE = Assembly.GetCallingAssembly().GetName().Name;
        private string Path;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        public IniFile()
        {
            Path = String.Format("{0}\\{1}.INI", AppDomain.CurrentDomain.BaseDirectory, EXE);
            // m_sLastError += Path;

            TypeConverter converter = TypeDescriptor.GetConverter(typeof(Keys));
            try
            {
                if (!KeyExists("keyToggleModOnOff", "App Settings"))
                    Write("keyToggleModOnOff", m_keyToggleMod.ToString(), "Aoo Settings");
                else
                    m_keyToggleMod = (Keys)converter.ConvertFromString(Read("keyToggleModOnOff", "App Settings"));
            }
            catch (Exception eINIErr)
            {
                m_sLastError += String.Format("keyToggleModOnOff Error='{0}'\n", eINIErr.Message);
            }

            try
            {
                if (!KeyExists("keyToggleInvisibility", "App Settings"))
                    Write("keyToggleInvisibility", m_keyToggleMod.ToString(), "Aoo Settings");
                else
                    m_keyToggleInvisibiliy = (Keys)converter.ConvertFromString(Read("keyToggleInvisibility", "App Settings"));
            }
            catch (Exception eINIErr)
            {
                m_sLastError += String.Format("keyToggleInvisibility Error='{0}'\n", eINIErr.Message);
            }

            try
            {
                if (!KeyExists("bIncludePlayerAndPassengers", "App Settings"))
                    Write("bIncludePlayerAndPassengers", m_bIncludePlayerAndPassengers.ToString(), "App Settings");
                else
                    Boolean.TryParse(Read("bIncludePlayerAndPassengers", "App Settings"), out m_bIncludePlayerAndPassengers);
            }
            catch (Exception eINIErr)
            {
                m_sLastError += String.Format("bIncludePlayerAndPassengers Error='{0}'\n", eINIErr.Message);
            }

            try
            {
                if (!KeyExists("bChameleonLikeTransparency", "App Settings"))
                    Write("bChameleonLikeTransparency", m_bChameleonLikeTransparency.ToString(), "App Settings");
                else
                    Boolean.TryParse(Read("bChameleonLikeTransparency", "App Settings"), out m_bChameleonLikeTransparency);
            }
            catch (Exception eINIErr)
            {
                m_sLastError += String.Format("bChameleonLikeTransparencyError='{0}'\n", eINIErr.Message);
            }

            try
            {
                if (!KeyExists("nPercentageOfInvisibility", "App Settings"))
                    Write("nPercentageOfInvisibility", m_nPercentageOfInvisibility.ToString(), "App Settings");
                else
                    int.TryParse(Read("nPercentageOfInvisibility", "App Settings"), out m_nPercentageOfInvisibility);
            }
            catch (Exception eINIErr)
            {
                m_sLastError += String.Format("StartAbductionImmediatelyKey Error='{0}'\n", eINIErr.Message);
            }

            try
            {
                if (!KeyExists("ShowDebugPanel", "Debug Settings"))
                    Write("ShowDebugPanel", m_bShowDebugPanel.ToString(), "Debug Settings");
                else
                    m_bShowDebugPanel = Convert.ToBoolean(Read("ShowDebugPanel", "Debug Settings"));
            }
            catch (Exception eINIErr)
            {
                m_sLastError += String.Format("ShowDebugPanel Error='{0}'\n", eINIErr.Message);
            }

            try
            {
                if (!KeyExists("ToggleDebugKey", "Debug Settings"))
                    Write("ToggleDebugKey", m_keyToggleDebug.ToString(), "Debug Settings");
                else
                    m_keyToggleDebug = (Keys)converter.ConvertFromString(Read("ToggleDebugKey", "Debug Settings"));
            }
            catch (Exception eINIErr)
            {
                m_sLastError += String.Format("ToggleDebugKey Error='{0}'\n", eINIErr.Message);
            }
        }

        private string Read(string Key, string Section = null)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 255, Path);
            return RetVal.ToString();
        }

        private void Write(string Key, string Value, string Section = null)
        {
            WritePrivateProfileString(Section ?? EXE, Key, Value, Path);
        }

        private void DeleteKey(string Key, string Section = null)
        {
            Write(Key, null, Section ?? EXE);
        }

        private void DeleteSection(string Section = null)
        {
            Write(null, null, Section ?? EXE);
        }

        private bool KeyExists(string Key, string Section = null)
        {
            return Read(Key, Section).Length > 0;
        }
    }
}