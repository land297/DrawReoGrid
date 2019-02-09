using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Linq;
using ReoGridUserControl;

namespace SIKABReoGridWindow
{
    public static class TranslateRGF
    {

        public static List<string> Translate(string xmlPath)
        {

            //String xData = @"C:\Users\Svamptroll\Desktop\test1.rgf";
            //XmlSerializer x = new XmlSerializer(typeof(Grid));
            //Grid myTest = (Grid)x.Deserialize(new StringReader(xData));

            //XDocument doc = XDocument.Load(xData);
            XElement xml = XElement.Load(xmlPath);

            var defaultHeight = xml.Element("head").Element("default-row-height").Value;
            var defaultWidth = xml.Element("head").Element("default-col-width").Value;
            int maxRow = 200;
            int maxColumn = 200;

            string defaultTextHeight = xml.Element("style").Attribute("font-size").Value;
            string defaultTextFont = xml.Element("style").Attribute("font").Value;



            Dictionary<string, GridCells> _cellDictionary = new Dictionary<string, GridCells>();
            for (int i = 0; i < maxRow; i++)
            {
                for (int j = 0; j < maxColumn; j++)
                {
                    string key = i.ToString() + "," + j.ToString();
                    _cellDictionary.Add(key, new GridCells(_cellDictionary) { Column = i.ToString(), Row = j.ToString(), Height = defaultHeight, Width = defaultWidth });
                }
            }

            var query = from e in xml.Elements("cells").Elements("cell")
                        select new GridCells(_cellDictionary)
                        {
                            Row = (string)e.Attribute("row").Value,
                            Column = (string)e.Attribute("col").Value,
                            RowSpan = e.Attribute("rowspan") != null ? e.Attribute("rowspan").Value : "1",
                            ColumnSpan = e.Attribute("colspan") != null ? e.Attribute("colspan").Value : "1",
                            Value = (string)e.Value,
                            Height = defaultHeight,
                            Width = defaultWidth,
                        };

            foreach (GridCells gc in query)
            {
                string key = gc.Column + "," + gc.Row;
                _cellDictionary[key].RowSpan = gc.RowSpan;
                _cellDictionary[key].Value = gc.Value;
                _cellDictionary[key].FontSize = gc.FontSize;
                _cellDictionary[key].Font = gc.Font;
                _cellDictionary[key].Color = gc.Color;
                _cellDictionary[key].Align = gc.Align;
            }

            query = from e in xml.Elements("cells").Elements("cell")
                        from s in e.Elements("style")
                        select new GridCells(_cellDictionary)
                        {
                            Row = (string)e.Attribute("row").Value,
                            Column = (string)e.Attribute("col").Value,
                            RowSpan = e.Attribute("rowspan") != null ? e.Attribute("rowspan").Value : "1",
                            ColumnSpan = e.Attribute("colspan") != null ? e.Attribute("colspan").Value : "1",
                            Value = (string)e.Value,
                            Height = defaultHeight,
                            Width = defaultWidth,
                            Font = s.Attribute("font") != null ? s.Attribute("font").Value : defaultTextFont,
                            FontSize = s.Attribute("font-size") != null ? s.Attribute("font-size").Value : defaultTextHeight,
                            Color = s.Attribute("color") != null ? s.Attribute("color").Value : "-",
                            Align = s.Attribute("align") != null ? s.Attribute("align").Value : "-",
                            TextWrap = s.Attribute("text-wrap") != null ? s.Attribute("text-wrap").Value : "-",
                        };

            foreach (GridCells gc in query)
            {
                string key = gc.Column + "," + gc.Row;
                _cellDictionary[key].RowSpan = gc.RowSpan;
                _cellDictionary[key].Value = gc.Value;
                _cellDictionary[key].FontSize = gc.FontSize;
                _cellDictionary[key].Font = gc.Font;
                _cellDictionary[key].Color = gc.Color;
                _cellDictionary[key].Align = gc.Align;
                _cellDictionary[key].TextWrap = gc.TextWrap;
            }

            var queryx = from e in xml.Elements("rows").Elements("row")
                         select new string[] { e.Attribute("row").Value, e.Attribute("height").Value };

            foreach (var v in queryx)
            {
                for (int i = 0; i < maxColumn; i++)
                {
                    string key = i.ToString() + "," + v[0];
                    _cellDictionary[key].Height = v[1];
                }
            }


            queryx = from e in xml.Elements("cols").Elements("col")
                     select new string[] { e.Attribute("col").Value, e.Attribute("width").Value };

            foreach (var v in queryx)
            {
                for (int i = 0; i < maxRow; i++)
                {
                    string key = v[0] + "," + i.ToString();
                    _cellDictionary[key].Width = v[1];
                }
            }

            //System.Diagnostics.Debug.WriteLine("****");

            var queryHBorders = from e in xml.Elements("h-borders").Elements("h-border")
                                select new GridBorder((string)e.Attribute("row").Value, (string)e.Attribute("col").Value,
                                e.Attribute("color") != null ? (string)e.Attribute("color").Value : "-", e.Attribute("style") != null ? (string)e.Attribute("style").Value : "-",
                                (string)e.Attribute("pos").Value, (string)e.Attribute("cols").Value, "h") { };
            var queryVBorders = from e in xml.Elements("v-borders").Elements("v-border")
                                select new GridBorder((string)e.Attribute("row").Value, (string)e.Attribute("col").Value,
                                e.Attribute("color") != null ? (string)e.Attribute("color").Value : "-", (string)e.Attribute("style") != null ? (string)e.Attribute("style").Value : "-",
                                (string)e.Attribute("pos").Value, (string)e.Attribute("rows").Value, "v") { };

            List<GridBorder> _borders = queryHBorders.ToList();
            _borders.AddRange(queryVBorders.ToList());

            List<string> stras = new List<string>();
            List<string> text = new List<string>();

            var trams = from e in _cellDictionary.Values where e.Value != null && e.Value != "" select e;
            foreach (GridCells gc in trams)
            {

                int[] rowCordinate;
                int[] columnCordinate;

                string start;

                int rRowCord;

                rowCordinate = gc.GetRowCordinate();
                columnCordinate = gc.GetColumCordinate();

                if (gc.RowSpan == "1")
                {
                    rRowCord = (rowCordinate[0] + rowCordinate[1]) / 2;
                }
                else
                {
                    for (int i = int.Parse(gc.Row); i < int.Parse(gc.Row) + int.Parse(gc.RowSpan); i++)
                    {
                        string key = gc.Column + "," + i.ToString();
                        int[] tempRowCord = _cellDictionary[key].GetRowCordinate();
                        rowCordinate[0] -= tempRowCord[0];
                        rowCordinate[1] -= tempRowCord[1];

                    }

                    rRowCord = (rowCordinate[0] - rowCordinate[1]) / 2;
                }
                start = columnCordinate[0].ToString() + " " + (rRowCord.ToString());
                //end = columnCordinate[1].ToString() + " " + (rowCordinate[0]).ToString();
                text.Add("text;" + start + ";" + gc.Value + ";" + (columnCordinate[1] - columnCordinate[0]).ToString() + ";" + gc.Font + ";" + gc.FontSize + ";" + gc.Align + ";" + gc.Color + ";" + gc.TextWrap);


            }
            // _drawText = text;

            foreach (GridBorder gb in _borders)
            {
                int[] rowCordinate;
                int[] columnCordinate;

                string start;
                string end;

                for (int i = 0; i < gb.Columns.Count; i++)
                {

                    string column = gb.Columns[i].ToString();
                    string row = gb.Rows[i].ToString();

                    string key = column + "," + row;

                    rowCordinate = _cellDictionary[key].GetRowCordinate();
                    columnCordinate = _cellDictionary[key].GetColumCordinate();


                    switch (gb.Type)
                    {
                        case "h":
                            {
                                start = columnCordinate[0].ToString() + " " + (rowCordinate[0].ToString());
                                end = columnCordinate[1].ToString() + " " + (rowCordinate[0]).ToString();
                                stras.Add("stra;" + start + ";" + end + ";" + gb.Style + ";" + column + "," + row);
                                break;
                            }

                        case "v":
                            {
                                start = columnCordinate[0].ToString() + " " + rowCordinate[1].ToString();
                                end = columnCordinate[0].ToString() + " " + rowCordinate[0].ToString();
                                stras.Add("stra;" + start + ";" + end + ";" + gb.Style + ";" + column + "," + row);
                                break;

                            }
                    }

                    //_drawStras = stras;

                }
            }

            List<string> fileContent = new List<string>();
            fileContent.AddRange(stras);
            fileContent.AddRange(text);
            return fileContent;

            //string sssss = System.IO.Path.GetTempPath();


            //File.WriteAllLines(System.IO.Path.GetTempPath() + "grid.txt", fileContent.ToArray());
        }
    }
}
