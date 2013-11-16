﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PipesSolver
{
    /// <summary>
    /// Класс хода
    /// </summary>
    class Move
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
        /// Конструктор класса
        /// </summary>
        /// <param name="parEnter"></param>
        /// <param name="parRotatedPipe"></param>
        public Move(Point parEnter, Pipe parRotatedPipe, Pipe parOriginalPipe)
        {
            _RotatedPipe = parRotatedPipe;
            _OriginalPipe = parOriginalPipe;
            _Enter = parEnter;
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
    
    }
}