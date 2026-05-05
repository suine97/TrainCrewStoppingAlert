using System;
using System.Diagnostics;
using System.Windows.Forms;
using TrainCrew;
using XAudio2SoundManager;

namespace TrainCrewStoppingAlert
{
    public partial class MainForm : Form
    {
        private readonly Timer _timer;
        private readonly Sound _sound;
        private readonly AlertManager _alertManager;
        private string _soundAlertPhase;
        private bool _isPower;
        private bool _isReset;

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
            TrackBar_MasterVolume.ValueChanged += TrackBar_Volume_ValueChanged;

            // インスタンス化
            _sound = Sound.Instance;
            _alertManager = new AlertManager();

            // TrainCrewInput初期化
            TrainCrewInput.Init();

            // Timer初期化
            _timer = InitializeTimer(100, Timer_Tick);

            // 初期設定
            _sound.SetMasterVolume(TrackBar_MasterVolume.Value * 0.1f);
            _soundAlertPhase = "音声再生待機";
            _isPower = CheckBox_Power.Checked;
            _isReset = false;

            // ComboBoxに音声ファイル名を設定
            foreach (var soundName in _sound.SoundSource.Keys)
            {
                ComboBox_SoundA.Items.Add(soundName);
                ComboBox_SoundB.Items.Add(soundName);
            }
            if (ComboBox_SoundA.Items.Count > 0) ComboBox_SoundA.SelectedIndex = 0;
            if (ComboBox_SoundB.Items.Count > 0) ComboBox_SoundB.SelectedIndex = 0;

            // 設定ファイル読み込み
            InportSettingFile();
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

            // 電源投入状態の更新
            _isPower = CheckBox_Power.Checked;

