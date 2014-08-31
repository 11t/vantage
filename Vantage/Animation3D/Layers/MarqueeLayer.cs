namespace Vantage.Animation3D.Layers
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;

    using SharpDX;

    using Vantage.Animation2D.OsbTypes;
    using Vantage.Animation3D.Animation.EasingCurves;

    public class MarqueeLayer : Layer
    {
        private const int MarqueeLetterColumns = 5;

        private const int MarqueeLetterRows = 6;

        private const string MarqueeLetterData = @"A
11110
10001
10001
11111
10001
10001
B
11110
10001
10001
11110
10001
11111
C
01111
10000
10000
10000
10000
01111
D
11110
10001
10001
10001
10001
11110";

        private static IDictionary<char, int[,]> LetterDictionary = InitializeMarqueeLetterDictionary();

        public MarqueeLayer(string cellSpriteName, int cellSpriteSpacing, int numColumns, int numRows, OsbColor defaultColor)
        {
            Sprite3D[,] cellSpriteArray = new Sprite3D[numRows, numColumns];
            float startPositionX = -cellSpriteSpacing * (numColumns / 2.0f);
            float startPositionY = cellSpriteSpacing * (numRows / 2.0f);
            var startPosition = new Vector3(startPositionX, startPositionY, 0);
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numColumns; j++)
                {
                    var position = startPosition + new Vector3(cellSpriteSpacing * j, -cellSpriteSpacing * i, 0);
                    var cellSprite = this.NewSprite(cellSpriteName);
                    cellSprite.SetPosition(0, position);
                    cellSprite.SetColor(0, defaultColor);
                    cellSpriteArray[i, j] = cellSprite;
                }
            }

            this.ColumnCount = numColumns;
            this.RowCount = numRows;
            this.CellSpriteName = cellSpriteName;
            this.CellSpriteSpacing = cellSpriteSpacing;
            this.CellSpriteArray = cellSpriteArray;
            this.DefaultColor = defaultColor;
        }

        public string CellSpriteName { get; private set; }

        public int CellSpriteSpacing { get; private set; }

        public Sprite3D[,] CellSpriteArray { get; private set; }

        public int ColumnCount { get; private set; }

        public int RowCount { get; private set; }

        public OsbColor DefaultColor { get; private set; }

        public void Display(string displayString, float time, OsbColor color, IEasingCurve easingCurve)
        {
            int rowMarginOffset = 1;
            int numColumns = (displayString.Length * MarqueeLetterColumns) + (displayString.Length - 1);
            int numRows = MarqueeLetterRows;
            int[,] colorIndicatorArray = new int[numRows, numColumns];
            for (int k = 0; k < displayString.Length; k++)
            {
                char letterChar = displayString[k];
                int[,] letterArray = LetterDictionary[letterChar];
                for (int i = 0; i < MarqueeLetterRows; i++)
                {
                    for (int j = 0; j < MarqueeLetterColumns; j++)
                    {
                        colorIndicatorArray[i, (k * MarqueeLetterColumns) + k + j] = letterArray[i, j];
                    }
                }
            }

            for (int i = 0; i < colorIndicatorArray.GetLength(0); i++)
            {
                for (int j = 0; j < colorIndicatorArray.GetLength(1); j++)
                {
                    if (colorIndicatorArray[i, j] != 0)
                    {
                        this.CellSpriteArray[i + rowMarginOffset, j].SetColor(time, color, easingCurve);
                        Debug.WriteLine("colored" + color.ToOsbString());
                    }
                }
            }
        }

        private static IDictionary<char, int[,]> InitializeMarqueeLetterDictionary()
        {
            var letterDictionary = new Dictionary<char, int[,]>();
            var stringReader = new StringReader(MarqueeLetterData);
            while (stringReader.Peek() != -1)
            {
                string letterString = stringReader.ReadLine();
                char letterChar = letterString[0];

                int[,] letterArray = new int[6, 5];
                for (int i = 0; i < 6; i++)
                {
                    string rowString = stringReader.ReadLine();
                    for (int j = 0; j < 5; j++)
                    {
                        if (rowString[j] == '0')
                        {
                            letterArray[i, j] = 0;
                        }
                        else
                        {
                            letterArray[i, j] = 1;
                        }
                    }
                }

                letterDictionary[letterChar] = letterArray;
            }

            return letterDictionary;
        }
    }
}
