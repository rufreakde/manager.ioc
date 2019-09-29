using UnityEngine;
using System;


namespace manager.ioc
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class ReadOnlyAttribute : PropertyAttribute
    {
        private bool readOnly = true;

        public bool ReadOnly
        {
            get { return readOnly; }
            set { readOnly = value; }
        }

        public ReadOnlyAttribute()
        {

        }

        public ReadOnlyAttribute(bool _IsReadOnly)
        {
            ReadOnly = _IsReadOnly;
        }
    }
}