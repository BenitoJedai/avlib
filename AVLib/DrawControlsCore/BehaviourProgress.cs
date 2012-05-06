using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AVLib.Utils;

namespace VALib.Draw.Controls.Core
{
    

    public class BehaviourProgress : BaseBehaviour
    {
        public override void Apply(BaseControl control)
        {
            base.Apply(control);
            m_maxPos = Math.Max(0, CurrentControl.Value["MaxPosition"].AsInteger());
            m_translatedMaxPos = Math.Max(0, CurrentControl.Value["TranslatedMaxPosition"].AsInteger());
            Pos = CurrentControl.Value["Position"].AsInteger();

            CurrentControl.Property["Position"].AnyChanged += Pos_Changed;
            CurrentControl.Property["MaxPosition"].AnyChanged += MaxPos_Changed;
            CurrentControl.Property["TranslatedPosition"].AnyChanged += TranslatedPos_Changed;
            CurrentControl.Property["TranslatedMaxPosition"].AnyChanged += TranslatedMaxPos_Changed;
        }

        public override void UnApply()
        {
            CurrentControl.Property["Position"].AnyChanged -= Pos_Changed;
            CurrentControl.Property["MaxPosition"].AnyChanged -= MaxPos_Changed;
            CurrentControl.Property["TranslatedPosition"].AnyChanged -= TranslatedPos_Changed;
            CurrentControl.Property["TranslatedMaxPosition"].AnyChanged -= TranslatedMaxPos_Changed;

            base.UnApply();
        }

        private void Pos_Changed()
        {
            Pos = CurrentControl.Value["Position"].AsInteger();
        }

        private void MaxPos_Changed()
        {
            MaxPos = CurrentControl.Value["MaxPosition"].AsInteger();
        }

        private void TranslatedPos_Changed()
        {
            TranslatedPos = CurrentControl.Value["TranslatedPosition"].AsInteger();
        }

        private void TranslatedMaxPos_Changed()
        {
            TranslatedMaxPos = CurrentControl.Value["TranslatedMaxPosition"].AsInteger();
        }

        private int m_pos;
        private int m_maxPos;
        private int m_translatedPos;
        private int m_translatedMaxPos;

        public int PosToTranslated(int pos)
        {
            if (m_translatedMaxPos != 0 && m_maxPos != 0)
            {
                return (int)(float)(pos / (m_maxPos / (float)m_translatedMaxPos));
            }
            return 0;
        }

        private int Pos
        {
            get { return m_pos; }
            set
            {
                if (m_pos != value)
                {
                    m_pos = Math.Min(m_maxPos, Math.Max(0, value));
                    if (m_translatedMaxPos != 0 && m_maxPos != 0)
                    {
                        m_translatedPos = (int)(float)(m_pos / (m_maxPos / (float)m_translatedMaxPos));
                        CurrentControl.Value["TranslatedPosition"] = m_translatedPos;
                    }
                }
                if (m_pos != value) CurrentControl.Value["Position"] = m_pos;
            }
        }

        private int MaxPos
        {
            get { return m_maxPos; }
            set
            {
                if (m_maxPos != value)
                {
                    m_maxPos = Math.Max(0, value);
                    if (m_pos > m_maxPos)
                        Pos = m_maxPos;
                    else
                        if (m_translatedMaxPos != 0 && m_maxPos != 0)
                        {
                            m_translatedPos = (int)(float)(m_pos / (m_maxPos / (float)m_translatedMaxPos));
                            CurrentControl.Value["TranslatedPosition"] = m_translatedPos;
                        }
                }
                if (m_maxPos != value) CurrentControl.Value["MaxPosition"] = m_maxPos;
            }
        }

        public int TranslatedToPos(int translated)
        {
            if (m_translatedMaxPos != 0 && m_maxPos != 0)
            {
                return (int)(float)(translated * (m_maxPos / (float)m_translatedMaxPos));
            }
            return 0;
        }

        private int TranslatedPos
        {
            get { return m_translatedPos; }
            set
            {
                if (m_translatedPos != value)
                {
                    m_translatedPos = Math.Min(m_translatedMaxPos, Math.Max(0, value));
                    if (m_translatedMaxPos != 0 && m_maxPos != 0)
                    {
                        m_pos = (int)(float)(m_translatedPos * (m_maxPos / (float)m_translatedMaxPos));
                        CurrentControl.Value["Position"] = m_pos;
                    }
                }
                if (m_translatedPos != value) CurrentControl.Value["TranslatedPosition"] = m_translatedPos;
            }
        }

        private int TranslatedMaxPos
        {
            get { return m_translatedMaxPos; }
            set
            {
                if (m_translatedMaxPos != value)
                {
                    m_translatedMaxPos = Math.Max(0, value);
                    if (m_translatedPos > m_translatedMaxPos)
                        TranslatedPos = m_translatedMaxPos;
                    else
                        if (m_translatedMaxPos != 0 && m_maxPos != 0)
                        {
                            m_translatedPos = (int)(float)(m_pos / (m_maxPos / (float)m_translatedMaxPos));
                            CurrentControl.Value["TranslatedPosition"] = m_translatedPos;
                        }
                }
                if (m_translatedMaxPos != value) CurrentControl.Value["TranslatedMaxPosition"] = m_translatedMaxPos;
            }
        }
    }
}
