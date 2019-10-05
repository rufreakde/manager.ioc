using UnityEngine;
using Type = System.Type;
using Object = UnityEngine.Object;
using minimap.rts.twod;
using players.rts;

/*
NOTE: 
    Benutz SetUp der rest braucht dich nicht zu interessieren^^!
    SetUp() ist auch die Reihenfolge in der die inits ausgeführt werden.
*/

namespace manager.ioc
{
    [AddComponentMenu("manager")]
    public class MANAGER : MonoBehaviour
    {
        [Split("SingletonContainer")]
        [SimpleButton("SetUp", typeof(MANAGER))]
        //[SimpleButton("ClearDictionary", typeof(CSingletonContainer))]
        public bool AWAKE_INIT_DONE = false;
        public bool DEBUG = false;


        public static MANAGER GET = null;
        [SerializeField]
        public CustomDict<Type, Object> Singletons = new CustomDict<Type, Object>();

        #region Behaviour and init
        /// <summary>
        /// Gets called at the Beginning of the Game. It adds all the managers into the singleton managers dict.
        /// </summary>
        public virtual void SetUp()
        {
            if (DEBUG)
                Debug.Log("Init all Managers...");

            AddS(typeof(PlayerManager));
            AddS(typeof(MinimapManager));

            if (DEBUG)
                Debug.Log("... finished all Managers");
        }
        #endregion

        #region logic
        public void InitSinglitons()
        {

            for (int i = 0; i < Singletons.Count; i++)
            {
                IamSingleton tSingletonInterfaceHandle = (IamSingleton)Singletons.Values[i];

                if (tSingletonInterfaceHandle != null)
                    tSingletonInterfaceHandle.iInitialize();
                else
                {
                    Debug.LogError("Error: " + Singletons.Values[i].ToString() + " does not include the " + typeof(IamSingleton) + " interface! ");
                }
            }
        }

        /// <summary>
        /// Searches by tag for a GameObject and returns it. If no GameObject is found it will create the provided _DefualtPrefab as a child of MANAGER.
        /// </summary>
        /// <param name="_TagOfHolder"></param>
        /// <param name="_DefaultPrefab"></param>
        /// <returns></returns>
        public static GameObject CheckGameObjectAvailability(string _TagOfHolder, GameObject _DefaultPrefab)
        {
            GameObject ScriptHolder = null;

            try
            {
                ScriptHolder = GameObject.FindWithTag(_TagOfHolder);
            }
            catch (UnityException _Exception)
            {
                Debug.LogError(_Exception);
            }

            if (ScriptHolder == null)
            {
                ScriptHolder = GameObject.Instantiate(_DefaultPrefab, _DefaultPrefab.transform.position, _DefaultPrefab.transform.rotation) as GameObject;
                ScriptHolder.name = ScriptHolder.name.Split('(')[0]; // "(Clone)" suffix... is annoying here since this happens in the init
                ScriptHolder.transform.SetParent(MANAGER.GET.transform);
            }

            return ScriptHolder;
        }

        /// <summary>
        /// Searches by tag for a GameObject and then through all its children to get provided script. If no GameObject is found it will create the provided _DefualtPrefab as a child of MANAGER.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_TagOfHolder"></param>
        /// <param name="_DefaultPrefab"></param>
        /// <returns></returns>
        public static T CheckScriptAvailability<T>(string _TagOfHolder, GameObject _DefaultPrefab)
        {

            GameObject ScriptHolder = MANAGER.CheckGameObjectAvailability(_TagOfHolder, _DefaultPrefab);

            T Script = ScriptHolder.GetComponentInChildren<T>();
            return Script;
        }

        public void ClearDictionary()
        {
            Singletons.Clear();
        }

        #region Logic
        //this is the init on each game start
        //init manualy via inspector over buttons!
        void Awake()
        {
            if (GET == null)
            {
                DontDestroyOnLoad(this.gameObject);
                GET = this;
            }
            else if (GET != this)
            {
                Destroy(this.gameObject);
            }

            //if there are components missing add them to the dict!
            SetUp();

            //create them all and add into list also initializes them
            InitSinglitons();

            AWAKE_INIT_DONE = true;
        }

        /// <summary>
        /// Returns an Object cast the object into the prefered type.
        /// </summary>
        /// <param name="_SingletonClass"></param>
        /// <returns></returns>
        public Object GetSingleton(Type _SingletonClass)
        {
            if (Singletons.ContainsKey(_SingletonClass))
            {
                Object OUT;
                Singletons.TryGetValue(_SingletonClass, out OUT);
                return OUT;
            }
            else
            {
                Debug.LogError("Could not find referenzed key in " + Singletons + " on static class " + this.ToString());
                return null;
            }
        }

        /// <summary>
        /// Get a casted object if found the generic type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetSingleton<T>() where T : UnityEngine.Object
        {
            if (Singletons.ContainsKey(typeof(T)))
            {
                Object OUT;
                Singletons.TryGetValue(typeof(T), out OUT);
                T Return = OUT as T;
                return Return;
            }
            else
            {
                Debug.LogError("Could not find referenzed key in " + Singletons + " on static class " + this.ToString());
                return default(T);
            }
        }

        /// <summary>
        /// Adds a object into a singleton dictionary. Returns true if it was a succes and false if it was not added.
        /// </summary>
        /// <param name="_KEY"></param>
        /// <param name="_OBJ"></param>
        /// <returns></returns>
        public bool AddS(Type _KEY)
        {
            var tObj = GetComponent(_KEY);

            if (tObj == null)
            {
                tObj = gameObject.AddComponent(_KEY);
            }

            if (Singletons.ContainsKey(_KEY))
                Singletons.set(_KEY, tObj);
            else
                Singletons.Add(_KEY, tObj);

            if (Singletons.ContainsKey(_KEY))
            {
                //Debug.Log("Adding to dict!");
                return true;
            }
            else
            {
                Debug.LogError("Adding key into dict failed");
                return false;
            }
        }
        //there will never be a delete Singleton! Because never add something you dont need somwhere else!

        void OnGUI()
        {
            if (!DEBUG)
                return;

            GUILayout.BeginVertical();

            GUILayout.Label("DICTIONARY SINGLETONS:");

            for (int i = 0; i < Singletons.Count; i++)
            {
                GUILayout.Label(Singletons.Values[i].ToString());
            }

            GUILayout.EndVertical();
        }
    }
    #endregion
}
#endregion
