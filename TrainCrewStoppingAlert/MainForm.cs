using System;
using System.Diagnostics;
using System.Drawing.Text;
using System.Windows.Forms;
using TrainCrew;
using XAudio2SoundManager;
using static System.Windows.Forms.AxHost;

namespace TrainCrewStoppingAlert
{
    public partial class MainForm : Form
    {
        private readonly Timer _timer;
        private readonly Sound _sound;
        private readonly AlertManager _alertManager;
        private string _soundAlertPhase;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            // ウィンドウ最前面設定
            this.TopMost = CheckBox_TopMost.Checked;

            // イベント設定
            FormClosing += MainForm_FormClosing;
            CheckBox_TopMost.CheckedChanged += CheckBox_TopMost_CheckedChanged;
            CheckBox_Power.CheckedChanged += CheckBox_Power_CheckedChanged;
            TrackBar_Volume.ValueChanged += TrackBar_Volume_ValueChanged;

            _timer = InitializeTimer(100, Timer_Tick);
            _sound = Sound.Instance;
            _alertManager = new AlertManager();
            _alertManager.UpdatePower(CheckBox_Power.Checked);
            _sound.SetMasterVolume(TrackBar_Volume.Value * 0.1f);
            _soundAlertPhase = "音声再生待機";

            // ComboBoxに音声ファイル名を設定
            foreach (var soundName in _sound.SoundSource.Keys)
            {
                ComboBox_SoundA.Items.Add(soundName);
                ComboBox_SoundB.Items.Add(soundName);
            }
            if (ComboBox_SoundA.Items.Count > 0) ComboBox_SoundA.SelectedIndex = 0;
            if (ComboBox_SoundB.Items.Count > 0) ComboBox_SoundB.SelectedIndex = 0;

            TrainCrewInput.Init();
        }

        /// <summary>
        /// Timer初期化メソッド
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="tickEvent"></param>
        /// <returns></returns>
        private Timer InitializeTimer(int interval, EventHandler tickEvent)
        {
            var timer = new Timer
            {
                Interval = interval
            };
            timer.Tick += tickEvent;
            timer.Start();
            return timer;
        }

        /// <summary>
        /// Timer_Tickイベント (Interval:100ms)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            // TrainCrewから車両データ取得
            var state = TrainCrewInput.GetTrainState();
            // TrainCrewから駅データ取得
            TrainCrewInput.RequestStaData();
            // TrainCrewから信号・操作スイッチデータ取得
            TrainCrewInput.RequestData(DataRequest.Signal | DataRequest.Switch);

            // 各種データが正常に取得できていない場合は処理しない
            if (state == null || state.CarStates.Count == 0 || state.stationList.Count == 0) { return; }
            try { var dataCheck = state.stationList[state.nowStaIndex].Name; }
            catch { return; }

