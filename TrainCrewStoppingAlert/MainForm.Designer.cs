using System.Windows.Forms;

namespace TrainCrewStoppingAlert
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            CheckBox_TopMost = new CheckBox();
            CheckBox_VolumeHalf = new CheckBox();
            CheckBox_SoundPlayB = new CheckBox();
            GroupBox_SoundA = new GroupBox();
            Label_SoundAVolume = new Label();
            TrackBar_SoundAVolume = new TrackBar();
            Button_SoundA_TestPlay = new Button();
            ComboBox_SoundA = new ComboBox();
            RadioButton_SoundA_LoopPlay = new RadioButton();
            RadioButton_SoundA_SinglePlay = new RadioButton();
            GroupBox_SoundB = new GroupBox();
            Label_SoundBVolume = new Label();
            TrackBar_SoundBVolume = new TrackBar();
            Button_SoundB_TestPlay = new Button();
            RadioButton_SoundB_LoopPlay = new RadioButton();
            RadioButton_SoundB_SinglePlay = new RadioButton();
            ComboBox_SoundB = new ComboBox();
            TrackBar_MasterVolume = new TrackBar();
            GroupBox_Volume = new GroupBox();
            Label_MasterVolume = new Label();
            Label_AlertPhase = new Label();
            CheckBox_Power = new CheckBox();
            GroupBox_SoundA.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)TrackBar_SoundAVolume).BeginInit();
            GroupBox_SoundB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)TrackBar_SoundBVolume).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TrackBar_MasterVolume).BeginInit();
            GroupBox_Volume.SuspendLayout();
            SuspendLayout();
            // 
            // CheckBox_TopMost
            // 
            CheckBox_TopMost.Checked = true;
            CheckBox_TopMost.CheckState = CheckState.Checked;
            CheckBox_TopMost.Location = new System.Drawing.Point(221, 12);
            CheckBox_TopMost.Name = "CheckBox_TopMost";
            CheckBox_TopMost.Size = new System.Drawing.Size(91, 24);
            CheckBox_TopMost.TabIndex = 1;
            CheckBox_TopMost.Text = "最前面表示";
            CheckBox_TopMost.UseVisualStyleBackColor = true;
            // 
            // CheckBox_VolumeHalf
            // 
            CheckBox_VolumeHalf.Checked = true;
            CheckBox_VolumeHalf.CheckState = CheckState.Checked;
            CheckBox_VolumeHalf.Location = new System.Drawing.Point(12, 287);
            CheckBox_VolumeHalf.Name = "CheckBox_VolumeHalf";
            CheckBox_VolumeHalf.Size = new System.Drawing.Size(155, 24);
            CheckBox_VolumeHalf.TabIndex = 2;
            CheckBox_VolumeHalf.Text = "ブレーキ時音量半減";
            CheckBox_VolumeHalf.UseVisualStyleBackColor = true;
            CheckBox_VolumeHalf.CheckedChanged += CheckBox_VolumeHalf_CheckedChanged;
            // 
            // CheckBox_SoundPlayB
            // 
            CheckBox_SoundPlayB.Location = new System.Drawing.Point(12, 317);
            CheckBox_SoundPlayB.Name = "CheckBox_SoundPlayB";
            CheckBox_SoundPlayB.Size = new System.Drawing.Size(155, 24);
            CheckBox_SoundPlayB.TabIndex = 3;
            CheckBox_SoundPlayB.Text = "ブレーキ時音声Bを再生";
            CheckBox_SoundPlayB.UseVisualStyleBackColor = true;
            CheckBox_SoundPlayB.CheckedChanged += CheckBox_SoundPlayB_CheckedChanged;
            // 
            // GroupBox_SoundA
            // 
            GroupBox_SoundA.Controls.Add(Label_SoundAVolume);
            GroupBox_SoundA.Controls.Add(TrackBar_SoundAVolume);
            GroupBox_SoundA.Controls.Add(Button_SoundA_TestPlay);
            GroupBox_SoundA.Controls.Add(ComboBox_SoundA);
            GroupBox_SoundA.Controls.Add(RadioButton_SoundA_LoopPlay);
            GroupBox_SoundA.Controls.Add(RadioButton_SoundA_SinglePlay);
            GroupBox_SoundA.Location = new System.Drawing.Point(12, 85);
            GroupBox_SoundA.Name = "GroupBox_SoundA";
            GroupBox_SoundA.Size = new System.Drawing.Size(300, 80);
            GroupBox_SoundA.TabIndex = 4;
            GroupBox_SoundA.TabStop = false;
            GroupBox_SoundA.Text = "音声A";
            // 
            // Label_SoundAVolume
            // 
            Label_SoundAVolume.Location = new System.Drawing.Point(167, 51);
            Label_SoundAVolume.Name = "Label_SoundAVolume";
            Label_SoundAVolume.Size = new System.Drawing.Size(81, 22);
            Label_SoundAVolume.TabIndex = 12;
            Label_SoundAVolume.Text = "100%";
            Label_SoundAVolume.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TrackBar_SoundAVolume
            // 
            TrackBar_SoundAVolume.AutoSize = false;
            TrackBar_SoundAVolume.LargeChange = 1;
            TrackBar_SoundAVolume.Location = new System.Drawing.Point(167, 22);
            TrackBar_SoundAVolume.Name = "TrackBar_SoundAVolume";
            TrackBar_SoundAVolume.Size = new System.Drawing.Size(81, 34);
            TrackBar_SoundAVolume.TabIndex = 11;
            TrackBar_SoundAVolume.Value = 10;
            TrackBar_SoundAVolume.ValueChanged += TrackBar_SoundAVolume_ValueChanged;
            // 
            // Button_SoundA_TestPlay
            // 
            Button_SoundA_TestPlay.BackColor = System.Drawing.Color.SpringGreen;
            Button_SoundA_TestPlay.ForeColor = System.Drawing.Color.Black;
            Button_SoundA_TestPlay.Location = new System.Drawing.Point(254, 16);
            Button_SoundA_TestPlay.Name = "Button_SoundA_TestPlay";
            Button_SoundA_TestPlay.Size = new System.Drawing.Size(40, 32);
            Button_SoundA_TestPlay.TabIndex = 10;
            Button_SoundA_TestPlay.Text = "再生";
            Button_SoundA_TestPlay.UseVisualStyleBackColor = false;
            Button_SoundA_TestPlay.Click += Button_SoundA_TestPlay_Click;
            // 
            // ComboBox_SoundA
            // 
            ComboBox_SoundA.FormattingEnabled = true;
            ComboBox_SoundA.Location = new System.Drawing.Point(6, 22);
            ComboBox_SoundA.Name = "ComboBox_SoundA";
            ComboBox_SoundA.Size = new System.Drawing.Size(155, 23);
            ComboBox_SoundA.TabIndex = 9;
            // 
            // RadioButton_SoundA_LoopPlay
            // 
            RadioButton_SoundA_LoopPlay.AutoSize = true;
            RadioButton_SoundA_LoopPlay.Checked = true;
            RadioButton_SoundA_LoopPlay.Location = new System.Drawing.Point(85, 51);
            RadioButton_SoundA_LoopPlay.Name = "RadioButton_SoundA_LoopPlay";
            RadioButton_SoundA_LoopPlay.Size = new System.Drawing.Size(76, 19);
            RadioButton_SoundA_LoopPlay.TabIndex = 7;
            RadioButton_SoundA_LoopPlay.TabStop = true;
            RadioButton_SoundA_LoopPlay.Text = "ループ再生";
            RadioButton_SoundA_LoopPlay.UseVisualStyleBackColor = true;
            // 
            // RadioButton_SoundA_SinglePlay
            // 
            RadioButton_SoundA_SinglePlay.AutoSize = true;
            RadioButton_SoundA_SinglePlay.Location = new System.Drawing.Point(6, 51);
            RadioButton_SoundA_SinglePlay.Name = "RadioButton_SoundA_SinglePlay";
            RadioButton_SoundA_SinglePlay.Size = new System.Drawing.Size(73, 19);
            RadioButton_SoundA_SinglePlay.TabIndex = 6;
            RadioButton_SoundA_SinglePlay.Text = "単発再生";
            RadioButton_SoundA_SinglePlay.UseVisualStyleBackColor = true;
            // 
            // GroupBox_SoundB
            // 
            GroupBox_SoundB.Controls.Add(Label_SoundBVolume);
            GroupBox_SoundB.Controls.Add(TrackBar_SoundBVolume);
            GroupBox_SoundB.Controls.Add(Button_SoundB_TestPlay);
            GroupBox_SoundB.Controls.Add(RadioButton_SoundB_LoopPlay);
            GroupBox_SoundB.Controls.Add(RadioButton_SoundB_SinglePlay);
            GroupBox_SoundB.Controls.Add(ComboBox_SoundB);
            GroupBox_SoundB.Location = new System.Drawing.Point(12, 171);
            GroupBox_SoundB.Name = "GroupBox_SoundB";
            GroupBox_SoundB.Size = new System.Drawing.Size(300, 80);
            GroupBox_SoundB.TabIndex = 5;
            GroupBox_SoundB.TabStop = false;
            GroupBox_SoundB.Text = "音声B";
            // 
            // Label_SoundBVolume
            // 
            Label_SoundBVolume.Location = new System.Drawing.Point(167, 51);
            Label_SoundBVolume.Name = "Label_SoundBVolume";
            Label_SoundBVolume.Size = new System.Drawing.Size(81, 22);
            Label_SoundBVolume.TabIndex = 15;
            Label_SoundBVolume.Text = "100%";
            Label_SoundBVolume.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TrackBar_SoundBVolume
            // 
            TrackBar_SoundBVolume.AutoSize = false;
            TrackBar_SoundBVolume.LargeChange = 1;
            TrackBar_SoundBVolume.Location = new System.Drawing.Point(167, 22);
            TrackBar_SoundBVolume.Name = "TrackBar_SoundBVolume";
            TrackBar_SoundBVolume.Size = new System.Drawing.Size(81, 34);
            TrackBar_SoundBVolume.TabIndex = 14;
            TrackBar_SoundBVolume.Value = 10;
            TrackBar_SoundBVolume.ValueChanged += TrackBar_SoundBVolume_ValueChanged;
            // 
            // Button_SoundB_TestPlay
            // 
            Button_SoundB_TestPlay.BackColor = System.Drawing.Color.SpringGreen;
            Button_SoundB_TestPlay.ForeColor = System.Drawing.Color.Black;
            Button_SoundB_TestPlay.Location = new System.Drawing.Point(254, 16);
            Button_SoundB_TestPlay.Name = "Button_SoundB_TestPlay";
            Button_SoundB_TestPlay.Size = new System.Drawing.Size(40, 32);
            Button_SoundB_TestPlay.TabIndex = 13;
            Button_SoundB_TestPlay.Text = "再生";
            Button_SoundB_TestPlay.UseVisualStyleBackColor = false;
            Button_SoundB_TestPlay.Click += Button_SoundB_TestPlay_Click;
            // 
            // RadioButton_SoundB_LoopPlay
            // 
            RadioButton_SoundB_LoopPlay.AutoSize = true;
            RadioButton_SoundB_LoopPlay.Checked = true;
            RadioButton_SoundB_LoopPlay.Location = new System.Drawing.Point(85, 51);
            RadioButton_SoundB_LoopPlay.Name = "RadioButton_SoundB_LoopPlay";
            RadioButton_SoundB_LoopPlay.Size = new System.Drawing.Size(76, 19);
            RadioButton_SoundB_LoopPlay.TabIndex = 12;
            RadioButton_SoundB_LoopPlay.TabStop = true;
            RadioButton_SoundB_LoopPlay.Text = "ループ再生";
            RadioButton_SoundB_LoopPlay.UseVisualStyleBackColor = true;
            // 
            // RadioButton_SoundB_SinglePlay
            // 
            RadioButton_SoundB_SinglePlay.AutoSize = true;
            RadioButton_SoundB_SinglePlay.Location = new System.Drawing.Point(6, 51);
            RadioButton_SoundB_SinglePlay.Name = "RadioButton_SoundB_SinglePlay";
            RadioButton_SoundB_SinglePlay.Size = new System.Drawing.Size(73, 19);
            RadioButton_SoundB_SinglePlay.TabIndex = 11;
            RadioButton_SoundB_SinglePlay.Text = "単発再生";
            RadioButton_SoundB_SinglePlay.UseVisualStyleBackColor = true;
            // 
            // ComboBox_SoundB
            // 
            ComboBox_SoundB.FormattingEnabled = true;
            ComboBox_SoundB.Location = new System.Drawing.Point(6, 22);
            ComboBox_SoundB.Name = "ComboBox_SoundB";
            ComboBox_SoundB.Size = new System.Drawing.Size(155, 23);
            ComboBox_SoundB.TabIndex = 10;
            // 
            // TrackBar_MasterVolume
            // 
            TrackBar_MasterVolume.AutoSize = false;
            TrackBar_MasterVolume.LargeChange = 1;
            TrackBar_MasterVolume.Location = new System.Drawing.Point(6, 21);
            TrackBar_MasterVolume.Name = "TrackBar_MasterVolume";
            TrackBar_MasterVolume.Size = new System.Drawing.Size(191, 34);
            TrackBar_MasterVolume.TabIndex = 8;
            TrackBar_MasterVolume.Value = 10;
            TrackBar_MasterVolume.ValueChanged += TrackBar_Volume_ValueChanged;
            // 
            // GroupBox_Volume
            // 
            GroupBox_Volume.Controls.Add(Label_MasterVolume);
            GroupBox_Volume.Controls.Add(TrackBar_MasterVolume);
            GroupBox_Volume.Location = new System.Drawing.Point(12, 12);
            GroupBox_Volume.Name = "GroupBox_Volume";
            GroupBox_Volume.Size = new System.Drawing.Size(203, 62);
            GroupBox_Volume.TabIndex = 9;
            GroupBox_Volume.TabStop = false;
            GroupBox_Volume.Text = "全体音量";
            // 
            // Label_MasterVolume
            // 
            Label_MasterVolume.Location = new System.Drawing.Point(63, -4);
            Label_MasterVolume.Name = "Label_MasterVolume";
            Label_MasterVolume.Size = new System.Drawing.Size(37, 22);
            Label_MasterVolume.TabIndex = 10;
            Label_MasterVolume.Text = "100%";
            Label_MasterVolume.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Label_AlertPhase
            // 
            Label_AlertPhase.BackColor = System.Drawing.Color.White;
            Label_AlertPhase.BorderStyle = BorderStyle.FixedSingle;
            Label_AlertPhase.Location = new System.Drawing.Point(220, 39);
            Label_AlertPhase.Name = "Label_AlertPhase";
            Label_AlertPhase.Size = new System.Drawing.Size(92, 35);
            Label_AlertPhase.TabIndex = 10;
            Label_AlertPhase.Text = "停P受信待機";
            Label_AlertPhase.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CheckBox_Power
            // 
            CheckBox_Power.Checked = true;
            CheckBox_Power.CheckState = CheckState.Checked;
            CheckBox_Power.Location = new System.Drawing.Point(12, 257);
            CheckBox_Power.Name = "CheckBox_Power";
            CheckBox_Power.Size = new System.Drawing.Size(155, 24);
            CheckBox_Power.TabIndex = 11;
            CheckBox_Power.Text = "停車予報装置電源投入";
            CheckBox_Power.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.Control;
            ClientSize = new System.Drawing.Size(324, 351);
            Controls.Add(CheckBox_Power);
            Controls.Add(Label_AlertPhase);
            Controls.Add(GroupBox_Volume);
            Controls.Add(GroupBox_SoundB);
            Controls.Add(GroupBox_SoundA);
            Controls.Add(CheckBox_SoundPlayB);
            Controls.Add(CheckBox_VolumeHalf);
            Controls.Add(CheckBox_TopMost);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "MainForm";
            Text = "TrainCrewStoppingAlert";
            GroupBox_SoundA.ResumeLayout(false);
            GroupBox_SoundA.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)TrackBar_SoundAVolume).EndInit();
            GroupBox_SoundB.ResumeLayout(false);
            GroupBox_SoundB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)TrackBar_SoundBVolume).EndInit();
            ((System.ComponentModel.ISupportInitialize)TrackBar_MasterVolume).EndInit();
            GroupBox_Volume.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private CheckBox CheckBox_TopMost;
        private CheckBox CheckBox_VolumeHalf;
        private CheckBox CheckBox_SoundPlayB;
        private GroupBox GroupBox_SoundA;
        private GroupBox GroupBox_SoundB;
        private RadioButton RadioButton_SoundA_SinglePlay;
        private RadioButton RadioButton_SoundA_LoopPlay;
        private TrackBar TrackBar_MasterVolume;
        private ComboBox ComboBox_SoundA;
        private ComboBox ComboBox_SoundB;
        private RadioButton RadioButton_SoundB_LoopPlay;
        private RadioButton RadioButton_SoundB_SinglePlay;
        private GroupBox GroupBox_Volume;
        private Label Label_MasterVolume;
        private Button Button_SoundA_TestPlay;
        private Button Button_SoundB_TestPlay;
        private Label Label_AlertPhase;
        private CheckBox CheckBox_Power;
        private TrackBar TrackBar_SoundAVolume;
        private Label Label_SoundAVolume;
        private Label Label_SoundBVolume;
        private TrackBar TrackBar_SoundBVolume;
    }
}
