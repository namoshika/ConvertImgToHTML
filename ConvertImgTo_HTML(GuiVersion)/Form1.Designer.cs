namespace ConvertImgTo_HTML_GuiVersion_
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.jobList = new System.Windows.Forms.ListView();
            this.jobListGroup = new System.Windows.Forms.GroupBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.addJobButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.outputTypeGroup = new System.Windows.Forms.Panel();
            this.movieModeRadio = new System.Windows.Forms.RadioButton();
            this.charModeRadio = new System.Windows.Forms.RadioButton();
            this.colorModeRadio = new System.Windows.Forms.RadioButton();
            this.savePath = new System.Windows.Forms.TextBox();
            this.targetPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.savePathSelectButton = new System.Windows.Forms.Button();
            this.targetPathSelectButton = new System.Windows.Forms.Button();
            this.OptionGroup = new System.Windows.Forms.GroupBox();
            this.jobListGroup.SuspendLayout();
            this.outputTypeGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // jobList
            // 
            this.jobList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.jobList.Location = new System.Drawing.Point(6, 43);
            this.jobList.Name = "jobList";
            this.jobList.Size = new System.Drawing.Size(566, 128);
            this.jobList.TabIndex = 3;
            this.jobList.UseCompatibleStateImageBehavior = false;
            this.jobList.View = System.Windows.Forms.View.List;
            // 
            // jobListGroup
            // 
            this.jobListGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.jobListGroup.Controls.Add(this.cancelButton);
            this.jobListGroup.Controls.Add(this.jobList);
            this.jobListGroup.Location = new System.Drawing.Point(12, 108);
            this.jobListGroup.Name = "jobListGroup";
            this.jobListGroup.Size = new System.Drawing.Size(580, 177);
            this.jobListGroup.TabIndex = 4;
            this.jobListGroup.TabStop = false;
            this.jobListGroup.Text = "処理リスト";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(499, 14);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(73, 25);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "キャンセル";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // addJobButton
            // 
            this.addJobButton.Location = new System.Drawing.Point(308, 77);
            this.addJobButton.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.addJobButton.Name = "addJobButton";
            this.addJobButton.Size = new System.Drawing.Size(73, 25);
            this.addJobButton.TabIndex = 4;
            this.addJobButton.Text = "追加";
            this.addJobButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 21;
            this.label1.Text = "処理モード:";
            // 
            // outputTypeGroup
            // 
            this.outputTypeGroup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.outputTypeGroup.Controls.Add(this.movieModeRadio);
            this.outputTypeGroup.Controls.Add(this.charModeRadio);
            this.outputTypeGroup.Controls.Add(this.colorModeRadio);
            this.outputTypeGroup.Location = new System.Drawing.Point(69, 54);
            this.outputTypeGroup.Name = "outputTypeGroup";
            this.outputTypeGroup.Size = new System.Drawing.Size(312, 21);
            this.outputTypeGroup.TabIndex = 20;
            // 
            // movieModeRadio
            // 
            this.movieModeRadio.AutoSize = true;
            this.movieModeRadio.Location = new System.Drawing.Point(182, 3);
            this.movieModeRadio.Name = "movieModeRadio";
            this.movieModeRadio.Size = new System.Drawing.Size(80, 16);
            this.movieModeRadio.TabIndex = 0;
            this.movieModeRadio.TabStop = true;
            this.movieModeRadio.Text = "MovieMode";
            this.movieModeRadio.UseVisualStyleBackColor = true;
            // 
            // charModeRadio
            // 
            this.charModeRadio.AutoSize = true;
            this.charModeRadio.Location = new System.Drawing.Point(110, 3);
            this.charModeRadio.Name = "charModeRadio";
            this.charModeRadio.Size = new System.Drawing.Size(66, 16);
            this.charModeRadio.TabIndex = 0;
            this.charModeRadio.TabStop = true;
            this.charModeRadio.Text = "AAMode";
            this.charModeRadio.UseVisualStyleBackColor = true;
            // 
            // colorModeRadio
            // 
            this.colorModeRadio.AutoSize = true;
            this.colorModeRadio.Location = new System.Drawing.Point(3, 3);
            this.colorModeRadio.Name = "colorModeRadio";
            this.colorModeRadio.Size = new System.Drawing.Size(101, 16);
            this.colorModeRadio.TabIndex = 0;
            this.colorModeRadio.TabStop = true;
            this.colorModeRadio.Text = "ColorCharMode";
            this.colorModeRadio.UseVisualStyleBackColor = true;
            // 
            // savePath
            // 
            this.savePath.Location = new System.Drawing.Point(69, 29);
            this.savePath.Name = "savePath";
            this.savePath.Size = new System.Drawing.Size(277, 19);
            this.savePath.TabIndex = 14;
            // 
            // targetPath
            // 
            this.targetPath.Location = new System.Drawing.Point(69, 6);
            this.targetPath.Name = "targetPath";
            this.targetPath.Size = new System.Drawing.Size(277, 19);
            this.targetPath.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 12);
            this.label3.TabIndex = 19;
            this.label3.Text = "保存先:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 12);
            this.label2.TabIndex = 18;
            this.label2.Text = "処理対象:";
            // 
            // savePathSelectButton
            // 
            this.savePathSelectButton.Location = new System.Drawing.Point(352, 28);
            this.savePathSelectButton.Name = "savePathSelectButton";
            this.savePathSelectButton.Size = new System.Drawing.Size(29, 21);
            this.savePathSelectButton.TabIndex = 16;
            this.savePathSelectButton.Text = "...";
            this.savePathSelectButton.UseVisualStyleBackColor = true;
            // 
            // targetPathSelectButton
            // 
            this.targetPathSelectButton.Location = new System.Drawing.Point(352, 4);
            this.targetPathSelectButton.Name = "targetPathSelectButton";
            this.targetPathSelectButton.Size = new System.Drawing.Size(29, 21);
            this.targetPathSelectButton.TabIndex = 17;
            this.targetPathSelectButton.Text = "...";
            this.targetPathSelectButton.UseVisualStyleBackColor = true;
            // 
            // OptionGroup
            // 
            this.OptionGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.OptionGroup.Location = new System.Drawing.Point(387, 4);
            this.OptionGroup.Name = "OptionGroup";
            this.OptionGroup.Size = new System.Drawing.Size(205, 98);
            this.OptionGroup.TabIndex = 22;
            this.OptionGroup.TabStop = false;
            this.OptionGroup.Text = "オプション";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 297);
            this.Controls.Add(this.OptionGroup);
            this.Controls.Add(this.addJobButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.outputTypeGroup);
            this.Controls.Add(this.savePath);
            this.Controls.Add(this.targetPath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.savePathSelectButton);
            this.Controls.Add(this.targetPathSelectButton);
            this.Controls.Add(this.jobListGroup);
            this.Name = "Form1";
            this.Text = "Form1";
            this.jobListGroup.ResumeLayout(false);
            this.outputTypeGroup.ResumeLayout(false);
            this.outputTypeGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView jobList;
        private System.Windows.Forms.GroupBox jobListGroup;
        private System.Windows.Forms.Button addJobButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel outputTypeGroup;
        private System.Windows.Forms.RadioButton movieModeRadio;
        private System.Windows.Forms.RadioButton charModeRadio;
        private System.Windows.Forms.RadioButton colorModeRadio;
        private System.Windows.Forms.TextBox savePath;
        private System.Windows.Forms.TextBox targetPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button savePathSelectButton;
        private System.Windows.Forms.Button targetPathSelectButton;
        private System.Windows.Forms.GroupBox OptionGroup;
    }
}

