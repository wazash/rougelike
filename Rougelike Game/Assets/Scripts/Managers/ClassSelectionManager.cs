using System.Collections.Generic;
using Units;
using UnityEngine;

namespace Managers
{
    public class ClassSelectionManager : MonoBehaviour
    {
        [SerializeField] private GameObject classSelectionScreen;
        [SerializeField] private PlayerData[] playerClasses;
        [SerializeField] private ClassSelectionWindow classWindowPrefab;
        [SerializeField] private Transform classWindowParent;

        private readonly List<ClassSelectionWindow> classSelectionWindows = new();

        public GameObject ClassSelectionScreen => classSelectionScreen;
        public List<ClassSelectionWindow> ClassSelectionWindows => classSelectionWindows;


        public void RegisterClassSelectionWindows()
        {
            Debug.Log("Registering class selection windows");
            classSelectionWindows.Clear();

            foreach (var playerClass in playerClasses)
            {
                var classSelectionWindow = Instantiate(classWindowPrefab, classWindowParent);
                classSelectionWindow.SetVisuals(playerClass);
                classSelectionWindows.Add(classSelectionWindow);
                Debug.Log("Registered class selection window", classSelectionWindow);
            }
        }
    }
}
