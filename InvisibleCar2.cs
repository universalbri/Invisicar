using GTA;
using GTA.Native;
using GTA.Math;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Threading;

namespace GTA
{
    public class InvisibleCar2 : Script
    {
        private TheBeginning m_parentScript;
        private IniFile m_AppSettings = null;

        // DEBUG WINDOW DRAWING CODE
        private const float DEBUGTEXTSCALE = 0.33f;
        private UIText m_UIText = new UIText("", new Point(5, 5), DEBUGTEXTSCALE);
        private UIRectangle m_UIRectangle = new UIRectangle(new Point(5, 5), new Size(425, 450), Color.Black);

        private bool m_bIsInvisible = false;
        private int m_nCurrentTransparency;

        public InvisibleCar2(Script mainScript, IniFile appSettings)
        {
            this.Tick += OnTick;
            this.KeyUp += OnKeyUp;
            this.Interval = 50;

            m_parentScript = (TheBeginning)mainScript;
            m_AppSettings = appSettings;

            m_UIText.Enabled = true;
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (m_AppSettings.m_bShowDebugPanel && m_parentScript.m_bDebugToggled)
            {
                m_parentScript.DEBUG.OUT("InvisibleCar", Color.LightBlue);
            }

            if (false)
            {
                if (m_bIsInvisible &&
                    m_AppSettings.m_bChameleonLikeTransparency &&
                    m_nCurrentTransparency != (int)((float)Game.Player.Character.CurrentVehicle.Alpha / 255.0f) &&
                    m_AppSettings.m_nPercentageOfInvisibility > m_nCurrentTransparency)
                {
                    m_nCurrentTransparency = ((int)((float)Game.Player.Character.CurrentVehicle.Alpha / 255.0f)) - 1;
                    Game.Player.Character.CurrentVehicle.Alpha = (int)(((float)m_nCurrentTransparency / 100.0f) * 255.0f);
                }
                else if (!m_bIsInvisible &&
                            !m_AppSettings.m_bChameleonLikeTransparency &&
                            m_nCurrentTransparency != 100) ;
                {
                    m_nCurrentTransparency = ((int)((float)Game.Player.Character.CurrentVehicle.Alpha / 255.0f)) + 1;
                    Game.Player.Character.CurrentVehicle.Alpha = (int)(((float)m_nCurrentTransparency / 100.0f) * 255.0f);
                }
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            /*
             if (m_parentScript.m_bIsModEnabled == true)
            {
                if (e.KeyCode == m_AppSettings.m_keyToggleInvisibiliy )
                {
                    m_bIsInvisible = !m_bIsInvisible;
                    UI.Notify(String.Format("Invisibility {0}", m_bIsInvisible ? "Activated" : "Deactivated"));
                    if (m_AppSettings.m_bChameleonLikeTransparency)
                    {
                        float visGoal = (((float)m_AppSettings.m_nPercentageOfInvisibility) / 100.0f) * 255.0f;
                        Game.Player.Character.CurrentVehicle.Alpha = (m_bIsInvisible) ? (int) visGoal: 255;
                    }
                }
            }
            */
        }
    }
}