            try
            {
                // 運転画面(運転・ポーズ中)なら処理
                if (TrainCrewInput.gameState.gameScreen == GameScreen.MainGame
                    || TrainCrewInput.gameState.gameScreen == GameScreen.MainGame_Pause)
                {
                    SuspendLayout();

                    // 電源未投入なら初期化して終了
                    if (!_isPower)
                    {
                        if (!_isReset)
                        {
                            _alertManager.InitializeAlertPhase();
                            _soundAlertPhase = "音声再生待機";
                            _sound.SoundAllStop();
                            _isReset = true;
                        }
                        return;
                    }
                    else if (_isPower && _isReset)
                    {
                        _isReset = false;
                    }

                    // AlertManagerの停車予報更新
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
                    if (!_isReset)
                    {
                        _alertManager.InitializeAlertPhase();
                        _soundAlertPhase = "音声再生待機";
                        _sound.SoundAllStop();
                        _isReset = true;
                    }

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
            float _soundAVolume = TrackBar_SoundAVolume.Value * 0.1f;
            float _soundBVolume = TrackBar_SoundBVolume.Value * 0.1f;

            switch (_soundAlertPhase)
            {
                case "音声再生待機":
                    if (_alertManager.AlertPhase == "停P受信")
                    {
                        System.Threading.Thread.Sleep(100);
                        _sound.SoundPlay(ComboBox_SoundA.SelectedItem.ToString(), _isSoundALoop, _soundAVolume);
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
                        System.Threading.Thread.Sleep(100);
                        _sound.SoundPlay(ComboBox_SoundB.SelectedItem.ToString(), _isSoundBLoop, _soundBVolume * (_isVolumeHalf ? 0.5f : 1.0f));
                        _soundAlertPhase = "停車待機";
                    }
                    else
                    {
                        _sound.SetVolume(ComboBox_SoundA.SelectedItem.ToString(), _soundAVolume * (_isVolumeHalf ? 0.5f : 1.0f));
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
        /// UI設定をSetting.iniから読み込むメソッド
        /// </summary>
        private void InportSettingFile()
        {
            string settingFilePath = ".\\Setting.ini";

            try
            {
                if (!System.IO.File.Exists(settingFilePath))
                {
                    return;
                }

                var lines = System.IO.File.ReadAllLines(settingFilePath);
                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("[") || line.StartsWith(";"))
                    {
                        continue;
                    }

                    var parts = line.Split('=');
                    if (parts.Length != 2)
                    {
                        continue;
                    }

                    var key = parts[0].Trim();
                    var value = parts[1].Trim();

                    switch (key)
                    {
                        case "TopMost":
                            if (bool.TryParse(value, out bool topMost))
                            {
                                CheckBox_TopMost.Checked = topMost;
                            }
                            break;
                        case "MasterVolume":
                            if (int.TryParse(value, out int masterVolume))
                            {
                                TrackBar_MasterVolume.Value = Math.Clamp(masterVolume, TrackBar_MasterVolume.Minimum, TrackBar_MasterVolume.Maximum);
                            }
                            break;
                        case "SoundAVolume":
                            if (int.TryParse(value, out int soundAVolume))
                            {
                                TrackBar_SoundAVolume.Value = Math.Clamp(soundAVolume, TrackBar_SoundAVolume.Minimum, TrackBar_SoundAVolume.Maximum);
                            }
                            break;
                        case "SoundBVolume":
                            if (int.TryParse(value, out int soundBVolume))
                            {
                                TrackBar_SoundBVolume.Value = Math.Clamp(soundBVolume, TrackBar_SoundBVolume.Minimum, TrackBar_SoundBVolume.Maximum);
                            }
                            break;
                        case "Power":
                            if (bool.TryParse(value, out bool power))
                            {
                                CheckBox_Power.Checked = power;
                            }
                            break;
                        case "VolumeHalf":
                            if (bool.TryParse(value, out bool volumeHalf))
                            {
                                CheckBox_VolumeHalf.Checked = volumeHalf;
                            }
                            break;
                        case "SoundPlayB":
                            if (bool.TryParse(value, out bool soundPlayB))
                            {
                                CheckBox_SoundPlayB.Checked = soundPlayB;
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"設定ファイル読み込みエラー: {ex.Message}");
            }
        }

        /// <summary>
        /// UI設定をSetting.iniに出力するメソッド
        /// </summary>
        private void ExportSettingFile()
        {
            string settingFilePath = ".\\Setting.ini";

            try
            {
                var lines = new System.Collections.Generic.List<string>
                {
                    "[UI]",
                    $"TopMost={CheckBox_TopMost.Checked}",
                    $"MasterVolume={TrackBar_MasterVolume.Value}",
                    $"SoundAVolume={TrackBar_SoundAVolume.Value}",
                    $"SoundBVolume={TrackBar_SoundBVolume.Value}",
                    $"Power={CheckBox_Power.Checked}",
                    $"VolumeHalf={CheckBox_VolumeHalf.Checked}",
                    $"SoundPlayB={CheckBox_SoundPlayB.Checked}"
                };

                System.IO.File.WriteAllLines(settingFilePath, lines);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"設定ファイル保存エラー: {ex.Message}");
            }
        }

        /// <summary>
        /// MainForm_FormClosingイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ExportSettingFile();
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
        /// 全体音量バーの値変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrackBar_Volume_ValueChanged(object sender, EventArgs e)
        {
            Label_MasterVolume.Text = $"{TrackBar_MasterVolume.Value * 10}%";
            _sound.SetMasterVolume(TrackBar_MasterVolume.Value * 0.1f);
        }

        /// <summary>
        /// 音声A音量バーの値変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrackBar_SoundAVolume_ValueChanged(object sender, EventArgs e)
        {
            bool _isVolumeHalf = CheckBox_VolumeHalf.Checked;
            Label_SoundAVolume.Text = $"{TrackBar_SoundAVolume.Value * 10}%";
            if (ComboBox_SoundA.SelectedItem != null)
            {
                _sound.SetVolume(ComboBox_SoundA.SelectedItem.ToString(), TrackBar_SoundAVolume.Value * 0.1f * (_isVolumeHalf ? 0.5f : 1.0f));
            }
        }

        /// <summary>
        /// 音声B音量バーの値変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrackBar_SoundBVolume_ValueChanged(object sender, EventArgs e)
        {
            bool _isVolumeHalf = CheckBox_VolumeHalf.Checked;
            Label_SoundBVolume.Text = $"{TrackBar_SoundBVolume.Value * 10}%";
            if (ComboBox_SoundB.SelectedItem != null)
            {
                _sound.SetVolume(ComboBox_SoundB.SelectedItem.ToString(), TrackBar_SoundBVolume.Value * 0.1f * (_isVolumeHalf ? 0.5f : 1.0f));
            }
        }

        /// <summary>
        /// 音声Aテスト再生ボタンクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_SoundA_TestPlay_Click(object sender, EventArgs e)
        {
            string _soundName = ComboBox_SoundA.SelectedItem.ToString();
            float _volume = TrackBar_SoundAVolume.Value * 0.1f;
            try
            {
                _sound.SoundStop(_soundName);
                System.Threading.Thread.Sleep(100);
                _sound.SoundPlay(_soundName, false, _volume * _sound.fMasterVolume);
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
            float _volume = TrackBar_SoundBVolume.Value * 0.1f;
            try
            {
                _sound.SoundStop(_soundName);
                System.Threading.Thread.Sleep(100);
                _sound.SoundPlay(_soundName, false, _volume * _sound.fMasterVolume);
            }
            catch
            {
                MessageBox.Show("音声再生に失敗しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// CheckBox_VolumeHalf_CheckedChangedイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_VolumeHalf_CheckedChanged(object sender, EventArgs e)
        {
            bool _isVolumeHalf = CheckBox_VolumeHalf.Checked;
            bool _isSoundPlayB = CheckBox_SoundPlayB.Checked;
            float _soundAVolume = TrackBar_SoundAVolume.Value * 0.1f;
            float _soundBVolume = TrackBar_SoundBVolume.Value * 0.1f;

            switch (_alertManager.AlertPhase)
            {
                case "ブレーキ投入":
                    if (_isSoundPlayB)
                        _sound.SetVolume(ComboBox_SoundB.SelectedItem.ToString(), _soundBVolume * (_isVolumeHalf ? 0.5f : 1.0f));
                    else
                        _sound.SetVolume(ComboBox_SoundA.SelectedItem.ToString(), _soundAVolume * (_isVolumeHalf ? 0.5f : 1.0f));
                    break;
                default:
                    if (_isSoundPlayB)
                        _sound.SetVolume(ComboBox_SoundB.SelectedItem.ToString(), _soundBVolume);
                    else
                        _sound.SetVolume(ComboBox_SoundA.SelectedItem.ToString(), _soundAVolume);
                    break;
            }
        }

        /// <summary>
        /// CheckBox_SoundPlayB_CheckedChangedイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_SoundPlayB_CheckedChanged(object sender, EventArgs e)
        {
            bool _isVolumeHalf = CheckBox_VolumeHalf.Checked;
            bool _isSoundALoop = RadioButton_SoundA_LoopPlay.Checked;
            bool _isSoundBLoop = RadioButton_SoundB_LoopPlay.Checked;
            bool _isSoundPlayB = CheckBox_SoundPlayB.Checked;
            float _soundAVolume = TrackBar_SoundAVolume.Value * 0.1f;
            float _soundBVolume = TrackBar_SoundBVolume.Value * 0.1f;

            switch (_alertManager.AlertPhase)
            {
                case "ブレーキ投入":
                    if (_isSoundPlayB)
                    {
                        _sound.SoundStop(ComboBox_SoundA.SelectedItem.ToString());
                        System.Threading.Thread.Sleep(100);
                        _sound.SoundPlay(ComboBox_SoundB.SelectedItem.ToString(), _isSoundBLoop, _soundBVolume * (_isVolumeHalf ? 0.5f : 1.0f));
                    }
                    else
                    {
                        _sound.SoundStop(ComboBox_SoundB.SelectedItem.ToString());
                        System.Threading.Thread.Sleep(100);
                        _sound.SoundPlay(ComboBox_SoundA.SelectedItem.ToString(), _isSoundALoop, _soundAVolume * (_isVolumeHalf ? 0.5f : 1.0f));
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
