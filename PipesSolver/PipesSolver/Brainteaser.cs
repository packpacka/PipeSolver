using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;

namespace PipesSolver
{
    /// <summary>
    /// Класс головоломки
    /// </summary>
    public class Brainteaser
    {
        /// <summary>
        /// Структура сведений о выполении алгоритма
        /// </summary>
        public struct SolveResult
        {
            /// <summary>
            /// Метод поиска
            /// </summary>
            public string Method;
            /// <summary>
            /// Результат выполнения
            /// </summary>
            public bool Complete;
            /// <summary>
            /// Время выполнения 
            /// </summary>
            public Stopwatch ProcessTime ;
            /// <summary>
            /// Максимальная глубина
            /// </summary>
            public int MaxDepth;
            /// <summary>
            /// Количество сгенерированных узлов
            /// </summary>
            public int GeneratedNodesCount;
            /// <summary>
            /// Уонечное состояние поля
            /// </summary>
            public Pipe[,] Field;
        }

        /// <summary>
        /// Максимально допустимая глубина дерева поиска
        /// </summary>
        public static int AllowableDepth = 30;

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
        /// Исходное поле
        /// </summary>
        private Pipe[,] _Field;
        /// <summary>
        /// Рабочая копия поля
        /// </summary>
        private Pipe[,] _WorkingField;
        
        /// <summary>
        /// Лог 
        /// </summary>
        private SolveResult _Log;
                
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
        
            _WorkingField = parField;
        }

        /// <summary>
        /// Поиск в глубину
        /// </summary>
        /// <returns></returns>
        public bool DepthSolve()
        {
            //Получаем рабочую копию поля
            _WorkingField = GetFieldCopy(_Field);

            _Log.MaxDepth = 0;
            //Засекаем время выполнения
            _Log.ProcessTime = new Stopwatch();
            _Log.ProcessTime.Start();

            // помечаем, что задача еще не решена
            bool complete = false;

            //создаем дерево решения
            SolutionThree solutionThree = new SolutionThree(null, null, 0);

            //назначаем текущей точкой точку входа
            Point currentPoint = _Enter;
            
            //создаем начальное направление движения
            Orientation direction = new Orientation();
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
            
            //Фиксируем время выполнения
            _Log.ProcessTime.Stop();
            //Записываем результат выполнения
            _Log.Complete = complete;
            _Log.Method = "Поиск в глубину";
            _Log.Field = GetFieldCopy(_WorkingField);
           
            //возвращаем результат поиска
            return complete;
        }

       
        /// <summary>
        /// Рекурсивный поиск в глубину
        /// </summary>
        /// <param name="parCurrentPoint"></param>
        /// <param name="parDirection"></param>
        /// <param name="parSolutionNode"></param>
        /// <returns></returns>
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

