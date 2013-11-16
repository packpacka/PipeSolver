using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace PipesSolver
{
    /// <summary>
    /// Класс главной формы
    /// </summary>
    public partial class FormPipeSolver : Form
    {
        /// <summary>
        /// структура, хранящая  параметры изображения
        /// </summary>
        public struct IconInfo
        {
            public bool fIcon;
            public int xHotspot;
            public int yHotspot;
            public IntPtr hbmMask;
            public IntPtr hbmColor;
        }

        /// <summary>
        /// Множество возможных действий
        /// </summary>
        private enum Actions
        {
            MarkEnter,
            MarkExit,
            AdditionPipe,
            None
        }

        /// <summary>
        /// Ширина ячейки поля
        /// </summary>
        const int FIELD_CELL_WIDTH = 40;
        /// <summary>
        /// Высота ячейки поля
        /// </summary>
        const int FIELD_CELL_HEIGTH = 40;

        /// <summary>
        /// Точка входа
        /// </summary>
        private Point _Enter;
        /// <summary>
        /// Точка выхода
        /// </summary>
        private Point _Exit;
        /// <summary>
        /// Количество колонок поля
        /// </summary>
        private int _ColCount;
        /// <summary>
        /// Количество строк поля
        /// </summary>
        private int _RowCount;
        /// <summary>
        /// Добавляемая на поле труба
        /// </summary>
        private Pipe _AddedPipe;
        
        /// <summary>
        /// Массив труб расположенные на поле
        /// </summary>
        private Pipe[,] _PipesOnField;

        /// <summary>
        /// Текущее действие
        /// </summary>
        private Actions _Action;

        /// <summary>
        /// Конструктор формы
        /// </summary>
        public FormPipeSolver()
        {
            InitializeComponent();

            //инициализируем  точку входа и выхода
            _Enter = _Exit = new Point(0, 0);
            //Инициализируем текущее действие 
            _Action = Actions.None;         
            //Инициализируем поле
            _PipesOnField = new Pipe[0,0];
            //создаем табличку с шаблонами труб
            CreatePipeTemplate();        

        }

        

        /// <summary>
        /// Создание таблицы шаблонов труб
        /// </summary>
        public void CreatePipeTemplate()
        {
            dataGridViewTemplate.RowTemplate.Height = FIELD_CELL_HEIGTH; 
            dataGridViewTemplate.Rows.Add();
            dataGridViewTemplate.Rows[0].Cells[0].Value = Properties.Resources.angle1;
            dataGridViewTemplate.Rows[0].Cells[1].Value = Properties.Resources.angle2;
            dataGridViewTemplate.Rows[0].Cells[2].Value = Properties.Resources.angle3;
            dataGridViewTemplate.Rows[0].Cells[3].Value = Properties.Resources.angle4;
            dataGridViewTemplate.Rows[0].Cells[4].Value = Properties.Resources.direct1;
            dataGridViewTemplate.Rows[0].Cells[5].Value = Properties.Resources.direct2;
    
        }


        //Магия...
        [DllImport("user32.dll")]
        public static extern IntPtr CreateIconIndirect(ref IconInfo icon);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);
        //задание курсора в виде выбранной трубы
        public static System.Windows.Forms.Cursor CreateCursor(Bitmap bmp, int xHotSpot, int yHotSpot)
        {
            IconInfo tmp = new IconInfo();
            GetIconInfo(bmp.GetHicon(), ref tmp);
            tmp.xHotspot = xHotSpot;
            tmp.yHotspot = yHotSpot;
            tmp.fIcon = false;
            return new System.Windows.Forms.Cursor(CreateIconIndirect(ref tmp));
        }

        /// <summary>
        /// Установить курсор по умолчанию
        /// </summary>
        public void SetDefaultCuror()
        {
            Cursor = Cursors.Default;
            dataGridViewField.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Нажатие на кнопку применить
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonApplyDim_Click(object sender, EventArgs e)
        {
            try
            {
                //Получаем введенное количество строк и столбцов поля
                _ColCount = Convert.ToInt32(textBoxColCount.Text);
                _RowCount = Convert.ToInt32(textBoxRowCount.Text);

                //создаем массив труб на поле
                _PipesOnField = new Pipe[_ColCount + 2, _RowCount + 2];
                //если п олях введены значения больше 0
                if ((_ColCount != 0) && (_RowCount != 0))
                {
                    //очищаем поле
                    dataGridViewField.Columns.Clear();
                    //опять же обнуляем точку входа и выхода
                    _Enter = _Exit = new Point(0, 0);
                    //действие  - ничего
                    _Action = Actions.None;
                    //создаем поле
                    CreateTaskField();
                }
                    // если чтото случилось не так, выбрасываем исключение
                else throw new Exception();                
            }
            catch
            {
                //перехватываем исключения здесь
                MessageBox.Show("Введите корректно значения размеров поля");
            }
        }
        /// <summary>
        /// Создание поля задачи
        /// </summary>
        public void CreateTaskField()
        {
                DataGridViewImageColumn col = new DataGridViewImageColumn();
                col.Image = Properties.Resources.border;
                col.Width = FIELD_CELL_WIDTH / 2;
                col.ImageLayout = DataGridViewImageCellLayout.Stretch;
                dataGridViewField.Columns.Add(col);
                for (int i = 0; i < _ColCount; i++)
                {
                    col = new DataGridViewImageColumn();
                    col.Width = FIELD_CELL_WIDTH;
                    col.Image = Properties.Resources.fieldsegment;
                    col.ImageLayout = DataGridViewImageCellLayout.Stretch;
                    dataGridViewField.Columns.Add(col);
                }
                col = new DataGridViewImageColumn();
                col.Width = FIELD_CELL_WIDTH / 2;
                col.Image = Properties.Resources.border;
                col.ImageLayout = DataGridViewImageCellLayout.Stretch;
                dataGridViewField.Columns.Add(col);
            

            dataGridViewField.RowTemplate.Height = FIELD_CELL_HEIGTH / 2;
            dataGridViewField.RowCount += 1;
            dataGridViewField.RowTemplate.Height = FIELD_CELL_HEIGTH;
            dataGridViewField.RowCount += _RowCount;
            dataGridViewField.RowTemplate.Height = FIELD_CELL_HEIGTH / 2;
            dataGridViewField.RowCount += 1;

            for (int i = 0; i <= _ColCount + 1; i++)
            {
                dataGridViewField.Rows[0].Cells[i].Value = Properties.Resources.border;
                dataGridViewField.Rows[_RowCount + 1].Cells[i].Value = Properties.Resources.border;
            }                    
        }

        /// <summary>
        /// Создание поля решения
        /// </summary>
        private void CreateSolveField()
        {
            dataGridViewField.RowCount= _RowCount + 2;
            dataGridViewField.RowTemplate.Height = FIELD_CELL_HEIGTH / 2;
            dataGridViewField.RowCount += 1;
            dataGridViewField.RowTemplate.Height = FIELD_CELL_HEIGTH;
            dataGridViewField.RowCount += _RowCount;
            dataGridViewField.RowTemplate.Height = FIELD_CELL_HEIGTH / 2;
            dataGridViewField.RowCount += 1;

            for (int i = 0; i <= _ColCount; i++)
            {
                dataGridViewField.Rows[_RowCount+2].Cells[i].Value = Properties.Resources.border;
                dataGridViewField.Rows[_RowCount * 2 + 3].Cells[i].Value = Properties.Resources.border;
            }

            dataGridViewField.Rows[_RowCount + 2 + _Enter.Y].Cells[_Enter.X].Value = Properties.Resources.enter;
            dataGridViewField.Rows[_RowCount + 2 + _Exit.Y].Cells[_Exit.X].Value = Properties.Resources.exit;
        }

        /// <summary>
        /// Нажатие кнопки "Отметить вход"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonMarkEnter_Click(object sender, EventArgs e)
        {
            //Действие задаем - указание точки входа
            _Action = Actions.MarkEnter;
            //отключаем кнопу "Отметить вход"
            buttonMarkEnter.Enabled = false;
            //курсор меняем на вид руки
            Cursor = Cursors.Hand;
            dataGridViewField.Cursor = Cursors.Hand;
            //включаем кнопу "Отметить выход"
            buttonMarkExit.Enabled = true;
        }
        /// <summary>
        /// Нажатие на кнопку "Отметить выход"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonMarkExit_Click(object sender, EventArgs e)
        {
            //Действие задаем - указание точки выхода
            _Action = Actions.MarkExit;
            //отключаем кнопу "Отметить выход"
            buttonMarkExit.Enabled = false;
            //курсор меняем на вид руки
            Cursor = Cursors.Hand;
            dataGridViewField.Cursor = Cursors.Hand;
            //включаем кнопу "Отметить вход"
            buttonMarkEnter.Enabled = true; 
        }

        /// <summary>
        /// Нажате по ячейке таблицы шаблонов труб
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewTemplate_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Действие задаем -добавление трубы на поле
            _Action = Actions.AdditionPipe;
            //Создаем новую трубу по образу и подобию той, на которую нажали в табличке шаблонов
            _AddedPipe = Pipe.TemplatePipes[e.ColumnIndex].Clone();
            //видоизменяем курсор...немного магии 
            Bitmap img = new Bitmap(_AddedPipe.Image, new Size(FIELD_CELL_WIDTH, FIELD_CELL_HEIGTH));
            Cursor = CreateCursor(img, 2, 2);
            dataGridViewField.Cursor = CreateCursor(img, 2, 2);

            //на всякий случай активируем кнопки "Отметить вход" и "Отметиь выход"
            buttonMarkEnter.Enabled = true;
            buttonMarkExit.Enabled = true;
        }

        /// <summary>
        /// Нажате на ячейку в поле с трубами
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewField_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //если нажали на бордюр поля
            if (((e.ColumnIndex == 0) || (e.ColumnIndex == dataGridViewField.ColumnCount - 1) ||
                (e.RowIndex == 0) || (e.RowIndex == dataGridViewField.RowCount - 1))&&(e.ColumnIndex != e.RowIndex))
            {
                //если в данный момент действие  - указание выхода,
                if ((_Action == Actions.MarkEnter) && ((_Exit.X != e.ColumnIndex) || (_Exit.Y != e.RowIndex)))
                {
                    //на всякий случай закрашиваем старую точку входа (если она уже была отмечена ранее) в цвет бордюра
                    dataGridViewField.Rows[_Enter.Y].Cells[_Enter.X].Value = Properties.Resources.border;
                    //задаем новую точку входа
                    _Enter = new Point(e.ColumnIndex, e.RowIndex);
                    //и окрашиваем бордюр в нажатой точке цветом точки входа
                    dataGridViewField.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Properties.Resources.enter;
                    //Действие задаем - никакое
                    _Action = Actions.None;
                    //Включаем кнопку "отметить вход"
                    buttonMarkEnter.Enabled = true;
                    //задаем курсор по умолчанию
                    SetDefaultCuror();
                }
                //если дейстиве - указание выхода, то делаем все тоже самое что и со входом
                if ((_Action == Actions.MarkExit) && ((_Enter.X != e.ColumnIndex) || (_Enter.Y != e.RowIndex)))
                {
                    dataGridViewField.Rows[_Exit.Y].Cells[_Exit.X].Value = Properties.Resources.border;
                    _Exit = new Point(e.ColumnIndex, e.RowIndex);
                    dataGridViewField.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Properties.Resources.exit;

                    _Action = Actions.None;
                    buttonMarkExit.Enabled = true;
                    SetDefaultCuror();
                }
            }
            //если мы нажали по ячейке "игровой" области
            else
            {
                try
                {
                    //если текущее действие - добавление трубы, то
                    if (_Action == Actions.AdditionPipe)
                    {
                        //задаем добавляемой трубе положение соответственно тому, по какой ячейке поля мы нажали
                        _AddedPipe.Position = new Point(e.ColumnIndex, e.RowIndex);
                        //задаем картинку в ячейке на поле соответсвующую добавляемой трубе
                        dataGridViewField.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = _AddedPipe.Image;
                        //добавляем новую трубу в массив труб на поле
                        _PipesOnField[e.ColumnIndex, e.RowIndex] = _AddedPipe;
                        //ну и удаляем из памяти добавляемую трубу
                        _AddedPipe = null;
                        //действие задаем - ни какое
                        _Action = Actions.None;
                        //устанавливаем курсор по умолчанию
                        SetDefaultCuror();
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Клик по форме
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            //Если нажата правая кнопки мыши этот момент мы добавляли трубу
            if ((e.Button == System.Windows.Forms.MouseButtons.Right) && (_Action == Actions.AdditionPipe))
            {
                //отменяем действие
                _Action = Actions.None;
                SetDefaultCuror();
            }
        }
        /// <summary>
        /// Клик по таблице
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewField_MouseClick(object sender, MouseEventArgs e)
        {
            //Если нажата правая кнопки мыши этот момент мы добавляли трубу
            if ((e.Button == System.Windows.Forms.MouseButtons.Right) && (_Action == Actions.AdditionPipe))
            {
                _Action = Actions.None;
                Cursor = Cursors.Default;
                dataGridViewField.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Двойной клик по ячейке таблицы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewField_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //Если мы нечего не делали и в клетке, по которой был клик, есть труба,
            if ((_Action == Actions.None)&&((object)_PipesOnField[e.ColumnIndex,e.RowIndex] != null))
            { 
                //удаляем ту трубу
                _PipesOnField[e.ColumnIndex, e.RowIndex] = null;
                dataGridViewField.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Properties.Resources.fieldsegment;
            }
        }

        /// <summary>
        /// Нажатие на кнопку "Поиск в глубину"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDepthSolve_Click(object sender, EventArgs e)
        {
            //получаем копию поля 
            Pipe[,] testField = GetCopeField();
            //создаем задачу с нащим полем
            Brainteaser br = new Brainteaser(testField, _ColCount, _RowCount, _Enter, _Exit);
            //решаем задачу
            br.DepthSolve();
            //выводим ответ
            OutputSolve(br.Log.Field);
            //записываем результат выполнения в файл
            WriteLog(br.Log, "DepthLog.txt");
            //создаем форму сведений о выполнении 
            FormLog formLog = new FormLog();
            formLog.Show();
            //и заносим туда данные
            formLog.FillLogForm("DepthLog.txt");

        }

        private void OutputSolve(Pipe[,] parField)
        {
            CreateSolveField();

            for (int j = 0; j < _RowCount + 2; j++)
            {
               
                for (int i = 0; i < _ColCount + 2; i++)
                {
                    if ((object)parField[i, j] != null)
                    {
                        dataGridViewField.Rows[j + _RowCount + 2].Cells[i].Value = parField[i, j].Image;
                    }
                }
            }
        }

        /// <summary>
        /// Получить копию поля
        /// </summary>
        /// <returns></returns>
        public Pipe[,] GetCopeField()
        {
            Pipe[,] field = new Pipe[_ColCount + 2, _RowCount + 2];
            for (int i = 0; i < _ColCount + 2; i++)
                for (int j = 0; j < _RowCount + 2; j++)
                {
                    if ((object)_PipesOnField[i, j] != null)
                    {
                        field[i, j] = _PipesOnField[i, j].Clone();
                    }
                }

            return field;
        }

        private void WriteLog(Brainteaser.SolveResult parLog, string parPath)
        {
            StreamWriter sw = new StreamWriter(parPath);
            string result = parLog.Complete ? "решение найдено" : "решение ненайдено";
            
            sw.WriteLine("Метод решения - " + parLog.Method);
            sw.WriteLine("Статус выполнения - " + result);
            sw.WriteLine("Время выполнения - " + parLog.ProcessTime.Elapsed);
            sw.WriteLine("Максимальная глубина поиска - " + parLog.MaxDepth);
            sw.WriteLine("Количество сгенерированных узлов - " + parLog.GeneratedNodesCount);
            sw.Close();
        }

        private void buttonDepthLog_Click(object sender, EventArgs e)
        {
            //создаем форму сведений о выполнении 
            FormLog logForm = new FormLog();
            logForm.Show();
            //и заносим туда данные
            logForm.FillLogForm("DepthLog.txt");
        }

        private void buttonBreadthSolve_Click(object sender, EventArgs e)
        {
            //получаем копию поля 
            Pipe[,] testField = GetCopeField();
            //создаем задачу с нащим полем
            Brainteaser br = new Brainteaser(testField, _ColCount, _RowCount, _Enter, _Exit);
            //решаем задачу
            br.BreadthSolve();
            //выводим ответ
            OutputSolve(br.Log.Field);
            //записываем результат выполнения в файл
            WriteLog(br.Log, "BreadthLog.txt");
            //создаем форму сведений о выполнении 
            FormLog formLog = new FormLog();
            formLog.Show();
            //и заносим туда данные
            formLog.FillLogForm("BreadthLog.txt");
        }

        private void buttonBreathLog_Click(object sender, EventArgs e)
        {
            //создаем форму сведений о выполнении 
            FormLog formLog = new FormLog();
            formLog.Show();
            //и заносим туда данные
            formLog.FillLogForm("DepthLog.txt");
        }

    }
}
