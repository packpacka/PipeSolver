using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PipesSolver
{
    /// <summary>
    /// Класс хода
    /// </summary>
    public class Move
    {
        /// <summary>
        /// Труба,выбранная для хода и уже повернутая в нужном направлении
        /// </summary>
        private Pipe _RotatedPipe;
        /// <summary>
        /// Труба,выбранная для хода в исходном состоянии
        /// </summary>
        private Pipe _OriginalPipe;
        /// <summary>
        /// Точка входа
        /// </summary>
        private Point _Enter;
        /// <summary>
        /// Состояние поля на текущем ходе
        /// </summary>
        private Pipe[,] _Field;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="parEnter"></param>
        /// <param name="parRotatedPipe"></param>
        public Move(Point parEnter, Pipe parRotatedPipe, Pipe parOriginalPipe, Pipe[,] parField)
        {
            _RotatedPipe = parRotatedPipe;
            _OriginalPipe = parOriginalPipe;
            _Enter = parEnter;
            _Field = parField;
        }
        /// <summary>
        /// Труба,выбранная для хода и уже повернутая в нужном направлении
        /// </summary>
        public Pipe RotatedPipe
        {
            get { return _RotatedPipe; }
        }
        /// <summary>
        /// Труба,выбранная для хода в исходном состоянии
        /// </summary>
        public Pipe OriginalPipe
        {
            get { return _OriginalPipe; }
        }
        /// <summary>
        /// Точка входа
        /// </summary>
        public Point Enter
        {
            get { return _Enter; }
        }
        /// <summary>
        /// Состояние поля на текущем ходе
        /// </summary>
        public Pipe[,] Field
        {
            get { return _Field; }
        }    
    }
}
