using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PipesSolver
{
    /// <summary>
    /// Класс головоломки
    /// </summary>
    class Brainteaser
    {
        /// <summary>
        /// Количнство колонок поля
        /// </summary>
        private int _ColCount;
        /// <summary>
        /// Количество строк поля
        /// </summary>
        private int _RowCount;
        /// <summary>
        /// Точка входа
        /// </summary>
        private Point _Enter;
        /// <summary>
        /// Точка выхода
        /// </summary>
        private Point _Exit;
        /// <summary>
        /// Поле
        /// </summary>
        private Pipe[,] _Field;
                
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="parField">Поле</param>
        /// <param name="parColCount">количество колонок</param>
        /// <param name="parRowCount">количество строк</param>
        /// <param name="parEnter">точка входа</param>
        /// <param name="parExit">точка выхода</param>
        public Brainteaser(Pipe[,] parField, int parColCount, int parRowCount, Point parEnter, Point parExit)
        {
            _ColCount = parColCount;
            _RowCount = parRowCount;
            _Enter = parEnter;
            _Exit = parExit;
        
            _Field = parField;
        }

        /// <summary>
        /// Поиск в глубину
        /// </summary>
        /// <returns></returns>
        public bool DepthSolve()
        {
            //создаем дерево решения
            SolutionThree solutionThree = new SolutionThree(null, null, _Enter, 0);
            //назначаем текущей точкой точку входа
            Point currentPoint = _Enter;
            // помечаем, что задача еще не решена
            bool complete = false;

            //создаем начальное направление движения
            Orientation direction = Orientation.Up;
            if (_Enter.X == 0)
            {
                direction = Orientation.Right;
            }
            else if (_Enter.X == _ColCount + 1)
            {
                direction = Orientation.Left;
            }
            else if (_Enter.Y == 0)
            {
                direction = Orientation.Down;
            }
            else if (_Enter.Y == _RowCount + 1)
            {
                direction = Orientation.Up;
            }

            //рекурсивный вызов метода поиска в глубину
            complete = DepthSolve(currentPoint, direction, solutionThree);

            return complete;
        }

       

        private bool DepthSolve(Point parCurrentPoint, Orientation parDirection, SolutionThree parSolutionNode)
        {
            //решение еще не найдено
            bool complete = false;
            //добавляем узел в дерево решений
            SolutionThree solutionThree = parSolutionNode;
            //назначаем текущую точку
            Point currentPoint = parCurrentPoint;
            //назначаем направление движения
            Orientation direction = parDirection;
            //исходя из текущего положения и направления движения вычисляем следующую точку
            Point nextPoint = new Point(currentPoint.X + direction.X, currentPoint.Y + direction.Y);
            //Если следующая точка является точкой выхода,
            if (nextPoint == _Exit)
            {
                //то решение найдено, алгоритм завершен
                complete = true;
            }
            //если текущая труба вывела нас за границы поля
            else if ((nextPoint.X < 1) || (nextPoint.X > _ColCount) || (nextPoint.Y < 1) || (nextPoint.Y > _RowCount))
            {
                //тупик
                complete = false;
                
            }
            else
            {
                //если в следующей точки нету трубы
                if ((object) _Field[nextPoint.X, nextPoint.Y] == null)
                {
                    //тупик
                    complete = false;
                }
                //если в ледующей точке есть труба, то смотрим ее
                else
                {
                    //если следующая труба уже задействована в текущем рассматриваемом пути,
                    if (_Field[nextPoint.X, nextPoint.Y].PartOfPath == true)
                    {
                        //тупик
                        complete = false;
                    }
                    //если следующая труба еще не задействована
                    else 
                    {
                        //получаем следующую трубу
                        Pipe nextPipe = _Field[nextPoint.X, nextPoint.Y];
                        //разворачиваем ее так, какой-то из ее входов выходил на текущую точку
                        nextPipe.RotateTo(currentPoint);
                        //добавляем в дерево решений новую ветку рассмотрения
                        solutionThree.AddChildNode(nextPipe.Clone(), currentPoint);
                        //получаем новое направление движение (куда смотрит второй вход следующей трубы)
                        Orientation nextDirection = direction;
                        if (direction == -nextPipe.FirstHole)
                        {
                            nextDirection = nextPipe.SecondHole.Clone();
                        }
                        else
                        {
                            nextDirection = nextPipe.FirstHole.Clone();
                        }
                        //помечаем, что труба занята в решении
                        nextPipe.PartOfPath = true;
                        //вызываем рекурсивный методи рассматриваем все со следующей точки со следующим направлением
                        complete = DepthSolve(nextPoint, nextDirection, solutionThree.ChildNodes[0]);
                        //если поиск в глубину завершился неудачей и поворачивать еще можно 
                        //(т.е. следующая труба не прямая, ведь ее разворот приведет к томуже результату)
                        if ((complete == false) && (nextPipe.FirstHole != -nextPipe.SecondHole))
                        {
                            //удаляем текущую ветвь дерева решений
                            solutionThree.ChildNodes.RemoveAt(0);
                            //поворачиваем трубу второй раз, другим входом к текущей точке
                            nextPipe.RotateTo(currentPoint);
                            //добавляем в дерево решений новую ветвь
                            solutionThree.AddChildNode(nextPipe, currentPoint);
                            //вычисляем новое направление движения
                            if (direction == -nextPipe.FirstHole)
                            {
                                nextDirection = nextPipe.SecondHole;
                            }
                            else
                            {
                                nextDirection = nextPipe.FirstHole;
                            }
                            //и вызываем поиск в глубину в новом направлении (это уже второй вызов)
                            complete = DepthSolve(nextPoint, nextDirection, solutionThree.ChildNodes[0]);
                            //если и в эту сторону решение не находится
                            if (complete == false)
                            {
                                //то следующая труба - безперспективна
                                nextPipe.PartOfPath = false;
                            }
                        }
                    }
                }
            }

            return complete;
        }
        
        /// <summary>
        /// Метод добавления отдельной трубы на поле
        /// </summary>
        /// <param name="parNewPipe">добавляемая труба</param>
        public void AddPipe(Pipe parNewPipe)
        {
            _Field[parNewPipe.Position.X, parNewPipe.Position.Y] = parNewPipe;  
        }

        /// <summary>
        /// Количество столбцов поля
        /// </summary>
        public int ColCount
        {
            get { return _ColCount;}
        }
        /// <summary>
        /// Количество строк поля
        /// </summary>
        public int RowCount
        {
            get { return _RowCount; }
        }

        /// <summary>
        /// Поле
        /// </summary>
        public Pipe[,] Field
        {
            get { return _Field; }
        }
    }
}
