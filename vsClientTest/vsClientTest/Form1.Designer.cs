
namespace vsClientTest
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.SourcePath = new System.Windows.Forms.TextBox();
            this.ConnectVS = new System.Windows.Forms.Button();
            this.SourceLineNumber = new System.Windows.Forms.NumericUpDown();
            this.SourceColumnNumber = new System.Windows.Forms.NumericUpDown();
            this.CommandType = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.SourceLineNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SourceColumnNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // SourcePath
            // 
            this.SourcePath.Location = new System.Drawing.Point(53, 73);
            this.SourcePath.Name = "SourcePath";
            this.SourcePath.Size = new System.Drawing.Size(276, 22);
            this.SourcePath.TabIndex = 0;
            this.SourcePath.Text = "C:\\Users\\takaoka\\source\\repos\\TaskDataCreator\\TaskDataCreator\\FANZA.cs";
            // 
            // ConnectVS
            // 
            this.ConnectVS.Location = new System.Drawing.Point(345, 116);
            this.ConnectVS.Name = "ConnectVS";
            this.ConnectVS.Size = new System.Drawing.Size(125, 23);
            this.ConnectVS.TabIndex = 1;
            this.ConnectVS.Text = "コマンド送信";
            this.ConnectVS.UseVisualStyleBackColor = true;
            this.ConnectVS.Click += new System.EventHandler(this.ConnectVS_Click);
            // 
            // SourceLineNumber
            // 
            this.SourceLineNumber.Location = new System.Drawing.Point(350, 74);
            this.SourceLineNumber.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.SourceLineNumber.Name = "SourceLineNumber";
            this.SourceLineNumber.Size = new System.Drawing.Size(120, 22);
            this.SourceLineNumber.TabIndex = 3;
            this.SourceLineNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.SourceLineNumber.Value = new decimal(new int[] {
            51,
            0,
            0,
            0});
            // 
            // SourceColumnNumber
            // 
            this.SourceColumnNumber.Location = new System.Drawing.Point(486, 74);
            this.SourceColumnNumber.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.SourceColumnNumber.Name = "SourceColumnNumber";
            this.SourceColumnNumber.Size = new System.Drawing.Size(120, 22);
            this.SourceColumnNumber.TabIndex = 4;
            this.SourceColumnNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.SourceColumnNumber.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // CommandType
            // 
            this.CommandType.FormattingEnabled = true;
            this.CommandType.Items.AddRange(new object[] {
            "",
            "OpenDocument",
            "CenterLines",
            "SetCaretPos",
            "SetSelection"});
            this.CommandType.Location = new System.Drawing.Point(53, 30);
            this.CommandType.Name = "CommandType";
            this.CommandType.Size = new System.Drawing.Size(121, 23);
            this.CommandType.TabIndex = 5;
            this.CommandType.Text = "OpenDocument";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.CommandType);
            this.Controls.Add(this.SourceColumnNumber);
            this.Controls.Add(this.SourceLineNumber);
            this.Controls.Add(this.ConnectVS);
            this.Controls.Add(this.SourcePath);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.SourceLineNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SourceColumnNumber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox SourcePath;
        private System.Windows.Forms.Button ConnectVS;
        private System.Windows.Forms.NumericUpDown SourceLineNumber;
        private System.Windows.Forms.NumericUpDown SourceColumnNumber;
        private System.Windows.Forms.ComboBox CommandType;
    }
}

