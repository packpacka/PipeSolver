namespace PipesSolver
{
    partial class FormPipeSolver
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonApplyDim = new System.Windows.Forms.Button();
            this.textBoxColCount = new System.Windows.Forms.TextBox();
            this.textBoxRowCount = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridViewField = new System.Windows.Forms.DataGridView();
            this.buttonMarkEnter = new System.Windows.Forms.Button();
            this.buttonMarkExit = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridViewTemplate = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewImageColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewImageColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewImageColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewImageColumn();
            this.buttonDepthSolve = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewField)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonApplyDim
            // 
            this.buttonApplyDim.Location = new System.Drawing.Point(162, 36);
            this.buttonApplyDim.Name = "buttonApplyDim";
            this.buttonApplyDim.Size = new System.Drawing.Size(119, 23);
            this.buttonApplyDim.TabIndex = 0;
            this.buttonApplyDim.Text = "Применить";
            this.buttonApplyDim.UseVisualStyleBackColor = true;
            this.buttonApplyDim.Click += new System.EventHandler(this.buttonApplyDim_Click);
            // 
            // textBoxColCount
            // 
            this.textBoxColCount.Location = new System.Drawing.Point(19, 36);
            this.textBoxColCount.Name = "textBoxColCount";
            this.textBoxColCount.Size = new System.Drawing.Size(41, 22);
            this.textBoxColCount.TabIndex = 2;
            this.textBoxColCount.Text = "3";
            // 
            // textBoxRowCount
            // 
            this.textBoxRowCount.Location = new System.Drawing.Point(97, 36);
            this.textBoxRowCount.Name = "textBoxRowCount";
            this.textBoxRowCount.Size = new System.Drawing.Size(41, 22);
            this.textBoxRowCount.TabIndex = 3;
            this.textBoxRowCount.Text = "3";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(70, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "X";
            // 
            // dataGridViewField
            // 
            this.dataGridViewField.AllowUserToAddRows = false;
            this.dataGridViewField.AllowUserToDeleteRows = false;
            this.dataGridViewField.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewField.Location = new System.Drawing.Point(321, 12);
            this.dataGridViewField.Name = "dataGridViewField";
            this.dataGridViewField.ReadOnly = true;
            this.dataGridViewField.RowTemplate.Height = 24;
            this.dataGridViewField.Size = new System.Drawing.Size(623, 550);
            this.dataGridViewField.TabIndex = 6;
            this.dataGridViewField.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewField_CellContentClick);
            this.dataGridViewField.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewField_CellContentDoubleClick);
            this.dataGridViewField.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dataGridViewField_MouseClick);
            // 
            // buttonMarkEnter
            // 
            this.buttonMarkEnter.Location = new System.Drawing.Point(12, 96);
            this.buttonMarkEnter.Name = "buttonMarkEnter";
            this.buttonMarkEnter.Size = new System.Drawing.Size(156, 23);
            this.buttonMarkEnter.TabIndex = 7;
            this.buttonMarkEnter.Text = "Отметить вход";
            this.buttonMarkEnter.UseVisualStyleBackColor = true;
            this.buttonMarkEnter.Click += new System.EventHandler(this.buttonMarkEnter_Click);
            // 
            // buttonMarkExit
            // 
            this.buttonMarkExit.Location = new System.Drawing.Point(174, 96);
            this.buttonMarkExit.Name = "buttonMarkExit";
            this.buttonMarkExit.Size = new System.Drawing.Size(141, 23);
            this.buttonMarkExit.TabIndex = 8;
            this.buttonMarkExit.Text = "Отметить выход";
            this.buttonMarkExit.UseVisualStyleBackColor = true;
            this.buttonMarkExit.Click += new System.EventHandler(this.buttonMarkExit_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxColCount);
            this.groupBox1.Controls.Add(this.buttonApplyDim);
            this.groupBox1.Controls.Add(this.textBoxRowCount);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(303, 77);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Размерность";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGridViewTemplate);
            this.groupBox2.Location = new System.Drawing.Point(12, 191);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(303, 91);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Трубы";
            // 
            // dataGridViewTemplate
            // 
            this.dataGridViewTemplate.AllowUserToAddRows = false;
            this.dataGridViewTemplate.AllowUserToDeleteRows = false;
            this.dataGridViewTemplate.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.dataGridViewTemplate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTemplate.ColumnHeadersVisible = false;
            this.dataGridViewTemplate.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6});
            this.dataGridViewTemplate.EnableHeadersVisualStyles = false;
            this.dataGridViewTemplate.Location = new System.Drawing.Point(37, 30);
            this.dataGridViewTemplate.Name = "dataGridViewTemplate";
            this.dataGridViewTemplate.ReadOnly = true;
            this.dataGridViewTemplate.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataGridViewTemplate.RowHeadersVisible = false;
            this.dataGridViewTemplate.RowTemplate.Height = 24;
            this.dataGridViewTemplate.Size = new System.Drawing.Size(244, 43);
            this.dataGridViewTemplate.TabIndex = 0;
            this.dataGridViewTemplate.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTemplate_CellContentClick);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Column1";
            this.Column1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column1.Width = 40;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Column2";
            this.Column2.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column2.Width = 40;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Column3";
            this.Column3.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column3.Width = 40;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Column4";
            this.Column4.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column4.Width = 40;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Column5";
            this.Column5.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column5.Width = 40;
            // 
            // Column6
            // 
            this.Column6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column6.HeaderText = "Column6";
            this.Column6.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // buttonDepthSolve
            // 
            this.buttonDepthSolve.Location = new System.Drawing.Point(12, 343);
            this.buttonDepthSolve.Name = "buttonDepthSolve";
            this.buttonDepthSolve.Size = new System.Drawing.Size(156, 23);
            this.buttonDepthSolve.TabIndex = 11;
            this.buttonDepthSolve.Text = "Поиск в глубину";
            this.buttonDepthSolve.UseVisualStyleBackColor = true;
            this.buttonDepthSolve.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 395);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(156, 23);
            this.button2.TabIndex = 12;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 449);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(156, 23);
            this.button3.TabIndex = 13;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(12, 507);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(156, 23);
            this.button4.TabIndex = 14;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // FormPipeSolver
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(956, 574);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.buttonDepthSolve);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonMarkExit);
            this.Controls.Add(this.buttonMarkEnter);
            this.Controls.Add(this.dataGridViewField);
            this.Name = "FormPipeSolver";
            this.Text = "Решатель труб";
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewField)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTemplate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonApplyDim;
        private System.Windows.Forms.TextBox textBoxColCount;
        private System.Windows.Forms.TextBox textBoxRowCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridViewField;
        private System.Windows.Forms.Button buttonMarkEnter;
        private System.Windows.Forms.Button buttonMarkExit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dataGridViewTemplate;
        private System.Windows.Forms.DataGridViewImageColumn Column1;
        private System.Windows.Forms.DataGridViewImageColumn Column2;
        private System.Windows.Forms.DataGridViewImageColumn Column3;
        private System.Windows.Forms.DataGridViewImageColumn Column4;
        private System.Windows.Forms.DataGridViewImageColumn Column5;
        private System.Windows.Forms.DataGridViewImageColumn Column6;
        private System.Windows.Forms.Button buttonDepthSolve;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}

