using System.Collections.Generic;
using System.Linq;
using AZ.Core;
using LordAmbermaze.Core;
using UnityEditor;
using UnityEngine;

namespace LordAmbermaze.CellHighlight
{
    public class CellHighlightManager : Singleton<CellHighlightManager>
    {
        [SerializeField] private GameObject _cellHighlight;
        [SerializeField] private float _time = 0.2f;
        private float _blinkTime = 0.5f;
        private Dictionary<int, List<ICellHighlight>> _callersDict = new Dictionary<int, List<ICellHighlight>>();
        private Stack<ICellHighlight> _availableCells = new Stack<ICellHighlight>();
        private Dictionary<CellHighlightType, Color> _colors = new Dictionary<CellHighlightType, Color>()
        {
            {CellHighlightType.Red, new Color(1,0,0,0.25f)},
            {CellHighlightType.Yellow, new Color(1,0.7f,0,0.1f)},
            {CellHighlightType.Green, new Color(0,1,0,0.25f)},
        };

        public void Start()
        {
            EventManager.StartListening(EventList.Death, OnDeath);
        }

        private void OnDeath()
        {
            var callerIds = _callersDict.Keys.ToList();
            foreach (var callerId in callerIds)
            {
                RemoveHighlightCells(callerId);
            }
        }

        public void HighlightCells(ICollection<Vector2> zoneCells, CellHighlightType cellHighlightType)
        {
            var color = _colors[cellHighlightType];
            var initColor = new Color(color.r, color.g, color.b, 0);
            foreach (var zoneCell in zoneCells)
            {
                var highlightCell = GetCell();
                highlightCell.SetActivate(true);
                highlightCell.SetPos(zoneCell);
                highlightCell.ColorTo(initColor, color, _time);
            }
        }

        public void PermanentHighlightCells(int callerId, IEnumerable<Vector2> zoneCells, CellHighlightType cellHighlightType)
        {
            var cellsList = new List<ICellHighlight>();
            var color = _colors[cellHighlightType];
            var initColor = new Color(color.r, color.g, color.b, 0);
            foreach (var zoneCell in zoneCells)
            {
                var highlightCell = GetCell();
                highlightCell.SetActivate(true);
                highlightCell.SetPos(zoneCell);
                highlightCell.PermanentColorTo(initColor, color, _time);
                cellsList.Add(highlightCell);
            }
            _callersDict.Add(callerId, cellsList);
        }

        public void SetCellsColor(int callerId, IEnumerable<Vector2> zoneCells, CellHighlightType cellHighlightType)
        {
            var cellsList = new List<ICellHighlight>();
            var color = _colors[cellHighlightType];
            var initColor = new Color(color.r, color.g, color.b, 0);
            foreach (var zoneCell in zoneCells)
            {
                var highlightCell = GetCell();
                highlightCell.SetActivate(true);
                highlightCell.SetPos(zoneCell);
                highlightCell.SetColor(color);
                cellsList.Add(highlightCell);
            }
            _callersDict.Add(callerId, cellsList);
        }

        public void RemoveCellsColor(int callerId)
        {
            if (_callersDict.TryGetValue(callerId, out var cellsList))
            {
                foreach (var highlightCell in cellsList)
                {
                    highlightCell.RemoveColor();
                }

                _callersDict.Remove(callerId);
            }
        }

        public void PermanentBlinkCells(int callerId, ICollection<Vector2> zoneCells, CellHighlightType cellHighlightType)
        {
            var cellsList = new List<ICellHighlight>();
            var color = _colors[cellHighlightType];
            var initColor = new Color(color.r, color.g, color.b, 0);
            foreach (var zoneCell in zoneCells)
            {
                var highlightCell = GetCell();
                highlightCell.SetActivate(true);
                highlightCell.SetPos(zoneCell);
                highlightCell.BlinkColorTo(initColor, color, _blinkTime);
                cellsList.Add(highlightCell);
            }
            _callersDict.Add(callerId, cellsList);
        }

        public void RemoveHighlightCells(int callerId)
        {
            if (_callersDict.TryGetValue(callerId, out var cellsList))
            {
                foreach (var highlightCell in cellsList)
                {
                    highlightCell.RemoveColor(_time);
                }

                _callersDict.Remove(callerId);
            }
        }

        private ICellHighlight GetCell()
        {
            if (_availableCells.Count > 0)
            {
                return _availableCells.Pop();
            }

            return CreateNewCell();
        }

        private ICellHighlight CreateNewCell()
        {
            var highlightObj = Instantiate(_cellHighlight, Vector2.zero, Quaternion.identity, transform);
            var highlightCell = highlightObj.GetComponent<ICellHighlight>();
            highlightCell.Init(this);
            return highlightCell;
        }

        public void CellFree(ICellHighlight cellHighlight)
        {
            cellHighlight.SetActivate(false);
            _availableCells.Push(cellHighlight);
        }

        
    }
}