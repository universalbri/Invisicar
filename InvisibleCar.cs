using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Threading;

namespace GTA
{
    public class InvisibleCar : Script
    {
        // Initializer Information
        private TheBeginning m_parentScript;
        private IniFile m_AppSettings = null;

        // DEBUG WINDOW DRAWING CODE
        private const float DEBUGTEXTSCALE = 0.33f;
        private UIText m_UIText = new UIText("", new Point(5, 5), DEBUGTEXTSCALE);
        private UIRectangle m_UIRectangle = new UIRectangle(new Point(5, 5), new Size(425, 450), Color.Black);

        private bool m_bIsInvisible = false;
        private float m_fCurrentTransparency;

        private string m_sCurDebugState = "";
        public InvisibleCar(Script mainScript, IniFile appSettings)
        {
            mainScript.Tick += OnTick;
            mainScript.KeyUp += OnKeyUp;
            this.Interval = 100;

            m_parentScript = (TheBeginning)mainScript;
            m_AppSettings = appSettings;

            m_UIText.Enabled = true;
        }

        private void OnTick(object sender, EventArgs e)
        {
            string sCur; int curA;
            const float incDec = 1.0f;

            if (m_parentScript.m_bIsModEnabled == true)
            {
                if (!Game.Player.Character.IsInVehicle())
                {
                    m_bIsInvisible = false; // Turn off invisibility whenever the player is NOT in a vehicle
                    Game.Player.IgnoredByPolice = m_bIsInvisible;
                    m_fCurrentTransparency = 100.0f; 
                    setTransparency(255); // Just turn off transparency for everything if the player aint in a car.
                }
                else
                {
                    Game.Player.IgnoredByPolice = m_bIsInvisible;
                    if (m_AppSettings.m_bChameleonLikeTransparency && Game.Player.Character.CurrentVehicle != null)
                    {
                        if (!m_bIsInvisible && m_fCurrentTransparency < 100.0f)
                        {
                            m_fCurrentTransparency += incDec;
                            curA = (int)((m_fCurrentTransparency / 100.0f) * 255.0f);
                            setTransparency(curA);
                        }
                        else if (m_bIsInvisible &&
                                 ((int)m_fCurrentTransparency) > m_AppSettings.m_nPercentageOfInvisibility)
                        {
                            m_fCurrentTransparency -= incDec;
                            curA = (int)((m_fCurrentTransparency / 100.0f) * 255.0f);
                            setTransparency(curA);
                        }
                        else if (m_bIsInvisible)
                        {
                            if (Game.Player.Character.CurrentVehicle != null)
                            {
                                setTransparency((int)((((float)m_AppSettings.m_nPercentageOfInvisibility) / 100.0f) * 255.0f));
                            }
                            else
                                m_bIsInvisible = false; // Turn off invisibility when there's no current car. 
                        }
                        else if ( Game.Player.Character.CurrentVehicle != null && 
                                    Game.Player.Character.CurrentVehicle.Alpha < 100  )
                        {
                            m_bIsInvisible = true;
                            m_fCurrentTransparency = (((float)Game.Player.Character.CurrentVehicle.Alpha) / 255.0f) * 100.0f;
                            if (m_fCurrentTransparency < m_AppSettings.m_nPercentageOfInvisibility)
                                m_fCurrentTransparency = m_AppSettings.m_nPercentageOfInvisibility;
                            curA = (int)((m_fCurrentTransparency / 100.0f) * 255.0f);
                            setTransparency(curA);
                        }

                    }
                }
            }

            if (m_AppSettings.m_bShowDebugPanel && m_parentScript.m_bDebugToggled)
            {
                sCur = string.Format("CLT={0}", m_AppSettings.m_bChameleonLikeTransparency);
                m_parentScript.DEBUG.OUT("NEW INVISIBLE CAR", Color.LightBlue);
                m_parentScript.DEBUG.OUT(sCur, Color.Yellow);
                m_parentScript.DEBUG.OUT(m_sCurDebugState, Color.Yellow);
                sCur = string.Format("CurTran={0}", m_fCurrentTransparency);
                m_parentScript.DEBUG.OUT(sCur, Color.Yellow);
                sCur = string.Format("POI={0}", m_AppSettings.m_nPercentageOfInvisibility);
                m_parentScript.DEBUG.OUT(sCur, Color.Yellow);

            }
        }

        private void setTransparency(int curA)
        {
            // seems redundant, but there's time the alpha function's gonna get called
            // when the player's not in a car. I only toggle his state when required
            if (m_AppSettings.m_bIncludePlayerAndPassengers)
                Game.Player.Character.Alpha = curA;

            if (Game.Player.Character.CurrentVehicle != null)
            {
                Game.Player.Character.CurrentVehicle.Alpha = curA;
                if (m_AppSettings.m_bIncludePlayerAndPassengers)
                {
                    foreach ( Ped p in Game.Player.Character.CurrentVehicle.Passengers )
                        p.Alpha = curA;
                }
            }

        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (m_parentScript.m_bIsModEnabled == true)
            {
                if (e.KeyCode == m_AppSettings.m_keyToggleInvisibiliy)
                {
                    m_bIsInvisible = !m_bIsInvisible;
                    if (m_bIsInvisible && Game.Player.Character.CurrentVehicle == null)
                    {
                        m_bIsInvisible = false;
                        UI.Notify(String.Format("Must be in a vehicle to toggle invisibility"));
                        goto NOGO;
                    }

                    
                    UI.Notify(String.Format("Invisibility {0}", m_bIsInvisible ? "Activated" : "Deactivated"));

                    if (!m_AppSettings.m_bChameleonLikeTransparency)
                    {
                        float visGoal = (((float)m_AppSettings.m_nPercentageOfInvisibility) / 100.0f) * 255.0f;
                        if (Game.Player.Character.CurrentVehicle != null)
                            setTransparency((int)((m_bIsInvisible) ? (int)visGoal : 255));
                    }
                }
            }
        NOGO:
            return;
        }

    }
}
