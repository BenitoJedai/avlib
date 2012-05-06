using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VALib.Draw.Controls.Core
{
    public class BaseBehaviour
    {
        protected BaseControl CurrentControl;
        public string Name { get; set; }

        public virtual void Apply(BaseControl control)
        {
            CurrentControl = control;
        }

        public virtual void UnApply()
        {
            CurrentControl = null;
        }
    }

    public class BehaviourList
    {
        private BaseControl m_owner;

        public BehaviourList(BaseControl owner)
        {
            m_owner = owner;
        }

        private List<BaseBehaviour> list = null;
        private void CheckList()
        {
            if (list == null) list = new List<BaseBehaviour>();
        }
        public bool Exist(string name)
        {
            if (list == null) return false;
            foreach (var b in list)
                if (b.Name == name) return true;
            return false;
        }
        public void Remove(string name)
        {
            if (list == null) return;
            for (int i = list.Count - 1; i >= 0; i--)
                if (list[i].Name == name)
                {
                    list[i].UnApply();
                    list.RemoveAt(i);
                }
        }
        public void Add(BaseBehaviour behaviour, string name)
        {
            if (Exist(name)) Remove(name);
            CheckList();
            behaviour.Name = name;
            list.Add(behaviour);
            behaviour.Apply(m_owner);
        }
    }
}
