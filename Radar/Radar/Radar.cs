using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radar
{
    public class Radar
    {

        public float[] distanceBuffer { get; set; }

        public Radar()
        {
            distanceBuffer = new float[180];
        }

        private Point GetPoint(int distance, int angle)
        {
            double x, y, angleRad;
            angle = 180 - angle;
            angleRad = angle * Math.PI / 180f;
            x = Math.Cos(angleRad) * distance;
            y = Math.Sin(angleRad) * distance;
            
            return new Point((int)x, (int)y);
        }

        public Point MeasurePoint(string data, int scale)
        {
            int angle, distance;
            string[] reading = data.Split(';');
            if (reading.Length == 2)
            {
                angle = Int32.Parse(reading[0]);
                distance = Int32.Parse(reading[1]) * scale;


                return GetPoint(distance, angle);
            }
            // else throw new ExecutionEngineException("2 values expected!");
            else return new Point(1, 1);
           

        }

    }
}
