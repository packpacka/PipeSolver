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
        /// Ход
        /// </summary>
        private Move _Move;

        
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="parRoot">Родиельский узел</param>
        /// <param name="parLevel">Уровень</param>
        public SolutionThree(Move parMove, SolutionThree parRoot, int parLevel)
        {
            _Root = parRoot;
            _Level = parLevel;
            _Move = parMove;
            _ChildNodes = new List<SolutionThree>(); 
        }

        /// <summary>
        /// Добавить дочерний узел в дерево
        /// </summary>
        /// <param name="parMove"></param>
        public void AddChildNode(Move parMove)
        {
            _ChildNodes.Add(new SolutionThree(parMove, this, _Level + 1));
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
        public SolutionThree Root
        {
            get { return _Root; }
        }
        /// <summary>
        /// Уровень узла
        /// </summary>
        public int Level
        {
            get { return _Level; }
        }
        /// <summary>
        /// Ход
        /// </summary>
        public Move Move
        {
            get { return _Move; }
        }

    }
}
