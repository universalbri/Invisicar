using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Threading;

namespace GTA
{
    public class DEBUGLINE
    {
        public string line;
        public Color color;

        public DEBUGLINE(string sLine, Color cColor)
        {
            line = sLine;
            color = cColor;
        }
    }

    public class DebugLogger : Script
    {
        // Initializer Information
        private TheBeginning m_parentScript;
        private IniFile m_AppSettings = null;

        private const float PPL = 17.0f;

        // Basic shit.
        private readonly Random _random = new Random();

        // DEBUG WINDOW DRAWING CODE
        private const float DEBUGTEXTSCALE = 0.33f;
        private UIText m_UIText = new UIText("", new Point(5, 5), DEBUGTEXTSCALE);
        private UIRectangle m_UIRectangle = new UIRectangle(new Point(5, 5), new Size(425, 450), Color.Black);
        private List<DEBUGLINE> m_lstLogLines = new List<DEBUGLINE>();

        public DebugLogger(Script mainScript, IniFile appSettings)
        {
            mainScript.Tick += OnTick;
            mainScript.KeyUp += OnKeyUp;

            m_parentScript = (TheBeginning)mainScript;
            m_AppSettings = appSettings;

            m_UIText.Enabled = true;
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (m_AppSettings.m_bShowDebugPanel && m_parentScript.m_bDebugToggled)
            {
                OUT("DebugLogger", Color.LightBlue);
            }

            if (m_AppSettings == null)
            {
                m_AppSettings = new IniFile();
            }

            if (m_parentScript.m_bIsModEnabled == true)
            {

                if (m_AppSettings.m_bShowDebugPanel && m_parentScript.m_bDebugToggled)
                {
                    int y = -1;
                    UIText txt;

                    m_UIRectangle.Draw();
                    m_UIRectangle.Size = new Size(425, (int)((float)m_lstLogLines.Count * PPL));

                    foreach (DEBUGLINE dbg in m_lstLogLines)
                    {
                        txt = new UIText(dbg.line, new Point(10, 5 + ((y += 1) * 15)), DEBUGTEXTSCALE, dbg.color);
                        txt.Draw();
                    }

                    m_lstLogLines.Clear();
                }
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
            }
        }

        public void OUT(string sOUTTEXT, Color color)
        {
            m_lstLogLines.Add(new DEBUGLINE(sOUTTEXT, color));
        }
    }
}
