using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PipesSolver
{
    /// <summary>
    /// Класс формы лога
    /// </summary>
    public partial class FormLog : Form
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        public FormLog()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Заполнение формы лога
        /// </summary>
        /// <param name="parLogPath">Путь к файлу лога</param>
        public void FillLogForm(string parLogPath)
        {
            try
            {
                StreamReader sr = new StreamReader(parLogPath);
                while (!sr.EndOfStream)
                {
                    listBoxLog.Items.Add(sr.ReadLine());
                }
                sr.Close();
            }
            catch
            {

            }
           
        }
    }
}
