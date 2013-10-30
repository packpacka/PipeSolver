using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;


namespace PipesSolver
{
    /// <summary>
    /// Класс дерева решений
    /// </summary>
    class SolutionThree
    {
        /// <summary>
        /// Дочерние узлы дерева
        /// </summary>
        private List<SolutionThree> _ChildNodes;
        /// <summary>
        /// Родительский узел
        /// </summary>
        private SolutionThree _Root;
        /// <summary>
        /// Уровень узла
        /// </summary>
        private int _Level;
        /// <summary>
        /// Труба,выбранная для хода и уже повернутая в нужном направлении
        /// </summary>
        private Pipe _Pipe;
        /// <summary>
        /// Точка входа
        /// </summary>
        private Point _Enter;
        
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="parRoot">Родиельский узел</param>
        /// <param name="parLevel">Уровень</param>
        public SolutionThree(Pipe parCurrentPipe, SolutionThree parRoot, Point parEnter, int parLevel)
        {
            _Root = parRoot;
            _Level = parLevel;
            _Enter = parEnter;
            _ChildNodes = new List<SolutionThree>(); 
        }

        /// <summary>
        /// Добавить дочерний узел в дерево
        /// </summary>
        /// <param name="parCurrentPipe"></param>
        /// <param name="parEneter"></param>
        public void AddChildNode(Pipe parCurrentPipe, Point parEneter)
        {
            _ChildNodes.Add(new SolutionThree(parCurrentPipe, this, parEneter, _Level++));
        }

        /// <summary>
        /// Удалить дочерний узел с указанным индексом
        /// </summary>
        /// <param name="parChildIndex">Индекс удаляемого дочернего узла</param>
        public void DeleteChildNode(int parChildIndex)
        {
            _ChildNodes.RemoveAt(parChildIndex);
        }

        
        /// <summary>
        /// Дочерние узлы дерева
        /// </summary>
        public List<SolutionThree> ChildNodes
        {
            get { return _ChildNodes; }
        }
        /// <summary>
        /// Родительский узел
        /// </summary>
        private SolutionThree Root
        {
            get { return _Root; }
        }
        /// <summary>
        /// Уровень узла
        /// </summary>
        private int Level
        {
            get { return _Level; }
        }
        /// <summary>
        /// Труба,выбранная для хода и уже повернутая в нужном направлении
        /// </summary>
        private Pipe Pipe
        {
            get { return _Pipe; }
        }
        /// <summary>
        /// Точка входа
        /// </summary>
        private Point Enter
        {
            get { return _Enter; }
        }

    }
}
