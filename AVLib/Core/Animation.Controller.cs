﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace AVLib.Animations
{
    public interface IInvokeCompatible
    {
        bool InvokeRequired { get; }
        object Invoke(Delegate method);
        object Invoke(Delegate method, params Object[] args);
    }

    public static class AnimationControler
    {
        #region Internal and Private

        public class BaseThreadParam : ICloneable
        {
            public object control;
            internal AnimatorState animatorState;
            internal ControlState controlState;
            public int time;
            public string PropertyName;
            public int QueueLevel = 0;
            public bool CompleteIfCancel;
            public string queueName = "";
            internal FinalCallback finalCallback;

            public object Clone()
            {
                var clon = CreateClon();
                CloneTo(clon);
                return clon;
            }

            protected virtual BaseThreadParam CreateClon()
            {
                return new BaseThreadParam();
            }

            protected virtual void CloneTo(BaseThreadParam clon)
            {
                clon.time = time;
                clon.QueueLevel = QueueLevel;
                clon.finalCallback = finalCallback;
                clon.PropertyName = PropertyName;
            }
        }

        public class AnimatorState
        {
            internal BaseThreadParam Animator;
            public bool Canceled;
            public bool InQueue;
            internal WaitCallback QueueMethod;
            public BaseThreadParam QueueMethodParam;
            internal object QueueOwner;
        }

        internal class ControlState
        {
            private object control;
            private List<AnimatorState> Animators = new List<AnimatorState>();
            private bool isCancel = false;
            private bool isForce = false;

            public int AnimatorsCount
            {
                get
                {
                    lock (this)
                    {
                        return Animators.Count;
                    }
                }
            }

            public ControlState(object control)
            {
                this.control = control;
            }

            private void Add(AnimatorState animatorState)
            {
                Animators.Add(animatorState);
            }
            public void Cancel(bool forceComplete)
            {
                lock (this)
                {
                    isCancel = Animators.Count > 0;
                    isForce = isCancel && forceComplete;
                    //Trace.Write("Cancel: " + isCancel);
                    int removed = 0;
                    int canceled = 0;
                    for (int i = Animators.Count - 1; i >= 0; i--)
                    {
                        if (Animators[i].Animator == null)
                        {
                            if (isForce)
                            {
                                Animators[i].QueueMethodParam.CompleteIfCancel = true;
                                Animators[i].Canceled = true;
                            }
                            else
                            {
                                Animators.RemoveAt(i);
                                removed++;
                            }
                        }
                        else
                        {
                            if (forceComplete) Animators[i].QueueMethodParam.CompleteIfCancel = true;
                            Animators[i].Canceled = true;
                            canceled++;
                        }
                    }
                    //Trace.WriteLine(string.Format(" removed:{0} canceled:{1}", removed, canceled));
                }
            }
            private void Run(AnimatorState animatorState)
            {
                //Trace.WriteLine("Run.");
                if (Animators.IndexOf(animatorState) >= 0)
                {
                    if (animatorState.QueueMethodParam == null) throw new ApplicationException("Queue method param required and cannot be null");

                    animatorState.QueueMethodParam.animatorState = animatorState;
                    animatorState.QueueMethodParam.controlState = this;
                    animatorState.QueueMethodParam.control = control;


                    animatorState.Animator = animatorState.QueueMethodParam;
                    animatorState.InQueue = false;

                    ThreadPool.QueueUserWorkItem(animatorState.QueueMethod, animatorState.QueueMethodParam);
                }
            }
            private void CheckForRun(object owner)
            {
                //Trace.Write("CheckForRun:");
                bool allInQueue = true;
                bool allStoped = true;
                int minQueueLevel = int.MaxValue;
                for (int i = 0; i < Animators.Count; i++)
                {
                    if (!Animators[i].InQueue)
                        allInQueue = false;
                    else
                    {
                        if (Animators[i].QueueMethodParam.QueueLevel < minQueueLevel)
                            minQueueLevel = Animators[i].QueueMethodParam.QueueLevel;
                    }
                    if (Animators[i].Animator != null) allStoped = false;
                    if (Animators[i].Animator == null && (!isCancel || isForce))
                    {
                        if (!Animators[i].InQueue || (owner != null && Animators[i].QueueOwner == owner))
                        {
                            Run(Animators[i]);
                            allStoped = false;
                            //Trace.Write(" +1");
                        }
                    }
                }
                if (allInQueue && (!isCancel || isForce) && Animators.Count > 0)
                {
                    for (int i = 0; i < Animators.Count; i++)
                    {
                        if (Animators[i].Animator == null && Animators[i].QueueMethodParam.QueueLevel == minQueueLevel)
                        {
                            Run(Animators[i]);
                            //Trace.Write(" queue+1");
                        }
                    }
                }
                if (allStoped && isCancel)
                {
                    isCancel = isForce = false;
                    //Trace.Write(" isCancel=false");
                    if (Animators.Count > 0) CheckForRun(null);
                }
                //Trace.WriteLine("");
            }

            public AnimatorState Execute(WaitCallback queueMethod, BaseThreadParam queueMethodParam)
            {
                lock (this)
                {
                    var state = new AnimatorState() { QueueMethod = queueMethod, QueueMethodParam = queueMethodParam };
                    Add(state);
                    if (!isCancel)
                    {
                        //Trace.WriteLine("Execute: run");
                        Run(state);
                    }
                    //else
                    //Trace.WriteLine("Execute: wait for cancel");
                    return state;
                }
            }
            public AnimatorState Queue(WaitCallback queueMethod, BaseThreadParam queueMethodParam, object queueOwner)
            {
                lock (this)
                {
                    bool inQueue = Animators.Count > 0;
                    var state = new AnimatorState() { InQueue = true, QueueMethod = queueMethod, QueueMethodParam = queueMethodParam, QueueOwner = queueOwner };
                    Add(state);
                    if (!inQueue && !isCancel)
                    {
                        //Trace.WriteLine("Queue: run");
                        Run(state);
                    }
                    //else
                    //Trace.WriteLine("Queue: wait");
                    return state;
                }
            }
            public void AnimatorEnd(AnimatorState animator)
            {
                try
                {
                    lock (this)
                    {
                        //Trace.Write("AnimatorEnd:");
                        if (Animators.IndexOf(animator) >= 0)
                        {
                            Animators.Remove(animator);
                            //Trace.Write(" removed");
                        }
                        //else
                        //Trace.Write(" not found");
                        //Trace.WriteLine("");
                        CheckForRun(animator);
                    }
                }
                finally
                {
                    if (animator.QueueMethodParam.finalCallback != null)
                    {
                        try
                        {
                            if (animator.QueueMethodParam.control is Control)
                                ((Control)animator.QueueMethodParam.control).Invoke(new FinalCallback(animator.QueueMethodParam.finalCallback), animator);
                            else if (animator.QueueMethodParam.control is IInvokeCompatible)
                                ((IInvokeCompatible) animator.QueueMethodParam.control).Invoke(
                                    new FinalCallback(animator.QueueMethodParam.finalCallback), animator);
                            else
                                animator.QueueMethodParam.finalCallback(animator);
                        }
                        catch (Exception) { }
                    }
                }
            }
        }

        private class NamedControlState
        {
            public string name;
            public ControlState controlState;
        }

        private class NamedControlStateList
        {
            public Dictionary<string, ControlState> list = new Dictionary<string, ControlState>();

            public bool RemoveEmpty()
            {
                List<string> listForRemove = new List<string>();
                foreach (var item in list)
                {
                    if (item.Value.AnimatorsCount == 0)
                        listForRemove.Add(item.Key);
                }
                foreach (var name in listForRemove)
                {
                    list.Remove(name);
                    //Trace.WriteLine("Empty control removed");
                }
                return list.Count == 0;
            }

            public ControlState this[object control, string name]
            {
                get
                {
                    if (list.ContainsKey(name))
                        return list[name];
                    var state = new ControlState(control);
                    list.Add(name, state);
                    return state;
                }
            }

            public ControlState FindControlState(string name)
            {
                if (list.ContainsKey(name))
                    return list[name];
                return null;
            }
        }

        private static Dictionary<object, NamedControlStateList> animeControls = new Dictionary<object, NamedControlStateList>();

        private static void RemoveEmpty()
        {
            lock (animeControls)
            {
                List<object> listForRemove = new List<object>();
                foreach (var item in animeControls)
                {
                    if (item.Value.RemoveEmpty())
                        listForRemove.Add(item.Key);
                }
                foreach (var control in listForRemove)
                {
                    animeControls.Remove(control);
                    //Trace.WriteLine("Empty control removed");
                }
            }
        }

        private static bool CleanComplete()
        {
            lock (animeControls)
            {
                if (animeControls.Count == 0)
                {
                    cleanThreadActive = false;
                    return true;
                }
                return false;
            }
        }

        private static void CleanThreadProc(object threadParam)
        {
            //Trace.WriteLine("CleanThread: begin");
            do
            {
                RemoveEmpty();
                Thread.Sleep(1000);
            } while (!CleanComplete());
            //Trace.WriteLine("CleanThread: end");
        }

        private static bool cleanThreadActive = false;
        private static void CheckActive()
        {
            if (!cleanThreadActive)
            {
                cleanThreadActive = true;
                Thread th = new Thread(CleanThreadProc);
                th.Name = "Clean queue thread";
                th.Start(null);
                //ThreadPool.QueueUserWorkItem(CleanThreadProc, null);
            }
        }

        private static ControlState GetControlState(object control, string name)
        {
            CheckActive();
            if (animeControls.ContainsKey(control))
                return animeControls[control][control, name];

            var controlState = new NamedControlStateList();
            animeControls.Add(control, controlState);
            return controlState[control, name];
        }

        private static ControlState FindControlState(object control, string name)
        {
            if (animeControls.ContainsKey(control))
                return animeControls[control].FindControlState(name);
            return null;
        }

        internal class AnimePacket : ICloneable
        {
            public WaitCallback method = null;
            public BaseThreadParam threadParam = null;
            public bool isQueue = false;
            public object queueOwner = null;
            public bool cancel = false;

            public object Clone()
            {
                var clon = new AnimePacket();
                clon.method = method;
                clon.threadParam = (BaseThreadParam)threadParam.Clone();
                clon.isQueue = isQueue;
                clon.queueOwner = null;
                clon.cancel = cancel;
                return clon;
            }
        }

        internal static AnimatorState ProcessPacket(object control, AnimePacket packet)
        {
            if (packet.cancel)
            {
                Cancel(control, packet.threadParam.queueName, false);
                return null;
            }
            if (packet.isQueue) return Queue(control, packet.method, packet.threadParam, packet.queueOwner);
            return Execute(control, packet.method, packet.threadParam);
        }

        internal static void Cancel(object control, string name, bool forceComplete)
        {
            if (control == null) return;
            lock (animeControls)
            {
                if (name == "all")
                {
                    if (animeControls.ContainsKey(control))
                    {
                        var list = animeControls[control].list;
                        foreach (var controlState in list)
                        {
                            controlState.Value.Cancel(forceComplete);
                        }
                    }
                }
                else
                {
                    ControlState controlState = FindControlState(control, name);
                    if (controlState != null) controlState.Cancel(forceComplete);
                }
            }
        }

        internal static AnimatorState Execute(object control, WaitCallback queueMethod, BaseThreadParam queueMethodParam)
        {
            if (control == null) return null;
            lock (animeControls)
            {
                ControlState controlState = GetControlState(control, queueMethodParam.queueName);
                return controlState.Execute(queueMethod, queueMethodParam);
            }
        }

        internal static AnimatorState Queue(object control, WaitCallback queueMethod, BaseThreadParam queueMethodParam, object queueOwner)
        {
            if (control == null) return null;
            lock (animeControls)
            {
                ControlState controlState = GetControlState(control, queueMethodParam.queueName);
                return controlState.Queue(queueMethod, queueMethodParam, queueOwner);
            }
        }

        #endregion

        #region Public Configurations

        public delegate void FinalCallback(AnimatorState animator);

        public static int SlowTime = 1000;
        public static int MediumTime = 500;
        public static int FastTime = 200;
        public static int IterationTime = 40;

        public static int GetSpeedTime(Speed speed)
        {
            switch (speed)
            {
                case Speed.Fast:
                    return FastTime;
                case Speed.Medium:
                    return MediumTime;
                case Speed.Slow:
                    return SlowTime;
            }
            return FastTime;
        }

        public static int GetIterations(int time)
        {
            return time / IterationTime;
        }

        #endregion
    }
}