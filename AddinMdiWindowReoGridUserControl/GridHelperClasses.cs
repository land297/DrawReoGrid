using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIKABReoGridWindow
{
    /*public class Grid
    {

        public string DefaultRowHeight { get; set; }
        public string DefaultColWidth { get; set; }

        //public gridHead head { get; set; }
        //public GridStyle style { get; set; }
        //public GridRows[] rows { get; set; }
        //public gridCols cols { get; set; }
        //public GridVborder vBorders { get; set; }
        //public GridHborder hBorders { get; set; }
        public List<GridCells> _cells;
    }*/



    public class GridCells
    {
        public string Row { get; set; }
        public string Column { get; set; }
        public string RowSpan { get; set; }
        public string ColumnSpan { get; set; }
        public string Value { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public string Font { get; set; }
        public string FontSize { get; set; }
        public string Color { get; set; }
        public string Align { get; set; }
        public string TextWrap { get; set; }
        public List<CellBorder> Borders { get; set; }
        public GridCells PreviousRow { get; set; }
        public GridCells PreviousColumn { get; set; }
        private Dictionary<string, GridCells> _dicBackRef;

        public GridCells(Dictionary<string, GridCells> backref)
        {
            _dicBackRef = backref;
            Borders = new List<CellBorder>();
        }
        public int[] GetRowCordinate()
        {
            int preRow = int.Parse(Row) - 1;
            if (preRow < 0)
            { return new int[] { 0, int.Parse(Height) * -1 }; }
            else
            {
                string key = Column + "," + preRow.ToString();
                return new int[] { _dicBackRef[key].GetRowCordinate()[1], _dicBackRef[key].GetRowCordinate()[1] - int.Parse(Height) };
            }

        }
        public int[] GetColumCordinate()
        {
            int preColumn = int.Parse(Column) - 1;
            if (preColumn < 0)
            {
                return new int[] { 0, int.Parse(Width) };
            }
            else
            {
                string key = preColumn.ToString() + "," + Row;
                return new int[] { _dicBackRef[key].GetColumCordinate()[1], _dicBackRef[key].GetColumCordinate()[1] + int.Parse(Width) };
            }

        }
    }

    public class CellBorder
    {
        public string Style { get; set; }
        public string Position { get; set; }
        public string Type { get; set; }
    }

    public class GridBorder
    {
        public string Style { get; set; }
        public string Position { get; set; }
        public List<int> Rows { get; set; }
        public List<int> Columns { get; set; }
        public string Type { get; set; }

        public CellBorder GetCellBorder()
        {
            return new CellBorder() { Style = this.Style, Position = this.Position };
        }
        public GridBorder(string row, string col, string color, string style, string pos, string count, string type)
        {
            Rows = new List<int>();
            Columns = new List<int>();
            Style = style;
            Position = pos;
            Type = type;
            int startRow = int.Parse(row);
            int startColumn = int.Parse(col);

            if (Type == "h")
            {
                for (int i = 0; i < int.Parse(count); i++)
                {
                    Rows.Add(startRow);
                    Columns.Add(startColumn + i);
                }
            }
            else
            {
                for (int i = 0; i < int.Parse(count); i++)
                {
                    Rows.Add(startRow + i);
                    Columns.Add(startColumn);
                }
            }
        }
    }
}
