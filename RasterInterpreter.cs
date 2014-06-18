using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Paint
{
    public class RasterInterpreter
    {
        //fields
        private Bitmap _image = null;

        //Properties
        public Bitmap Image
        {
            get { return _image; }
            set { _image = value; }
        }

        //Functions
        public string Convert(Bitmap img)
        {
            int[,] imagarray = new int[649,414];
            int[,] burnpixels = new int[649, 414];
            List<int> starts = new List<int>();
            List<int> ends = new List<int>();
            bool hasdata = false;
            string outputfile = "";
            for (int y = 1; y < img.Height - 1; y++)
            {
                int end = 0;
                for (int x = 1; x < img.Width - 1; x++)
                {
                    Color p = img.GetPixel(x, y);
                    if (p.R != 255 || p.G != 255 || p.B != 255)
                    {
                        imagarray[x,y] = new int();
                        imagarray[x,y] = 1;
                        //outputfile += "1";
                        burnpixels[x, y] = new int();
                        burnpixels[x, y] = 1;
                        if (!hasdata)
                        {
                            hasdata = true;
                            starts.Add(x);
                        }

                        end = x;
                        

                    }
                    else
                    {
                        imagarray[x,y] = new int();
                        imagarray[x,y] = 0;
                        //outputfile += "0";
                    }
                }
                if (!hasdata)
                {
                    starts.Add(0);
                }
                ends.Add(end);
                hasdata = false;
                //outputfile += System.Environment.NewLine;
            }
            StringBuilder buffer = new StringBuilder();
            int i = 0;
            while (i < 413)
            {
                //left to right
                int k = 1;
                if (starts[i] != 0)
                {
                    if (starts[i] > starts[i-1])
                    {
                        int length = starts[i] - starts[i-1];
                        for (int u = 0; u < length; u++)
                        {
                            buffer.Append("01");
                            if (k == 30)
                            {
                                buffer.Append(",");
                                
                                k = 1;
                            }
                            k++;
                        }

                    }
                    
                    
                    for (int j = starts[i]; j <= ends[i]; j++)
                    {

                        if (burnpixels[j, i] == 1)
                        {
                            buffer.Append("92");
                        }
                        else
                        {
                            buffer.Append("01");
                        }
                        
                        if (k == 30)
                        {
                            buffer.Append(",");
                            k = 1;
                        }
                        k++;
                    }
                    if (ends[i+1] > ends[i])
                    {
                        int length = ends[i+1] - ends[i];
                        for (int u = 0; u < length; u++)
                        {
                            buffer.Append("01");
                            if (k == 30)
                            {
                                buffer.Append(",");
                                k = 1;
                            }
                            k++;
                        }

                    }
                    i++;
                    buffer.Append("0,5");
                    //right to left
                    k = 1;
                    if (ends[i] < ends[i - 1])
                    {
                        int length = ends[i-1] - ends[i];
                        for (int u = 0; u < length; u++)
                        {
                            buffer.Append("03");
                            if (k == 30)
                            {
                                buffer.Append(",");
                                k = 1;
                            }
                            k++;
                        }
                    }
                    for (int j = ends[i]; j >= starts[i]; j--)
                    {
                        if (burnpixels[j, i] == 1)
                        {
                            buffer.Append("94");
                        }
                        else
                        {
                            buffer.Append("03");
                        }
                        if (k == 30)
                        {
                            buffer.Append(",");
                            k = 1;
                        }
                        k++;
                    }
                    if (starts[i] < starts[i + 1])
                    {
                        int length = starts[i+1] - starts[i];
                        for (int u = 0; u < length; u++)
                        {
                            buffer.Append("03");
                            if (k == 30)
                            {
                                buffer.Append(",");
                                k = 1;
                            }
                            k++;
                        }

                    }
                    i++;
                    buffer.Append("0,5,");
                    if (k == 30)
                    {
                        buffer.Append(",");
                        k = 1;
                    }
                    k++;
                }
                else
                {
                    buffer.Append("5,");
                    i++;
                }
            }
            buffer.Append(",r");
            outputfile = buffer.ToString();
            return outputfile;
        }
    }
}
