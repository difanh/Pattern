namespace Brep
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Button1 = new System.Windows.Forms.Button();
            this.Button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this._tbx_bubblesize = new System.Windows.Forms.TextBox();
            this._tb_extru_dim = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Extrusion = new System.Windows.Forms.CheckBox();
            this.button5 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this._tbx_select_plane = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.pythonInterface = new System.Windows.Forms.Button();
            this.pBar = new System.Windows.Forms.ProgressBar();
            this.lbl_status = new System.Windows.Forms.Label();
            this.TensorClient = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Button1
            // 
            this.Button1.Location = new System.Drawing.Point(11, 9);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(227, 39);
            this.Button1.TabIndex = 5;
            this.Button1.Text = "Curve Evaluator";
            this.Button1.UseVisualStyleBackColor = true;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // Button2
            // 
            this.Button2.Location = new System.Drawing.Point(11, 54);
            this.Button2.Name = "Button2";
            this.Button2.Size = new System.Drawing.Size(227, 39);
            this.Button2.TabIndex = 6;
            this.Button2.Text = "Surface Evaluator";
            this.Button2.UseVisualStyleBackColor = true;
            this.Button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(11, 144);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(106, 39);
            this.button3.TabIndex = 7;
            this.button3.Text = "Write STL";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(11, 99);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(227, 39);
            this.button4.TabIndex = 8;
            this.button4.Text = "Create Pattern";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 310);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Bubble Size";
            // 
            // _tbx_bubblesize
            // 
            this._tbx_bubblesize.Location = new System.Drawing.Point(96, 307);
            this._tbx_bubblesize.Name = "_tbx_bubblesize";
            this._tbx_bubblesize.Size = new System.Drawing.Size(43, 20);
            this._tbx_bubblesize.TabIndex = 10;
            this._tbx_bubblesize.Text = "1";
            // 
            // _tb_extru_dim
            // 
            this._tb_extru_dim.Location = new System.Drawing.Point(96, 335);
            this._tb_extru_dim.Name = "_tb_extru_dim";
            this._tb_extru_dim.Size = new System.Drawing.Size(43, 20);
            this._tb_extru_dim.TabIndex = 12;
            this._tb_extru_dim.Text = "100";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 338);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Feature Dim.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(145, 338);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "mm";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(145, 310);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "mm";
            // 
            // Extrusion
            // 
            this.Extrusion.AutoSize = true;
            this.Extrusion.Location = new System.Drawing.Point(17, 369);
            this.Extrusion.Name = "Extrusion";
            this.Extrusion.Size = new System.Drawing.Size(69, 17);
            this.Extrusion.TabIndex = 15;
            this.Extrusion.Text = "Extrusion";
            this.Extrusion.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(122, 144);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(116, 39);
            this.button5.TabIndex = 16;
            this.button5.Text = "Purge";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(96, 369);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(42, 17);
            this.checkBox1.TabIndex = 17;
            this.checkBox1.Text = "Cut";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // _tbx_select_plane
            // 
            this._tbx_select_plane.Location = new System.Drawing.Point(96, 281);
            this._tbx_select_plane.Name = "_tbx_select_plane";
            this._tbx_select_plane.Size = new System.Drawing.Size(43, 20);
            this._tbx_select_plane.TabIndex = 19;
            this._tbx_select_plane.Text = "3";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 284);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Select Plane";
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(11, 189);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(227, 39);
            this.button6.TabIndex = 20;
            this.button6.Text = "Add Work Plane";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(11, 234);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(227, 39);
            this.button7.TabIndex = 21;
            this.button7.Text = "Anysotropic Pattern";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // pythonInterface
            // 
            this.pythonInterface.Location = new System.Drawing.Point(189, 341);
            this.pythonInterface.Name = "pythonInterface";
            this.pythonInterface.Size = new System.Drawing.Size(49, 45);
            this.pythonInterface.TabIndex = 22;
            this.pythonInterface.Text = "Python";
            this.pythonInterface.UseVisualStyleBackColor = true;
            this.pythonInterface.Click += new System.EventHandler(this.pythonInterface_Click);
            // 
            // pBar
            // 
            this.pBar.Location = new System.Drawing.Point(11, 392);
            this.pBar.MarqueeAnimationSpeed = 1;
            this.pBar.Name = "pBar";
            this.pBar.Size = new System.Drawing.Size(227, 16);
            this.pBar.TabIndex = 23;
            // 
            // lbl_status
            // 
            this.lbl_status.AutoSize = true;
            this.lbl_status.Location = new System.Drawing.Point(8, 413);
            this.lbl_status.Name = "lbl_status";
            this.lbl_status.Size = new System.Drawing.Size(37, 13);
            this.lbl_status.TabIndex = 24;
            this.lbl_status.Text = "Status";
            this.lbl_status.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // TensorClient
            // 
            this.TensorClient.Location = new System.Drawing.Point(189, 281);
            this.TensorClient.Name = "TensorClient";
            this.TensorClient.Size = new System.Drawing.Size(49, 45);
            this.TensorClient.TabIndex = 25;
            this.TensorClient.Text = "TClient";
            this.TensorClient.UseVisualStyleBackColor = true;
            this.TensorClient.Click += new System.EventHandler(this.TensorClient_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(251, 432);
            this.Controls.Add(this.TensorClient);
            this.Controls.Add(this.lbl_status);
            this.Controls.Add(this.pBar);
            this.Controls.Add(this.pythonInterface);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this._tbx_select_plane);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.Extrusion);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this._tb_extru_dim);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._tbx_bubblesize);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.Button2);
            this.Controls.Add(this.Button1);
            this.Name = "Form1";
            this.Text = "Pattern Generation";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button Button1;
        internal System.Windows.Forms.Button Button2;
        internal System.Windows.Forms.Button button3;
        internal System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _tbx_bubblesize;
        private System.Windows.Forms.TextBox _tb_extru_dim;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox Extrusion;
        internal System.Windows.Forms.Button button5;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox _tbx_select_plane;
        private System.Windows.Forms.Label label6;
        internal System.Windows.Forms.Button button6;
        internal System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button pythonInterface;
        private System.Windows.Forms.ProgressBar pBar;
        private System.Windows.Forms.Label lbl_status;
        private System.Windows.Forms.Button TensorClient;
    }
}

