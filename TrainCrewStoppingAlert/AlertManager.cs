using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TrainCrew;

namespace TrainCrewStoppingAlert
{
    /// <summary>
    /// 停車予報アラート管理クラス
    /// </summary>
    public class AlertManager
    {
        private CancellationTokenSource? _cts;
        private bool _isPower;
        private bool _isStationStopped;

        public string AlertPhase;
        public bool IsStoppingPatternReceived;
        public bool IsBrakeApplied;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AlertManager()
        {
            InitializeAlertPhase();
        }

        /// <summary>
        /// 遅延実行呼び出しメソッド
        /// </summary>
        /// <param name="seconds">遅延秒数[s]</param>
        /// <param name="action">呼び出し処理</param>
        /// <param name="cancellationToken">キャンセルトークン</param>
        /// <returns></returns>
        private async Task WaitForAsync(float seconds, Action action, CancellationToken cancellationToken = default)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(seconds), cancellationToken);
                if (!cancellationToken.IsCancellationRequested)
                {
                    action();
                }
            }
            catch (TaskCanceledException)
            {
            }
        }

        /// <summary>
        /// 実行中の遅延処理をキャンセル
        /// </summary>
        private void CancelWaitFor()
        {
            if (_cts != null)
            {
                _cts?.Cancel();
                _cts?.Dispose();
            }
            _cts = new CancellationTokenSource();
        }

        /// <summary>
        /// 電源状態更新メソッド
        /// </summary>
        /// <param name="isPower"></param>
        public void UpdatePower(bool isPower)
        {
            _isPower = isPower;
        }

        /// <summary>
        /// 停車予報更新メソッド
        /// </summary>
        /// <param name="_state"></param>
        public void UpdateAlertPhase(TrainState _state)
        {
            try
            {
                // 駅停車判定
                _isStationStopped = (-3.0f < _state.nextStaDistance && _state.nextStaDistance < 3.0f) && FloatExtensions.IsZero(_state.Speed);

                // 電源未投入なら初期化して終了
                if (!_isPower)
                {
                    InitializeAlertPhase();
                    return;
                }

                switch (AlertPhase)
                {
                    case "停P受信待機":
                        if (!_isStationStopped && _state.ATS_State.Contains("停P"))
                        {
                            AlertPhase = "停P受信";
                            IsStoppingPatternReceived = true;
                        }
                        break;
                    case "停P受信":
                        if (_state.Bnotch > 0)
                        {
                            AlertPhase = "ブレーキ投入";
                            IsBrakeApplied = true;
                        }
                        // 停止範囲内に停止後、ドア開で動作完了
                        else if (_isStationStopped && !_state.AllClose)
                        {
                            InitializeAlertPhase();
                        }
                        // 停止範囲内に停止後、5秒経過で動作完了
                        else if (_isStationStopped)
                        {
                            CancelWaitFor();
                            _ = WaitForAsync(5.0f, () => InitializeAlertPhase(), _cts.Token);
                        }
                        else
                        {
                            CancelWaitFor();
                        }
                        break;
                    case "ブレーキ投入":
                        // 停止範囲内に停止後、ドア開で動作完了
                        if (_isStationStopped && !_state.AllClose)
                        {
                            InitializeAlertPhase();
                        }
                        // 停止範囲内に停止後、5秒経過で動作完了
                        else if (_isStationStopped)
                        {
                            CancelWaitFor();
                            _ = WaitForAsync(5.0f, () => InitializeAlertPhase(), _cts.Token);
                        }
                        else
                        {
                            CancelWaitFor();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// AlertManagerの停車予報関連の状態を初期化するメソッド
        /// </summary>
        public void InitializeAlertPhase()
        {
            CancelWaitFor();

            AlertPhase = "停P受信待機";
            IsStoppingPatternReceived = false;
            IsBrakeApplied = false;

            _isStationStopped = true;
        }
    }
}
