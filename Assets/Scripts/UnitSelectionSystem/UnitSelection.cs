using System.Collections.Generic;
using UnityEngine;

namespace UnitSelectionSystem
{
    public class UnitSelection : MonoBehaviour
    {
        public static UnitSelection Instance { get; private set; }

        private List<GameObject> _selectedUnitList;
        private Dictionary<GameObject, IUnitSelecting> _unitDictionary;

        public List<Transform> UnitTransformList { get; private set; }

        private void Awake()
        {
            Instance = this;

            _unitDictionary = new Dictionary<GameObject, IUnitSelecting>();
            _selectedUnitList = new List<GameObject>();
            UnitTransformList = new List<Transform>();
        }

        public void AddObjectToUnitList(GameObject unit, IUnitSelecting unitSelecting)
        {
            _unitDictionary.Add(unit, unitSelecting);
            UnitTransformList.Add(unit.transform);
        }
        
        public void RemoveObjectToUnitList(GameObject unit)
        {
            _unitDictionary.Remove(unit);
            UnitTransformList.Remove(unit.transform);
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
                Deselect(unitToAdd);
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
            _selectedUnitList.Remove(unitToDeselect);
            _unitDictionary[unitToDeselect].UnitUnselected();
        }
    }
}
