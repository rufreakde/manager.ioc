using UnityEngine;
using System.Collections;

namespace manager.ioc
{
    public interface IamSingleton
    {

        /// <summary>
        /// Use this function to init the Singletons because they are not Monobehaviours! So no Awake no Start no Update and so on!
        /// </summary>
        void iInitialize();

    }
}