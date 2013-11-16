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
            /// Длина пути решения
            /// </summary>
            public int PathLenght;
            /// <summary>
            /// Максимальная глубина дерева поиска
            /// </summary>
            public int MaxDepth;
            /// <summary>
            /// Количество сгенерированных узлов
            /// </summary>
            public int GeneratedNodesCount;
            /// <summary>
            /// Направленность дерева поиска
            /// </summary>
            public double Directionality;
            /// <summary>
            /// Разветвленность дерева описка
            /// </summary>
            public double Branching;
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
        
            _Field = parField;
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
            _Log.PathLenght = 0;

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
            //если решение было найдено
            if (complete)
            {
                //рассчитываем разветвленность и направленность
                _Log.Branching = (double)_Log.GeneratedNodesCount / (double)_Log.PathLenght;
                _Log.Directionality = Math.Pow(_Log.GeneratedNodesCount, ((double)1 / _Log.PathLenght));
            }
            //если решение небыло найдено
            else
            {
                //указываем что длина пути решения, разветвленность и направленность не вычеслены числом -1
                _Log.PathLenght = -1;
                _Log.Branching = -1;
                _Log.Directionality = -1;
            }
            //возвращаем результат поиска
            return complete;
        }

        /// <summary>
        /// Рекурсивный поиск в глубину
        /// </summary>
        /// <param name="parCurrentPoint">текущее положение</param>
        /// <param name="parDirection">текущее направление</param>
        /// <param name="parSolutionNode">узел дерева</param>
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
            
            
            //Если текущая ситуация является целевой
            if (TargetReached(currentPoint, direction, _WorkingField))
            {
                //то решение найдено, алгоритм завершен
                complete = true;
                //запоминаем длину пути до этого узла
                _Log.PathLenght = solutionThree.Level;
            }
            // если решение зашло в тупик
            else if (DeadLock(currentPoint, direction, solutionThree, _WorkingField))
            {
                complete = false;
            }
            //если цель не достигнута и следующая точка не в тупике
            else
            {
                //получаем список возможных ходов из текущей ситуации
                List<Move> availableMoves = GetAvailableMoves(currentPoint, direction, _WorkingField);
                //проходим по всем ходами
                for (int i = 0; i < availableMoves.Count; i++)
                {

                    //Совершаем ход
                    MakeMove(availableMoves[i], _WorkingField);

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
                    Orientation nextDirection = GetNextDirection(currentPoint, availableMoves[i].RotatedPipe);

                    //вызываем рекурсивный метод и рассматриваем все со следующей точки со следующим направлением
                    complete = DepthSolve(nextPoint, nextDirection, solutionThree.ChildNodes[0]);

                    //Если цель была достигнута, 
                    if (complete)
                    {
                        //то заканчиваем просмотр возможных вариантов ходов из текущего положения
                        break;
                    }
                    else
                    {
                        //отменяем ход
                        CancelMove(availableMoves[i], _WorkingField);
                        //удаляем дочерний узел дерева
                        solutionThree.DeleteChildNode(0);
                    }
                }
                
            }            
            //возвращаем результат поиска из текущей точки
            return complete;
        }


        /// <summary>
        /// Поиск по оценочной функции
        /// </summary>
        /// <returns></returns>
        public bool GradientSolve()
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
            complete = GradientSolve(currentPoint, direction, solutionThree);

            //Фиксируем время выполнения
            _Log.ProcessTime.Stop();
            //Записываем результат выполнения
            _Log.Complete = complete;
            //Название метода
            _Log.Method = "Метод градиента";
            //Сохраняем результирующее поле
            _Log.Field = GetFieldCopy(_WorkingField);
            //если решение было найдено
            if (complete)
            {
                //рассчитываем разветвленность и направленность
                _Log.Branching = (double)_Log.GeneratedNodesCount / (double)_Log.PathLenght;
                _Log.Directionality = Math.Pow(_Log.GeneratedNodesCount, ((double)1 / _Log.PathLenght));
            }
            //если решение небыло найдено
            else
            {
                //указываем что длина пути решения, разветвленность и направленность не вычеслены числом -1
                _Log.PathLenght = -1;
                _Log.Branching = -1;
                _Log.Directionality = -1;
            }

            //возвращаем результат поиска
            return complete;
        }

        /// <summary>
        /// Рекурсивный поиск по оценочной функции
        /// </summary>
        /// <param name="parCurrentPoint"></param>
        /// <param name="parDirection"></param>
        /// <param name="parSolutionNode"></param>
        /// <returns></returns>
        private bool GradientSolve(Point parCurrentPoint, Orientation parDirection, SolutionThree parSolutionNode)
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
                //запоминаем текущий уровень дерева решений( он же лина пути решения)
                _Log.PathLenght = solutionThree.Level;
            }
            // если решение зашло в тупик
            else if (DeadLock(currentPoint, direction, solutionThree, _WorkingField))
            {
                complete = false;
            }
            //если цель не достигнута и следующая точка не в тупике
            else
            {
                //Получаем список возможных ходов
                List<Move> availableMoves = GetAvailableMoves(currentPoint, direction, _WorkingField);

                //СОРТИРУЕМ ИХ СОГЛАСНО ОЦЕНОЧНОЙ ФУНКЦИИ
                SortMovesByEstimator(availableMoves, currentPoint, direction);

                //проходим по всем ходам
                for (int i = 0; i < availableMoves.Count; i++)
                {

                    //Совершаем ход
                    MakeMove(availableMoves[i], _WorkingField);

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
                    Orientation nextDirection = GetNextDirection(currentPoint, availableMoves[i].RotatedPipe);

                    //вызываем рекурсивный метод и рассматриваем все со следующей точки со следующим направлением
                    complete = GradientSolve(nextPoint, nextDirection, solutionThree.ChildNodes[0]);

                    //Если цель была достигнута, то заканчиваем просмотр возможных вариантов ходов из текущего положения
                    if (complete)
                    {
                        break;
                    }
                    else
                    {
                        CancelMove(availableMoves[i], _WorkingField);
                    }
                }

            }
            //возвращаем результат поиска из текущей точки
            return complete;
        }

        /// <summary>
        /// Поиск в ширину
        /// </summary>
        /// <returns></returns>
        public bool BreadthSolve()
        {
            //Получаем рабочую копию поля
            _WorkingField = GetFieldCopy(_Field);


            //запускаем счетчик времени выполнения процедуры
            _Log.ProcessTime = new Stopwatch();
            _Log.ProcessTime.Start();
            _Log.PathLenght = 0;

            //инициализируем список вершин дерева решений на текущем уровне
            List<SolutionThree> currentLevelNodes = new List<SolutionThree>();
            //и на следующем уровне
            List<SolutionThree> nextLevelNodes = new List<SolutionThree>();

            // помечаем, что задача еще не решена
            bool complete = false;

            //флаг завершения алгоритма
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

            //добавляем в дерево решений первую(корневую) вершину
            Pipe theFirstPipe = new Pipe(_Enter, -direction, direction, null);
            Move theFirstMove = new Move(new Point(_Enter.X - direction.X, _Enter.Y - direction.Y), theFirstPipe, theFirstPipe, GetFieldCopy(_WorkingField)); 
            SolutionThree solutionThree = new SolutionThree(theFirstMove, null, 0);


            //добавляем корневую вершину в список вершин текущего уровня
            currentLevelNodes.Add(solutionThree);
            //считаем количество сгенерированных вершин
            _Log.GeneratedNodesCount++;

            //Сначала обрабатываем отдельно первую вершину дерева решений вне общего цикла
            //если это целевая вершина
            if (TargetReached(currentPoint, direction, currentLevelNodes[0].Move.Field))
            {
                //завершаем алгоритм
                done = true;
                complete = true;
                _Log.PathLenght = currentLevelNodes[0].Level;
            }
            //если вершина не целевая
            else
            {
                //далее обрабатываем все остальные варианты в цикле

                //пока алгоритм не завершен
                while (!done)
                {
                    //для каждого узла дерева на текущем уровне
                    for (int i = 0; (i < currentLevelNodes.Count) & (!done); i++)
                    {
                        //получаем текущее положение и направление движения
                        currentPoint = currentLevelNodes[i].Move.RotatedPipe.Position;
                        direction = GetNextDirection(currentLevelNodes[i].Move.Enter, currentLevelNodes[i].Move.RotatedPipe);
                        
                        //получаем список возможных ходов для рассматриваемого узла текущего уровня
                        List<Move> availableMove = GetAvailableMoves(currentPoint, direction, currentLevelNodes[i].Move.Field);
                        //для каждого возможного хода
                        for (int j = 0; (j < availableMove.Count) && (!done); j++)
                        {
                            //совершаем ход на поле рассматриваемого узла
                            MakeMove(availableMove[j], availableMove[j].Field);
                            //добавляем ход и получившееся поле в дерево решений
                            SolutionThree newNode = new SolutionThree(availableMove[j],currentLevelNodes[i], currentLevelNodes[i].Level + 1); 
                            
                            nextLevelNodes.Add(newNode);
                            _Log.GeneratedNodesCount++;

                            //фиксируем максимальный уровень дерева
                            if (_Log.MaxDepth < newNode.Level)
                            {
                                _Log.MaxDepth = newNode.Level;
                            }

                            //получаем следующее состояние, соответствующее сгенерированной вершине
                            Point nextPoint = availableMove[j].RotatedPipe.Position;
                            Orientation nextDirection = GetNextDirection(availableMove[j].Enter, availableMove[j].RotatedPipe);

                            //если в сгенерированном узле цель достигнута
                            if (TargetReached(nextPoint, nextDirection, newNode.Move.Field))
                            {
                                //завершаем алгоритм
                                done = true;
                                //цель достигнута
                                complete = true;
                                //записываем результирующее поле
                                _WorkingField = newNode.Move.Field;
                                //запоминаем текущий уровень дерева решений( он же лина пути решения)
                                _Log.PathLenght = newNode.Level;
                            }
                            //если цель не достигнута
                            else
                            {
                                // и не тупик
                                if (DeadLock(nextPoint, nextDirection, newNode, newNode.Move.Field))
                                {
                                   //удаляем сгенерированный узел из списка узлов следующего уровня
                                    nextLevelNodes.Remove(newNode);
                                    _Log.GeneratedNodesCount--;
                                }
                            }
                            
                        }
                        
                    }

                    //если была достигнута максимальная вершина дерева
                    if (_Log.MaxDepth >= AllowableDepth)
                    {
                        //алгоритм завершен
                        done = true;
                    }
                    //или если на следующем уровне нету узлов (т.е ни из одного узла дерева текущего уровня нельзя совершить ход)
                    else if (nextLevelNodes.Count == 0)
                    {
                        //алгоритм завершен
                        done = true;
                    }
                    //если еще есть куда ходить
                    else
                    {
                        //переходим на следующий уровень дерева решений
                        currentLevelNodes = nextLevelNodes;
                        nextLevelNodes = new List<SolutionThree>();
                    }
                }

            }//пока не завершится алгоритм повторяем все вышеизложенное

            //возвращаем результаты
            _Log.Method = "Поиск в ширину";
            _Log.Field = _WorkingField;
            _Log.Complete = complete;
            //если решение было найдено
            if (complete)
            {
                //рассчитываем разветвленность и направленность
                _Log.Branching = (double)_Log.GeneratedNodesCount / (double)_Log.PathLenght;
                _Log.Directionality = Math.Pow(_Log.GeneratedNodesCount, ((double)1 / _Log.PathLenght));
            }
            //если решение небыло найдено
            else
            {
                //указываем что длина пути решения, разветвленность и направленность не вычеслены числом -1
                _Log.PathLenght = -1;
                _Log.Branching = -1;
                _Log.Directionality = -1;
            }

            return complete;
        }

        /// <summary>
        /// Стратегия локально-углубленного поиска
        /// </summary>
        /// <param name="parLocalDepth">глубина локального углубления</param>
        /// <returns></returns>
        public bool PartialyRecess(int parLocalDepth)
        {
            //Получаем рабочую копию поля
            _WorkingField = GetFieldCopy(_Field);


            //запускаем счетчик времени выполнения процедуры
            _Log.ProcessTime = new Stopwatch();
            _Log.ProcessTime.Start();
            _Log.PathLenght = 0;

            // помечаем, что задача еще не решена
            bool complete = false;

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

            //добавляем в дерево решений первую(корневую) вершину
            Pipe theFirstPipe = new Pipe(_Enter, -direction, direction, null);
            Move theFirstMove = new Move(new Point(_Enter.X - direction.X, _Enter.Y - direction.Y), theFirstPipe, theFirstPipe, GetFieldCopy(_WorkingField));
            SolutionThree solutionThree = new SolutionThree(theFirstMove, null, 0);

            //запускаем поиск
            complete = PartialyRecess(solutionThree, parLocalDepth);

            //возвращаем результаты
            _Log.Method = "Стратегия локально-углубленного поиска";
            _Log.Field = _WorkingField;
            _Log.Complete = complete;
            //если решение было найдено
            if (complete)
            {
                //рассчитываем разветвленность и направленность
                _Log.Branching = (double)_Log.GeneratedNodesCount / (double)_Log.PathLenght;
                _Log.Directionality = Math.Pow(_Log.GeneratedNodesCount, ((double)1 / _Log.PathLenght));
            }
            //если решение небыло найдено
            else
            {
                //указываем что длина пути решения, разветвленность и направленность не вычеслены числом -1
                _Log.PathLenght = -1;
                _Log.Branching = -1;
                _Log.Directionality = -1;
            }
            return complete;
        }

        /// <summary>
        /// Частично-углубленный метод поиска
        /// </summary>
        /// <param name="parSolutionThree">узел дерева из которого совершается углубление</param>
        /// <param name="parLocalDepth">величина локального углубления</param>
        /// <returns></returns>
        private bool PartialyRecess(SolutionThree parSolutionThree, int parLocalDepth)
        {
            // помечаем, что задача еще не решена
            bool complete = false;
            //флаг завершения алгоритма
            bool done = false;

            int localDepth = 0;

            //инициализируем список вершин дерева решений на текущем уровне
            List<SolutionThree> currentLevelNodes = new List<SolutionThree>();
            //и на следующем уровне
            List<SolutionThree> nextLevelNodes = new List<SolutionThree>();


            //получаем текущее положение и направление движения
            Point currentPoint = parSolutionThree.Move.RotatedPipe.Position;
            Orientation direction = GetNextDirection(parSolutionThree.Move.Enter, parSolutionThree.Move.RotatedPipe);

            //добавляем корневой узел в список вершин текущег уровня
            currentLevelNodes.Add(parSolutionThree);

            //Сначала обрабатываем отдельно первую вершину дерева решений вне общего цикла
            //если это целевая вершина
            if (TargetReached(currentPoint, direction, currentLevelNodes[0].Move.Field))
            {
                //завершаем алгоритм
                done = true;
                complete = true;
                _WorkingField = currentLevelNodes[0].Move.Field;
                _Log.PathLenght = currentLevelNodes[0].Level;
            }
            //если вершина не целевая
            else
            {
                //пока алгоритм не завершен
                while (!done)
                {

                    //для каждого узла дерева на текущем уровне
                    for (int i = 0; (i < currentLevelNodes.Count) & (!done); i++)
                    {

                        //если пора углубляться
                        if (localDepth == parLocalDepth)
                        {
                            complete = PartialyRecess(currentLevelNodes[i], parLocalDepth);
                            done = complete;
                        }
                        //иначе выполняем стандартный поиск в ширину
                        else
                        {

                            //получаем текущее положение и направление движения
                            currentPoint = currentLevelNodes[i].Move.RotatedPipe.Position;
                            direction = GetNextDirection(currentLevelNodes[i].Move.Enter, currentLevelNodes[i].Move.RotatedPipe);

                            //получаем список возможных ходов для рассматриваемого узла текущего уровня
                            List<Move> availableMove = GetAvailableMoves(currentPoint, direction, currentLevelNodes[i].Move.Field);

                            //для каждого возможного хода
                            for (int j = 0; (j < availableMove.Count) && (!done); j++)
                            {
                                //совершаем ход на поле рассматриваемого узла
                                MakeMove(availableMove[j], availableMove[j].Field);
                                //добавляем ход и получившееся поле в дерево решений
                                SolutionThree newNode = new SolutionThree(availableMove[j], currentLevelNodes[i], currentLevelNodes[i].Level + 1);
                                nextLevelNodes.Add(newNode);
                                _Log.GeneratedNodesCount++;

                                //фиксируем максимальный уровень дерева
                                if (_Log.MaxDepth < newNode.Level)
                                {
                                    _Log.MaxDepth = newNode.Level;
                                }
                                //получаем следующее состояние, соответствующее сгенерированной вершине
                                Point nextPoint = availableMove[j].RotatedPipe.Position;
                                Orientation nextDirection = GetNextDirection(availableMove[j].Enter, availableMove[j].RotatedPipe);

                                //если в сгенерированном узле цель достигнута
                                if (TargetReached(nextPoint, nextDirection, newNode.Move.Field))
                                {
                                    //завершаем алгоритм
                                    done = true;
                                    //цель достигнута
                                    complete = true;
                                    //записываем результирующее поле
                                    _WorkingField = newNode.Move.Field;
                                    //запоминаем текущий уровень дерева решений( он же лина пути решения)
                                    _Log.PathLenght = newNode.Level;
                                }
                                //если цель не достигнута
                                else
                                {
                                    // и не тупик
                                    if (DeadLock(nextPoint, nextDirection, newNode, newNode.Move.Field))
                                    {
                                        //отменяем ход и удаляем узел дерева
                                        CancelMove(availableMove[j], currentLevelNodes[i].Move.Field);
                                        nextLevelNodes.Remove(newNode);
                                        _Log.GeneratedNodesCount--;
                                    }
                                }

                            }

                        }

                    }

                     //если была достигнута максимальная вершина дерева
                     if (_Log.MaxDepth >= AllowableDepth)
                     {
                            //алгоритм завершен
                            done = true;
                     }
                        //или если на следующем уровне нету узлов (т.е ни из одного узла дерева текущего уровня нельзя совершить ход)
                     else if (nextLevelNodes.Count == 0)
                     {
                         //алгоритм завершен
                         done = true;
                     }
                     //если еще есть куда ходить
                     else
                     {
                         //переходим на следующий уровень дерева решений
                         currentLevelNodes = nextLevelNodes;
                         nextLevelNodes = new List<SolutionThree>();
                         //если пора оценить вершины
                         localDepth++;
                         if (localDepth == parLocalDepth)
                         {
                             //сортируем по оценочной функции
                             SortTHreeNodesBysEstimator(currentLevelNodes);
                         }
                     }
                    
                }

            }//пока не завершится алгоритм повторяем все вышеизложенное

            return complete;
        }

        private void SortTHreeNodesBysEstimator(List<SolutionThree> parThreeNodes)
        {
             List<SolutionThree> nodes = parThreeNodes;
            List<int> estimatorValues = new List<int>();
            Point currentPoint;
            Orientation direction;

            //для каждого возможного узла 
            for (int i = 0; i < nodes.Count; i++)
            {
                currentPoint = nodes[i].Move.Enter;
                direction = new Orientation(nodes[i].Move.RotatedPipe.Position.X - currentPoint.X, nodes[i].Move.RotatedPipe.Position.Y - currentPoint.Y);
                //получаем его оценку
                int rating = Estimator(nodes[i].Move, currentPoint, direction);
                estimatorValues.Add(rating);
            }

            //сортируем возможные ходы согласно их оценкам (от меньших к большим
            for (int i = 0; i < nodes.Count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (estimatorValues[i] < estimatorValues[j])
                    {
                        int rating = estimatorValues[i];
                        estimatorValues.RemoveAt(i);
                        estimatorValues.Insert(j, rating);

                        SolutionThree node = nodes[i];
                        nodes.RemoveAt(i);
                        nodes.Insert(j, node);
                    }
                }
            }
        }

        /// <summary>
        /// Сортировка возможных ходов исхожя из оценочной функции
        /// </summary>
        /// <param name="parMove">Соверщаемый ход</param>
        /// <param name="parPosition">текущее положение</param>
        /// <param name="parDirection">текущее направление</param>
        /// <returns>рассчтояние от текущего положения через ход до выхода</returns>
        private void SortMovesByEstimator(List<Move> parAvailableMoves, Point parPosition, Orientation parDirection)
        {
            List<Move> availableMoves = parAvailableMoves;
            List<int> estimatorValues = new List<int>();
            Point currentPoint = parPosition;
            Orientation direction = parDirection;

            //для каждого возможного хода 
            for (int i = 0; i < availableMoves.Count; i++)
            {
                //получаем его оценку
                int rating = Estimator(availableMoves[i], currentPoint, direction);
                estimatorValues.Add(rating);
            }

            //сортируем возможные ходы согласно их оценкам (от меньших к большим
            for (int i = 0; i < availableMoves.Count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (estimatorValues[i] < estimatorValues[j])
                    {
                        int rating = estimatorValues[i];
                        estimatorValues.RemoveAt(i);
                        estimatorValues.Insert(j, rating);

                        Move move = availableMoves[i];
                        availableMoves.RemoveAt(i);
                        availableMoves.Insert(j, move);
                    }
                }
            }
        }

        /// <summary>
        /// Оценочная функция
        /// </summary>
        /// <param name="parMove">Соверщаемый ход</param>
        /// <param name="parPosition">текущее положение</param>
        /// <param name="parDirection">текущее направление</param>
        /// <returns>рассчтояние от текущего положения через ход до выхода</returns>
        private int Estimator(Move parMove, Point parPosition, Orientation parDirection)
        {
            Point currentPoint = parPosition;
            Orientation direction = parDirection;
            Move move = parMove;

            //исходя из текущего положения и направления движения вычисляем следующую точку
            Point nextPoint = new Point(move.RotatedPipe.Position.X, move.RotatedPipe.Position.Y);
            //получаем новое направление движение (куда смотрит второй вход следующей трубы)
            Orientation nextDirection = new Orientation();

            if (direction == -move.RotatedPipe.FirstHole)
            {
                nextDirection = move.RotatedPipe.SecondHole.Clone();
            }
            else
            {
                nextDirection = move.RotatedPipe.FirstHole.Clone();
            }

            //исходя из текущего положения и направления движения вычисляем точку через один ход
            Point nextNextPoint = new Point(nextPoint.X + nextDirection.X, nextPoint.Y + nextDirection.Y);

            //возвращем расстояние от текущей точке через один ход до выхода
            return Math.Abs(_Exit.X - nextNextPoint.X) + Math.Abs(_Exit.Y - nextNextPoint.Y);
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
            //если направление задано, то можно искать доступные ходы
            if ((object)parDirection != null)
            {
                //исходя из текущего положения и направления движения вычисляем следующую точку
                Point dstPoint = new Point(parPoint.X + parDirection.X, parPoint.Y + parDirection.Y);

                //если в следующей точке есть труба, то
                if ((object)parField[dstPoint.X, dstPoint.Y] != null)
                {
                    //получаем следующую трубу
                    Pipe originalDstPipe = parField[dstPoint.X, dstPoint.Y].Clone();
                    Pipe dstPipe = originalDstPipe.Clone();

                    for (int i = 0; i < Orientation.OrientationsCount; i++)
                    {
                        //разворачиваем ее первым входом к текущей точке
                        dstPipe.Rotate(true);
                        //добавляем текущею точку и поворнутую получившимся образом трубу в список возможных ходов 
                        moves.Add(new Move(parPoint, dstPipe.Clone(), originalDstPipe.Clone(), GetFieldCopy(parField)));
                    }

                }
            }
            //возвращаем список возможных ходов
            return moves;
        }
      
        /// <summary>
        /// Совершение хода
        /// </summary>
        /// <param name="parMove">Совершаемый ход</param>
        /// <param name="parField">Поле, на котором совершается ход</param>
        private void MakeMove(Move parMove, Pipe[,] parField)
        {
            parField[parMove.RotatedPipe.Position.X, parMove.RotatedPipe.Position.Y] = parMove.RotatedPipe;
        }

        /// <summary>
        /// Отмена хода
        /// </summary>
        /// <param name="parMove">Отменяемый ход</param>
        /// <param name="parField">Поле, на котором отменяется ход</param>
        private void CancelMove(Move parMove, Pipe[,] parField)
        {
            parField[parMove.OriginalPipe.Position.X, parMove.OriginalPipe.Position.Y] = parMove.OriginalPipe;
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
            
            //если наравление отсутствует, значит из предидущей точки в эту трубу войти нельзя
            if ((object)parDirection == null)
            {
                return false;
            }
            else
            {
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
                            //входим в трубу и направлением назначаем ее другой выход
                            direction = GetNextDirection(currentPoint, parField[nextPoint.X, nextPoint.Y]);
                            if ((object)direction != null)
                            {
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
        }

        /// <summary>
        /// Проверка тупика
        /// </summary>
        /// <param name="parPoint">Текущее положение</param>
        /// <param name="parDirection">Текущее направление движения</param>
        /// <param name="parSolutionThree">Текущий езел дерева решений</param>
        /// <returns></returns>
        private bool DeadLock(Point parPoint, Orientation parDirection, SolutionThree parSolutionThree, Pipe[,] parField)
        {
            //метка тупика
            bool deadLock = false;
            
            
            //если направление движение не определено
            if ((object)parDirection == null)
            {
                //занчи в текущую трубу невозможно войти
                //тупик
                deadLock = true;
            }
            else
            {
                //вычисляем следующую из текущей ситуации точку
                Point nextPoint = new Point(parPoint.X + parDirection.X, parPoint.Y + parDirection.Y);

                Point prevPoint = _Enter;
                //если это не корневая вершина дерева
                if (parSolutionThree.Move != null)
                {
                    //получаем предыдущую точку
                    prevPoint = parSolutionThree.Move.Enter;
                }

                //если достигнута максимально допустимая глубина дерева поиска
                if (parSolutionThree.Level >= AllowableDepth)
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
                    if ((nextPoint.X < 1) || (nextPoint.X > _ColCount) ||
                        (nextPoint.Y < 1) || (nextPoint.Y > _RowCount))
                    {
                        //тупик
                        deadLock = true;

                    }
                    else
                    {
                        //если в следующей точке нет трубы
                        if ((object)parField[nextPoint.X, nextPoint.Y] == null)
                        {
                            //тупик
                            deadLock = true;
                        }
                    }
                }
            }

            //возвращаем флаг тупика (если он был найден)
            return deadLock;
        }       
        
        /// <summary>
        /// Получить следующее направление движения
        /// </summary>
        /// <param name="parEnterPoint">точка, из которой входим в трубу</param>
        /// <param name="parPipe">труба, в которую входим</param>
        /// <returns></returns>
        private Orientation GetNextDirection(Point parEnterPoint, Pipe parPipe)
        {
            //получаем начальное направление
            Orientation direction = new Orientation(parPipe.Position.X - parEnterPoint.X, parPipe.Position.Y - parEnterPoint.Y);

            //вычисляем новое направление движения
            //если в трубу в следующей точке можно войти через первый вход,
            if (direction == -parPipe.FirstHole)
            {
                //следующим направлением назначаем направление второго выхода следующей трубы
                direction = parPipe.SecondHole;
            }
            //если в трубу в следующей точке можно войти через второй вход,
            else if (direction == -parPipe.SecondHole)
            {
                //следующим направлением назначаем направление первого выхода следующей трубы
                direction = parPipe.FirstHole;
            }
            //если в трубу вообще нельзя войти
            else
            {
                //то и направления следующего нет
                direction = null;
            }

            //возвращаем следующее направление
            return direction;
        }

        /// <summary>
        /// Получить копию поля
        /// </summary>
        /// <param name="parField"></param>
        /// <returns></returns>
        private Pipe[,] GetFieldCopy(Pipe[,] parField)
        {
            //инициализируем возвращаемую копию поля
            Pipe[,] fieldCopy = new Pipe[_ColCount + 2,_RowCount + 2];
            
            //пробегаем по всем строкам и столбцам
            for (int j = 0; j < _RowCount + 2; j++)
                for (int i = 0; i < _ColCount + 2; i++)
                {
                    //копируем каждый элемент из исходного поля в новое
                    fieldCopy[i, j] = parField[i, j];
                }

            //возвращаем копию поля
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

        /// <summary>
        /// Результат выполнения поиска решения
        /// </summary>
        public SolveResult Log
        {
            get { return _Log; }
        }
    }
}
