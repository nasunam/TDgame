using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Game.Core
{
    /// <summary>
    /// 全局时间流速管理。使用计数方式，支持多个 UI 同时请求慢速。
    /// 只有全部释放才恢复 1x。
    /// </summary>
    public class TimeScaleService
    {
        private const float NormalSpeed = 1f;
        private const float SlowSpeed = 0.15f;
        private int _slowRequests;
        public void RequestSlow()
        {
            _slowRequests++;
            if (_slowRequests == 1) Time.timeScale = SlowSpeed;
        }
        public void ReleaseSlow()
        {
            _slowRequests = Mathf.Max(0, _slowRequests - 1);
            if (_slowRequests == 0) Time.timeScale = NormalSpeed;
        }
        public void Reset()
        {
            _slowRequests = 0;
            Time.timeScale = NormalSpeed;
        }
    }
}