            //Если следующая точка является целью,
            if (TargetReached(currentPoint, direction, _WorkingField))
            {
                //то решение найдено, алгоритм завершен
                complete = true;
            }
            // если решение зашло в тупик
            else if (DeadLock(currentPoint, direction, solutionThree.Level, _WorkingField))
            {
                complete = false;
            }
            //если цель не достигнута и следующая точка не в тупике
            else
            {
                List<Move> availableMoves = GetAvailableMoves(currentPoint, direction, _WorkingField);
                for (int i = 0; i < availableMoves.Count; i++)
                {

                    //Совершаем ход
                    MakeMove(availableMoves[i]);

                    //добавляем в дерево решений новую ветку рассмотрения
                    solutionThree.AddChildNode(availableMoves[i]);
                    _Log.GeneratedNodesCount++;
                    //Если уровень узла нового узла дерева больше чем сохраненный в логе, 
                    if (solutionThree.ChildNodes[0].Level > _Log.MaxDepth)
                    {
                        //то записываем его в лог
                        _Log.MaxDepth = solutionThree.ChildNodes[0].Level;
                    }
                    
                    
                    //исходя из текущего положения и направления движения вычисляем следующую точку
                    Point nextPoint = new Point(availableMoves[i].RotatedPipe.Position.X, availableMoves[i].RotatedPipe.Position.Y);

                    //получаем новое направление движение (куда смотрит второй вход следующей трубы)
                    Orientation nextDirection = new Orientation();
                    if (direction == -availableMoves[i].RotatedPipe.FirstHole)
                    {
                        nextDirection = availableMoves[i].RotatedPipe.SecondHole.Clone();
                    }
                    else
                    {
                        nextDirection = availableMoves[i].RotatedPipe.FirstHole.Clone();
                    }                    

                    //вызываем рекурсивный метод и рассматриваем все со следующей точки со следующим направлением
                    complete = DepthSolve(nextPoint, nextDirection, solutionThree.ChildNodes[0]);

                    //Если цель была достигнута, то заканчиваем просмотр возможных вариантов ходов из текущего положения
                    if (complete)
                    {
                        break;
                    }
                    else
                    {
                        CancelMove(availableMoves[i]);
                    }
                }
                
            }            
            //возвращаем результат поиска из текущей точки
            return complete;
        }

        /// <summary>
        /// Получение списка возможных ходов из текущей ситуации
        /// </summary>
        /// <param name="parPoint">Текущее положение</param>
        /// <param name="parDirection">Текущее направление движения</param>
        /// <returns></returns>
        private List<Move> GetAvailableMoves(Point parPoint, Orientation parDirection, Pipe[,] parField)
        {
            
            //инициализируем список возможных ходов
            List<Move> moves = new List<Move>();
            //исходя из текущего положения и направления движения вычисляем следующую точку
            Point dstPoint = new Point(parPoint.X + parDirection.X, parPoint.Y + parDirection.Y);
            
            //если в следующей точке есть труба, то
            if ((object)parField[dstPoint.X, dstPoint.Y] != null)
            {
                //получаем следующую трубу
                Pipe originalDstPipe = parField[dstPoint.X, dstPoint.Y].Clone();
                Pipe dstPipe = originalDstPipe.Clone();
                
                //разворачиваем ее первым входом к текущей точке
                dstPipe.RotateTo(parPoint);
                //добавляем текущею точку и поворнутую получившимся образом трубу в список возможных ходов 
                moves.Add(new Move(parPoint, dstPipe.Clone(), originalDstPipe.Clone()));
                //поворачиваем трубу вторым вхожом к текущей точке
                dstPipe.RotateTo(parPoint);
                //и снова добавляем новый ход в список возможных ходов
                moves.Add(new Move(parPoint, dstPipe.Clone(), originalDstPipe.Clone()));
            }
            //возвращаем список возможных ходов
            return moves;
        }
        /// <summary>
        /// Совершение хода
        /// </summary>
        /// <param name="parMove">Совершаемый ход</param>
        private void MakeMove(Move parMove)
        {
            _WorkingField[parMove.RotatedPipe.Position.X, parMove.RotatedPipe.Position.Y] = parMove.RotatedPipe;
        }

        /// <summary>
        /// Отмена хода
        /// </summary>
        /// <param name="parMove">Отменяемый ход</param>
        private void CancelMove(Move parMove)
        {
            _WorkingField[parMove.OriginalPipe.Position.X, parMove.OriginalPipe.Position.Y] = parMove.OriginalPipe;
        }

        /// <summary>
        /// Проверка достижения целевой ситуации 
        /// </summary>
        /// <param name="parPoint">Текущее положение</param>
        /// <param name="parDirection">Текущее направление</param>
        /// <returns></returns>
        public bool TargetReached(Point parPoint, Orientation parDirection, Pipe[,] parField)
        {
            //метка целостности пути от точки входа до точки выхода
            bool pathIsWhole = true;
            //текущая рассматриваемая точка (сначала задаем ее как следующую из текущего состояния точку)
            Point currentPoint = new Point(parPoint.X + parDirection.X, parPoint.Y + parDirection.Y);
            //если следующая из текущего состояния точка не является точкой выхода, 
            if (currentPoint != _Exit)
            {
                //то цель не достигнута
                return false;
            }
            //если следующая из текущего состояния точка является точкой выхода, 
            //то целесообразно проверить целостность пути от точки входа до точки выхода
            else
            {
                //создаем начальное направление движения
                Orientation direction = new Orientation();
                if (currentPoint.X == 0)
                {
                    direction = Orientation.Right;
                }
                else if (currentPoint.X == _ColCount + 1)
                {
                    direction = Orientation.Left;
                }
                else if (currentPoint.Y == 0)
                {
                    direction = Orientation.Down;
                }
                else if (currentPoint.Y == _RowCount + 1)
                {
                    direction = Orientation.Up;
                }

                
                //Проверяем целостность пути от точки выхода до точки входа
                //переходим из трубы в трубу, пока не достигнем точки входа или не обнаружим разрыв пути
                while (pathIsWhole) 
                {
                    //вычисляем следующую точку
                    Point nextPoint = new Point(currentPoint.X + direction.X, currentPoint.Y + direction.Y);
                    //если она является точкой входа
                    if (nextPoint == _Enter)
                    {
                        //перемещаемся в нее 
                        currentPoint = nextPoint;
                        //и заканчиваем переходы
                        break;
                    }
                    //если точка входа еще не достигнута
                    else
                    {
                        
                        //вычисляем новое направление движения
                        //если в трубу в следующей точке можно войти через первый вход,
                        if (direction == -parField[nextPoint.X, nextPoint.Y].FirstHole)
                        {
                            //следующим направлением назначаем направление второго выхода следующей трубы
                            direction = parField[nextPoint.X, nextPoint.Y].SecondHole;
                            //и переходим в следующую трубу
                            currentPoint = nextPoint;
                        }
                        //если в трубу в следующей точке можно войти через второй вход,
                        else if (direction == -parField[nextPoint.X, nextPoint.Y].SecondHole)
                        {
                            //следующим направлением назначаем направление первого выхода следующей трубы
                            direction = parField[nextPoint.X, nextPoint.Y].FirstHole;
                            //переходим в следующую трубу
                            currentPoint = nextPoint;
                        }
                        //если же труба повернута так, что ни в один из ее входов нельзя войти
                        else
                        {
                            //отмечаем разрыв пути
                            pathIsWhole = false;
                            //следующей ветки цикла не будет
                        }
                    }
                }
                //если после выхода из цикла переходов по трубам метка целостности пути установлена,
                if (pathIsWhole)
                {
                    //значит цель достигнута
                    return true;
                }
                //если был зафиксирован разрыв
                else
                {
                    //цель не  достигнута
                    return false;
                }
            }            
        }

        /// <summary>
        /// Проверка тупика
        /// </summary>
        /// <param name="parPoint">Текущее положение</param>
        /// <param name="parDirection">Текущее направление движения</param>
        /// <param name="parSolutionThree">Текущий езел дерева решений</param>
        /// <returns></returns>
        private bool DeadLock(Point parPoint, Orientation parDirection, int parSolutionThreeLevel, Pipe[,] parField)
        {
            //метка тупика
            bool deadLock = false;
            //вычисляем следующую из текущей ситуации точку
            Point currentPoint = new Point(parPoint.X + parDirection.X, parPoint.Y + parDirection.Y);

            //если достигнута максимально допустимая глубина дерева поиска
            if (parSolutionThreeLevel >= AllowableDepth)
            {
                //заканчиваем поиск, 
                //т.к. считаем, что решение зациклилось и не может быть в нем столько ходов
                //тупик
                deadLock = true;
            }
            //в противном случае
            else
            {
                //если текущая труба вывела нас за границы поля
                if ((currentPoint.X < 1) || (currentPoint.X > _ColCount) || 
                    (currentPoint.Y < 1) || (currentPoint.Y > _RowCount))
                {
                    //тупик
                    deadLock = true;

                }
                else
                {
                    //если в следующей точке нет трубы
                    if ((object)parField[currentPoint.X, currentPoint.Y] == null)
                    {
                        //тупик
                        deadLock = true;
                    }
                }
            }

            //возвращаем флаг тупика (если он был найден)
            return deadLock;
        }       

        /// <summary>
        /// Поиск в ширину
        /// </summary>
        /// <returns></returns>
        public bool BreadthSolve()
        {
            
            
            // помечаем, что задача еще не решена
            bool complete = false;

            bool done = false;

            //создаем начальное направление движения
            Orientation direction = new Orientation();
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
            //назначаем текущей точкой точку входа
            Point currentPoint = _Enter;

            List<BreadthMethodSolutionThree> currentLevelNodes = new List<BreadthMethodSolutionThree>();
            List<BreadthMethodSolutionThree> nextLevelNodes = new List<BreadthMethodSolutionThree>();
            BreadthMethodSolutionThree solutionThree = new BreadthMethodSolutionThree(null, GetFieldCopy(_WorkingField), null, 0);
            currentLevelNodes.Add(solutionThree);

            while (!done)
            {
                for (int i = 0; i < currentLevelNodes.Count; i++)
                {
                    if (TargetReached(currentPoint, direction, currentLevelNodes[i].Field))
                    {
                        done = true;
                        complete = true;
                    }
                    else
                    {
                        if (!DeadLock(currentPoint, direction, solutionThree.Level, currentLevelNodes[i].Field))
                        {
                            
                        }
                    }
                   
                }
                
            }

            return false;
        }

        private Pipe[,] GetFieldCopy(Pipe[,] parField)
        {
            Pipe[,] fieldCopy = new Pipe[_ColCount + 2,_RowCount + 2];
            
            for (int j = 0; j < _RowCount + 2; j++)
                for (int i = 0; i < _ColCount + 2; i++)
                {
                    fieldCopy[i, j] = _WorkingField[i, j];
                }

            return fieldCopy;
        }

        /// <summary>
        /// Метод добавления отдельной трубы на поле
        /// </summary>
        /// <param name="parNewPipe">добавляемая труба</param>
        public void AddPipe(Pipe parNewPipe)
        {
            _WorkingField[parNewPipe.Position.X, parNewPipe.Position.Y] = parNewPipe;  
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

        public SolveResult Log
        {
            get { return _Log; }
        }
    }
}
