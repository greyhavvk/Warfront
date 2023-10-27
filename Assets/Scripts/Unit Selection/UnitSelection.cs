using System.Collections.Generic;
using UnityEngine;

namespace Unit_Selection
{
    public class UnitSelection : MonoBehaviour
    {
        private static UnitSelection _instance;
        public static UnitSelection Instance => _instance;
        
        private List<GameObject> _selectedUnitList;
        private List<Transform> _unitTransformList;
        private Dictionary<GameObject, IUnitSelecting> _unitDictionary;

        public List<Transform> UnitTransformList => _unitTransformList;
        private void Awake()
        {
            if (_instance!=null && _instance!=this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }

            _unitDictionary = new Dictionary<GameObject, IUnitSelecting>();
            _selectedUnitList = new List<GameObject>();
            _unitTransformList = new List<Transform>();
        }

        public void AddObjectToUnitList(GameObject unit, IUnitSelecting unitSelecting)
        {
            _unitDictionary.Add(unit, unitSelecting);
            _unitTransformList.Add(unit.transform);
        }
        
        public void RemoveObjectToUnitList(GameObject unit)
        {
            _unitDictionary.Remove(unit);
            _unitTransformList.Remove(unit.transform);
        }

        public void ClickSelect(GameObject unitToAdd)
        {
            DeselectAll();
            _selectedUnitList.Add(unitToAdd);
            _unitDictionary[unitToAdd].UnitSelected();
        }
        
        public void ShiftClickSelect(GameObject unitToAdd)
        {
            if (!_selectedUnitList.Contains(unitToAdd))
            {
                _selectedUnitList.Add(unitToAdd);
                _unitDictionary[unitToAdd].UnitSelected();
            }
            else
            {
                _selectedUnitList.Remove(unitToAdd);
                _unitDictionary[unitToAdd].UnitUnselected();
            }
        }
        
        public void DragSelect(GameObject unitToAdd)
        {
            if (!_selectedUnitList.Contains(unitToAdd))
            {
                _selectedUnitList.Add(unitToAdd);
                _unitDictionary[unitToAdd].UnitSelected();
            }
        }

        public void DeselectAll()
        {
            foreach (var selectedUnit in _selectedUnitList)
            {
                _unitDictionary[selectedUnit].UnitUnselected();
            }
            _selectedUnitList.Clear();
        }

        public void Deselect(GameObject unitToDeselect)
        {
            
        }
    }
}
