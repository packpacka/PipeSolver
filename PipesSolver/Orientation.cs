using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PipesSolver
{
    /// <summary>
    /// Класс вектора направления 
    /// </summary>
    public class Orientation
    {       
        /// <summary>
        /// Направление вверх
        /// </summary>
        public static Orientation Up
        { get { return new Orientation(new Point(0, -1)); } }
        /// <summary>
        /// Направление вниз
        /// </summary>
        public static Orientation Down
        { get { return new Orientation(new Point(0, 1)); } }
        /// <summary>
        /// Направление влево
        /// </summary>
        public static Orientation Left
        { get { return new Orientation(new Point(-1, 0)); } }
        /// <summary>
        /// Направление вправо
        /// </summary>
        public static Orientation Right
        { get { return new Orientation(new Point(1, 0)); } }

        /// <summary>
        /// Горизонтальная составляющая вектора направления
        /// </summary>
        private int _X;
        /// <summary>
        /// Вертикальная составляющая вектора направления
        /// </summary>
        private int _Y;

        /// <summary>
        /// Массив направлений по умолчанию(вверх, вниз, влево, вправо. Используются для поворота вектора
        /// </summary>
        private List<Point> _Orientations = new List<Point>();

        /// <summary>
        /// Конструктор класса
        /// </summary>
        private Orientation()
        {
            _Orientations.Add(new Point(1, 0));
            _Orientations.Add(new Point(0, 1));
            _Orientations.Add(new Point(-1, 0));
            _Orientations.Add(new Point(0, -1));

            _X = _Orientations[0].X;
            _Y = _Orientations[0].Y;
        }
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="parX">Горизонтальная составляющая вектора направления</param>
        /// <param name="parY">Вертикальная составляющая вектора направления</param>
        private Orientation(int parX, int parY)
        {
            _Orientations.Add(new Point(1, 0));
            _Orientations.Add(new Point(0, 1));
            _Orientations.Add(new Point(-1, 0));
            _Orientations.Add(new Point(0, -1));

            _X = parX;
            _Y = parY;
        }

        /// <summary>
        /// Конструктор формы
        /// </summary>
        /// <param name="parVector">вектор направления</param>
        private Orientation(Point parVector)
        {
            _Orientations.Add(new Point(1, 0));
            _Orientations.Add(new Point(0, 1));
            _Orientations.Add(new Point(-1, 0));
            _Orientations.Add(new Point(0, -1));
            _X = parVector.X;
            _Y = parVector.Y;
        }
        /// <summary>
        /// Повернуть вектор направления
        /// </summary>
        /// <param name="parClockwise"></param>
        public void Rotate(bool parClockwise)
        {
            
            int i = 0;
            //Если поворачиваем по часовой стрелке
            if (parClockwise)
            {    
                //Перебираем шаблонные векторы в прямую сторону
                for (i = 0; i < _Orientations.Count; i++)
                {
                    //Как только нашли тот, что соответствует текущему 
                    //выходим из цикла и запоминаем индекс шаблонного вектора
                    if ((_Orientations[i].X == _X) && (_Orientations[i].Y == _Y))
                        break;
                }
                //если индекс оказался последним в массиве шаблонов, то следующим будет тот, что в начале массива шаблонов
                if (i == _Orientations.Count - 1)
                    i = 0;
                //иначе просто увеличиваем индекс, получая следующего по часовой стрелке вектора
                else
                    i++;
                
            }
            //аналогичным образом просматриваем если поворот против часовой
            else
            {
                //индексы смотрим в обратном порядке
                for (i = 0; i < _Orientations.Count; i--)
                {
                    if ((_Orientations[i].X == _X) && (_Orientations[i].Y == _Y))
                        break;
                }
                if (i == 0)
                    i = _Orientations.Count - 1;
                else
                    i--;
            }

            _X = _Orientations[i].X;
            _Y = _Orientations[i].Y;
        }

        /// <summary>
        /// создаем копию текущего объекта
        /// </summary>
        /// <returns></returns>
        public Orientation Clone()
        {
            return new Orientation(_X, _Y);
        }
        /// <summary>
        /// перегруженный оператор минус
        /// </summary>
        /// <param name="parOrientation"></param>
        /// <returns></returns>
        public static Orientation operator -(Orientation parOrientation)
        {
            return new Orientation(new Point(-parOrientation.X, -parOrientation.Y));
        }
        /// <summary>
        /// перегруженный оператор равно
        /// </summary>
        /// <param name="parFirstOrientation"></param>
        /// <param name="parSecondOrientation"></param>
        /// <returns></returns>
        public static bool operator ==(Orientation parFirstOrientation, Orientation parSecondOrientation)
        {
            if ((parFirstOrientation.X == parSecondOrientation.X) && (parFirstOrientation.Y == parSecondOrientation.Y))
                return true;
            else
                return false;
        }
        /// <summary>
        /// перегруженный оператор не равно
        /// </summary>
        /// <param name="parFirstOrientation"></param>
        /// <param name="parSecondOrientation"></param>
        /// <returns></returns>
        public static bool operator !=(Orientation parFirstOrientation, Orientation parSecondOrientation)
        {
            if (!((parFirstOrientation.X == parSecondOrientation.X) && (parFirstOrientation.Y == parSecondOrientation.Y)))
                return true;
            else
                return false;
        }


        /// <summary>
        /// Горизонтальная составляющая вектора направления
        /// </summary>
        public int X
        { get { return _X; } }
        /// <summary>
        /// Вертикальная составляющая вектора направления
        /// </summary>
        public int Y
        { get { return _Y; } }

    }
}
