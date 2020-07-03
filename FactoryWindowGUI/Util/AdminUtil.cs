// ==================================================
// 文件名：AdminUtil.cs
// 创建时间：2020/04/12 16:25
// 上海芸浦信息技术有限公司
// copyright@yumpoo
// ==================================================
// 最后修改于：2020/05/11 16:25
// 修改人：jians
// ==================================================

using System;
using System.Threading.Tasks;
using log4net;
using ProcessControlService.WCFClients;

namespace FactoryWindowGUI.Util
{
    public class AdminUtil
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AdminUtil));

        private IHostConnection _adminHost;

        public AdminUtil()
        {
            Task.Run(ConnectToServer);
        }

        public bool Connected => _adminHost != null && _adminHost.Connected;

        public void ConnectToServer()
        {
            try
            {
                _adminHost = HostConnectionManager.CreateConnection(HostConnectionType.Admin);
            }
            catch (Exception ex)
            {
                Log.Error("连接AdminHost服务端失败：" + ex.Message);
            }
        }

        /// <summary>
        ///     get machine list from service
        /// </summary>
        /// <returns></returns>
        public string GetRedundancyStatus()
        {
            try
            {
                var proxy = (AdminProxy) _adminHost.GetProxy();

                if (proxy == null) return string.Empty;
                switch (proxy.GetRedundancyMode())
                {
                    case 0:
                        return "未知";
                    case 1:
                        return "主机";
                    case 2:
                        return "从机";
                    default:
                        return "错误";
                }
            }
            catch (Exception ex)
            {
                Log.Error("获取冗余状态失败：" + ex.Message);
                return null;
            }
        }

        public void ToggleRedundancyMode()
        {
            try
            {
                var proxy = (AdminProxy) _adminHost.GetProxy();

                proxy.ToggleRedundancyMode();
            }
            catch (Exception ex)
            {
                Log.Error("切换冗余状态失败：" + ex.Message);
            }
        }

        #region Dispose

        public void Dispose()
        {
            Dispose(true); ////释放托管资源
            GC.SuppressFinalize(this); //请求系统不要调用指定对象的终结器. //该方法在对象头中设置一个位，系统在调用终结器时将检查这个位
        }

        /// <summary>
        ///     释放资源
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed) //_isDisposed为false表示没有进行手动dispose
                if (disposing)
                    //清理托管资源
                    if (_adminHost != null)
                    {
                        HostConnectionManager.ReleaseConnection(HostConnectionType.Admin);
                        _adminHost = null;
                    }
            //清理非托管资源

            _isDisposed = true;
        }

        private bool _isDisposed;

        ~AdminUtil()
        {
            Dispose(false); //释放非托管资源，托管资源由终极器自己完成了
        }

        #endregion
    }
}