using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace NewSaveSystem
{
    public class PoIManager : MonoBehaviour, ISaveable
    {
        public List<PoI> PoIs = new();
        public PoI PoIPrefab;

        private void Awake()
        {
            SaveGameManager.RegisterSaveable(this);
        }

        [Button("Generate PoIs")]
        public void GeneratePoIs(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                GeneratePoI();
            }
        }

        public void GeneratePoI()
        {
            Vector3 position = new(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            PoI poiGameObject = Instantiate(PoIPrefab, position, Quaternion.identity);
            PoIs.Add(poiGameObject);

            SaveGameManager.RegisterSaveable(poiGameObject.GetComponent<PoI>());
        }

        public string GetSaveID() => "PoIManager";

        public object Save()
        {
            List<PoIData> poiDataList = new();
            foreach (PoI poi in PoIs)
            {
                poiDataList.Add((PoIData)poi.Save()); // Execute PoI.Save() and cast the result to PoIData
            }
            return poiDataList;
        }

        public void Load(object saveData)
        {
            var savedPoIs = saveData as List<PoIData>;
            var toRemove = new List<PoI>(PoIs);

            foreach (var poiData in savedPoIs)
            {
                var existingPoi = PoIs.Find(p => p.GetSaveID() == poiData.SaveId);
                if(existingPoi != null)
                {
                    existingPoi.Load(poiData);
                    toRemove.Remove(existingPoi);
                }
                else
                {
                    PoI newPoi = Instantiate(PoIPrefab, poiData.Position, Quaternion.identity);
                    newPoi.Load(poiData);
                    PoIs.Add(newPoi);
                }
            }

            foreach (var poi in toRemove)
            {
                PoIs.Remove(poi);
                Destroy(poi.gameObject);
            }
        }

        public System.Type GetDataType() => typeof(List<PoIData>);
    }
}
