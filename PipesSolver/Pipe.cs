using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PipesSolver
{
    /// <summary>
    /// Класс трубы
    /// </summary>
    public class Pipe 
    {
        /// <summary>
        /// Массив шаблонов труб
        /// </summary>
        public static List<Pipe> TemplatePipes = new List<Pipe> {
                        new Pipe(new Point(0, 0), Orientation.Up, Orientation.Right, Properties.Resources.angle1),
                        new Pipe(new Point(0, 0), Orientation.Right, Orientation.Down, Properties.Resources.angle2),
                        new Pipe(new Point(0, 0), Orientation.Down, Orientation.Left, Properties.Resources.angle3),
                        new Pipe(new Point(0, 0), Orientation.Left, Orientation.Up, Properties.Resources.angle4),
                        new Pipe(new Point(0, 0), Orientation.Up, Orientation.Down, Properties.Resources.direct1),
                        new Pipe(new Point(0, 0), Orientation.Left, Orientation.Right, Properties.Resources.direct2)
                        };        

        /// <summary>
        /// Первое отверстие трубы
        /// </summary>
        private Orientation _FirstHole;
        /// <summary>
        /// Второе отверстие трубы
        /// </summary>
        private Orientation _SecondHole;
        /// <summary>
        /// Положение трубы на поле
        /// </summary>
        private Point _Position;
        /// <summary>
        /// Изображение трубы
        /// </summary>
        private Bitmap _Image;
        /// <summary>
        /// Является ли частью рассматриваемого пути
        /// </summary>
        private bool _PartOfPath;
                       
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="parPosition">Положение на поле</param>
        /// <param name="parFirstHole">Первое отверстие </param>
        /// <param name="parSecondHole">Второе отверстие</param>
        /// <param name="parImage">Изображение трубы</param>
        public Pipe(Point parPosition, Orientation parFirstHole, Orientation parSecondHole, Bitmap parImage)
        {
            _Position = parPosition;
            _FirstHole = parFirstHole;
            _SecondHole = parSecondHole;
            _Image = parImage;
            _PartOfPath = false;
        }

        /// <summary>
        /// Создание копии текущего объекта
        /// </summary>
        /// <returns></returns>
        public Pipe Clone()
        {
            return new Pipe(new Point(_Position.X, _Position.Y), _FirstHole.Clone(), _SecondHole.Clone(), _Image);
        }

        /// <summary>
        /// Поворот трубы
        /// </summary>
        /// <param name="parClockwise">по часовой(true) или против часовой(false)</param>
        public void Rotate(bool parClockwise)
        {
            //поворачиваем первое и второе отверстие
            _FirstHole.Rotate(parClockwise);
            _SecondHole.Rotate(parClockwise);

            //просматриваем список шаблонов труб
            for (int i = 0; i < TemplatePipes.Count; i++)
            {
                //как только среди шаблонов находим с отверстиями, ориентированными также как у текущего объекта(трубы)
                if (((_FirstHole == TemplatePipes[i].FirstHole)&&(_SecondHole == TemplatePipes[i].SecondHole))
                   ||((_FirstHole == TemplatePipes[i].SecondHole)&&(_SecondHole == TemplatePipes[i].FirstHole)))
                {
                    //меняем картинку текущего объекта(трубы) на картинку найденного шаблона
                    _Image = TemplatePipes[i].Image;
                    break;
                }
            }
        }

       /// <summary>
       /// Перегруженный оператор сравнения
       /// </summary>
       /// <param name="parFirstPipe"></param>
       /// <param name="parSecondPipe"></param>
       /// <returns></returns>
        public  static bool operator == (Pipe parFirstPipe, Pipe parSecondPipe)
        {
            if ((parFirstPipe.FirstHole.X == parSecondPipe.FirstHole.X) && (parFirstPipe.FirstHole.Y == parSecondPipe.FirstHole.Y) &&
                (parFirstPipe.SecondHole.X == parSecondPipe.SecondHole.X) && (parFirstPipe.SecondHole.X == parSecondPipe.SecondHole.X))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Перегруженный оператор сравнения
        /// </summary>
        /// <param name="parFirstPipe"></param>
        /// <param name="parSecondPipe"></param>
        /// <returns></returns>
        public static bool operator != (Pipe parFirstPipe, Pipe parSecondPipe)
        {
            if ((object)parSecondPipe == null)
                if ((object)parFirstPipe == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            else
            if (!(parFirstPipe.FirstHole.X == parSecondPipe.FirstHole.X) && (parFirstPipe.FirstHole.Y == parSecondPipe.FirstHole.Y) &&
                (parFirstPipe.SecondHole.X == parSecondPipe.SecondHole.X) && (parFirstPipe.SecondHole.X == parSecondPipe.SecondHole.X))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Поворот трубы ближайшим отверстием по направлению к заданной точке
        /// </summary>
        /// <param name="parEnterPoint"></param>
        public void RotateTo(Point parEnterPoint)
        {
            //поворачиваем трубу, не долго думая
            Rotate(true);
            //пока
            while (true)
            {
                //первое или второе отверстие не станут смотреть в сторону заданной точки
                if (((parEnterPoint.X == _Position.X + _FirstHole.X) && (parEnterPoint.Y == _Position.Y + _FirstHole.Y)) ||
                    ((parEnterPoint.X == _Position.X + _SecondHole.X) && (parEnterPoint.Y == _Position.Y + _SecondHole.Y)))
                {
                    //если повернули как надо, выходим из цикла
                    break;
                }
                //иначе
                else
                {
                    //продолжаем поворачивать
                    Rotate(true);
                }
            }
        }

        /// <summary>
        /// Первое отверстие трубы
        /// </summary>
        public Orientation FirstHole
        { get { return _FirstHole; } }
        /// <summary>
        /// Второе отверстие трубы
        /// </summary>
        public Orientation SecondHole
        { get { return _SecondHole; } }
        /// <summary>
        /// Положение трубы на поле
        /// </summary>
        public Point Position
        { 
            get { return _Position; }
            set { _Position = value; }  
        }
        /// <summary>
        /// Изображение трубы
        /// </summary>
        public Bitmap Image
        {
            get { return _Image; }
            set { _Image = value; }  
        }
        /// <summary>
        /// Является ли труба частью
        /// </summary>
        public bool PartOfPath
        {
            get { return _PartOfPath; }
            set { _PartOfPath = value; }
        }
    }
}
