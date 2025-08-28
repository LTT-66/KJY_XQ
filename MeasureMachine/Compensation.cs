using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasureMachine
{
    internal class Compensation
    {
        public double Compensate_Circle_Method(double ring_size, double[] max, double[] coef_arry)//ring_size为测量值
        {
            double r = ring_size / 2.0, delta;
            double coef = 5;
            double compensate = 0;
            if (ring_size <= max[0])
                coef = coef_arry[0];
            else if (ring_size > max[0] && ring_size <= max[1])
                coef = coef_arry[1];
            else if (ring_size > max[1] && ring_size <= max[2])
                coef = coef_arry[2];
            else if (ring_size > max[2] && ring_size <= max[3])
                coef = coef_arry[3];
            else if (ring_size > max[3])
                coef = coef_arry[4];
            if (coef * coef * r * r - 1 < 0)
                compensate = 0;
            else
                compensate = ((coef * r - 0.5 * Math.Sqrt((coef * coef * r * r - 1)) - 0.5 * coef * coef * r * r * Math.Atan(1 / Math.Sqrt((coef * coef * r * r - 1)))) * 2) / coef;
            delta = ring_size + compensate;
            return delta;//返回修正后的测量值，不是系数
        }

        public double Compensate_Material_Method(double ring_size, double material_coef, double material_t)
        {
            // double material_coef = 11.5;

            double m_Ringsize = ring_size * (1 + material_coef * (20.0 - material_t) * 1E-6);
            return m_Ringsize;
        }

    }
}