            try
            {
                // 運転画面(運転・ポーズ中)なら処理
                if (TrainCrewInput.gameState.gameScreen == GameScreen.MainGame
                    || TrainCrewInput.gameState.gameScreen == GameScreen.MainGame_Pause)
                {
                    SuspendLayout();

                    // AlertManagerの停車予報更新
                    _alertManager.UpdatePower(CheckBox_Power.Checked);
                    _alertManager.UpdateAlertPhase(state);
                    UpdateAlertPhaseLabel();
                    UpdateAlertSound();

                    ResumeLayout();
                }
                // 運転画面以外なら処理
                else
                {
                    SuspendLayout();

                    // AlertManagerの停車予報初期化
                    _alertManager.InitializeAlertPhase();
                    _soundAlertPhase = "音声再生待機";

                    ResumeLayout();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 停車予報表示更新メソッド
        /// </summary>
        private void UpdateAlertPhaseLabel()
        {
            Label_AlertPhase.Text = _alertManager.AlertPhase;

            switch (_alertManager.AlertPhase)
            {
                case "停P受信待機":
                    Label_AlertPhase.BackColor = System.Drawing.Color.White;
                    break;
                case "停P受信":
                    Label_AlertPhase.BackColor = System.Drawing.Color.SpringGreen;
                    break;
                case "ブレーキ投入":
                    Label_AlertPhase.BackColor = System.Drawing.Color.Aqua;
                    break;
            }
        }

        /// <summary>
        /// 停車予報音声の更新メソッド
        /// </summary>
        private void UpdateAlertSound()
        {
            bool _isVolumeHalf = CheckBox_VolumeHalf.Checked;
            bool _isSoundALoop = RadioButton_SoundA_LoopPlay.Checked;
            bool _isSoundBLoop = RadioButton_SoundB_LoopPlay.Checked;
            bool _isSoundPlayB = CheckBox_SoundPlayB.Checked;

            switch (_soundAlertPhase)
            {
                case "音声再生待機":
                    _sound.SoundAllStop();
                    if (_alertManager.AlertPhase == "停P受信")
                    {
                        _sound.SoundPlay(ComboBox_SoundA.SelectedItem.ToString(), _isSoundALoop, (_isVolumeHalf ? 0.5f : 1.0f));
                        _soundAlertPhase = "停P音声再生";
                    }
                    break;
                case "停P音声再生":
                    if (_alertManager.AlertPhase == "ブレーキ投入")
                    {
                        _soundAlertPhase = "ブレーキ投入";
                    }
                    break;
                case "ブレーキ投入":
                    if (_isSoundPlayB)
                    {
                        _sound.SoundStop(ComboBox_SoundA.SelectedItem.ToString());
                        _sound.SoundPlay(ComboBox_SoundB.SelectedItem.ToString(), _isSoundBLoop, (_isVolumeHalf ? 0.5f : 1.0f));
                        _soundAlertPhase = "停車待機";
                    }
                    else
                    {
                        _sound.SetVolume(ComboBox_SoundA.SelectedItem.ToString(), (_isVolumeHalf ? 0.5f : 1.0f));
                        _soundAlertPhase = "停車待機";
                    }
                    break;
                case "停車待機":
                    if (_alertManager.AlertPhase == "停P受信待機")
                    {
                        _sound.SoundAllStop();
                        _soundAlertPhase = "音声再生待機";
                    }
                    break;
            }
        }

        /// <summary>
        /// MainForm_FormClosingイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            TrainCrewInput.Dispose();
            _sound.Dispose();
        }

        /// <summary>
        /// CheckBox_TopMost_CheckedChangedイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_TopMost_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = CheckBox_TopMost.Checked;
        }

        /// <summary>
        /// 音量バーの値変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrackBar_Volume_ValueChanged(object sender, EventArgs e)
        {
            Label_Volume.Text = $"{TrackBar_Volume.Value * 10}%";
            _sound.SetMasterVolume(TrackBar_Volume.Value * 0.1f);
        }

        /// <summary>
        /// 音声Aテスト再生ボタンクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_SoundA_TestPlay_Click(object sender, EventArgs e)
        {
            string _soundName = ComboBox_SoundA.SelectedItem.ToString();
            try
            {
                _sound.SoundPlay(_soundName, false);
            }
            catch
            {
                MessageBox.Show("音声再生に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 音声Bテスト再生ボタンクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_SoundB_TestPlay_Click(object sender, EventArgs e)
        {
            string _soundName = ComboBox_SoundB.SelectedItem.ToString();
            try
            {
                _sound.SoundPlay(_soundName, false);
            }
            catch
            {
                MessageBox.Show("音声再生に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// CheckBox_Power_CheckedChangedイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Power_CheckedChanged(object sender, EventArgs e)
        {
            _alertManager.UpdatePower(CheckBox_Power.Checked);
        }

        private void CheckBox_VolumeHalf_CheckedChanged(object sender, EventArgs e)
        {
            bool _isVolumeHalf = CheckBox_VolumeHalf.Checked;
            bool _isSoundPlayB = CheckBox_SoundPlayB.Checked;

            if (_isSoundPlayB)
                _sound.SetVolume(ComboBox_SoundB.SelectedItem.ToString(), (_isVolumeHalf ? 0.5f : 1.0f));
            else
                _sound.SetVolume(ComboBox_SoundA.SelectedItem.ToString(), (_isVolumeHalf ? 0.5f : 1.0f));
        }

        private void CheckBox_SoundPlayB_CheckedChanged(object sender, EventArgs e)
        {
            bool _isVolumeHalf = CheckBox_VolumeHalf.Checked;
            bool _isSoundALoop = RadioButton_SoundA_LoopPlay.Checked;
            bool _isSoundBLoop = RadioButton_SoundB_LoopPlay.Checked;
            bool _isSoundPlayB = CheckBox_SoundPlayB.Checked;

            if (_isSoundPlayB)
            {
                _sound.SoundStop(ComboBox_SoundA.SelectedItem.ToString());
                _sound.SoundPlay(ComboBox_SoundB.SelectedItem.ToString(), _isSoundBLoop, (_isVolumeHalf ? 0.5f : 1.0f));
            }
        }
    }
}
