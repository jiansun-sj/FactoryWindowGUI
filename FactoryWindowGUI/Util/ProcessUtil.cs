// ==================================================
// 文件名：ProcessUtil.cs
// 创建时间：2020/04/30 16:25
// 上海芸浦信息技术有限公司
// copyright@yumpoo
// ==================================================
// 最后修改于：2020/05/11 16:25
// 修改人：jians
// ==================================================

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using log4net;
using ProcessControlService.Contracts.ProcessData;
using ProcessControlService.WCFClients;

namespace FactoryWindowGUI.Util
{
    public sealed class ProcessUtil : IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ProcessUtil));

        private IHostConnection _processHost;

        public ProcessUtil()
        {
            Task.Run(ConnectToServer);
        }

        public bool Connected => _processHost != null && _processHost.Connected;

        private object _processLocker = new object();

        private void ConnectToServer()
        {
            try
            {
                _processHost = HostConnectionManager.CreateConnection(HostConnectionType.Process);
            }
            catch (Exception ex)
            {
                Log.Error("连接到Process服务端失败：" + ex.Message);
            }
        }

        /// <summary>
        ///     get all process name list from services
        /// </summary>
        /// <returns></returns>
        public List<string> GetProcessList()
        {
            try
            {
                var proxy = (ProcessProxy) _processHost.GetProxy();

                return proxy?.ListProcessNames();
            }
            catch (Exception ex)
            {
                Log.Error("获取GetProcessList失败：" + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///     get work flow chart info(K:sequence,V:stepName)
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public Dictionary<short, List<string>> GetWorkFlowChartInfo(string processName)
        {
            try
            {
                var proxy = (ProcessProxy) _processHost.GetProxy();

                return proxy?.GetProcessAllStepsIdName(processName);
            }
            catch (Exception ex)
            {
                Log.Error("获取GetWorkFlowChartInfo:" + processName + "失败:" + ex.Message);
                return null;
            }
        }

        public List<ProcessInstanceRecord> ReadProcessRecords(string processName, int readCount,
            DateTime searchDateTime)
        {
            try
            {
                lock (_processLocker)
                {
                    if (string.IsNullOrEmpty(processName)|| readCount<0 || readCount>10000)
                    {
                        MessageBox.Show($"Process历史记录查询条件不合法！");
                        return new List<ProcessInstanceRecord>();
                    }
                
                    var proxy = (ProcessProxy) _processHost.GetProxy();

                    return proxy?.ReadProcessInstanceRecords(processName, readCount, searchDateTime) ??
                           new List<ProcessInstanceRecord>();
                }
            }
            catch (Exception ex)
            {
                Log.Error("获取GetCurrentStep:" + processName + "失败:" + ex.Message);
                return new List<ProcessInstanceRecord>();
            }
        }


        public short GetCurrentStep(string processName)
        {
            try
            {
                var proxy = (ProcessProxy) _processHost.GetProxy();

                return proxy?.GetProcessStep(processName) ?? (short) 0;
            }
            catch (Exception ex)
            {
                Log.Error("获取GetCurrentStep:" + processName + "失败:" + ex.Message);
                return 0;
            }
        }

        public void StopProcessInstance(string processName)
        {
            try
            {
                var proxy = (ProcessProxy) _processHost.GetProxy();

                proxy?.StopProcessInstance(processName);
            }
            catch (Exception ex)
            {
                Log.Error("停止Process:" + processName + "失败:" + ex.Message);
            }
        }

        public List<string> GetStaticResources()
        {
            try
            {
                var proxy = (ProcessProxy) _processHost.GetProxy();

                return proxy == null ? new List<string>() : proxy.ReadStaticResourceNames();
            }
            catch (Exception ex)
            {
                Log.Error("读取非Process的资源名称失败" + ex.Message);
                return new List<string>();
            }
        }

        public void StartProcess(string processName, Dictionary<string, string> containers,
            Dictionary<string, string> inParameters)
        {
            try
            {
                var proxy = (ProcessProxy) _processHost.GetProxy();

                proxy?.StartProcess(processName, containers, inParameters);
            }
            catch (Exception ex)
            {
                Log.Error("停止Process:" + processName + "失败:" + ex.Message);
            }
        }

        public Dictionary<string, string> GetProcessAttributeLt(string processName)
        {
            try
            {
                var proxy = (ProcessProxy) _processHost.GetProxy();

                return proxy?.ListProcessInParameters(processName);
            }
            catch (Exception ex)
            {
                Log.Error("获取GetProcessAttributeLt失败：" + ex.Message);
                return null;
            }
        }

        public List<ProcessInfoModel> GetProcessInfoModels()
        {
            try
            {
                lock (_processLocker)
                {
                    var proxy = (ProcessProxy) _processHost.GetProxy();

                    return proxy?.GetProcessInfos();
                }
            }
            catch (Exception e)
            {
                Log.Error($"获取Process基本信息失败，异常为:[{e.Message}]");
                return null;
            }
        }

        public List<ProcessInstanceInfoModel> GetInstanceInfoModels(string processName)
        {
            try
            {
                lock (_processLocker)
                {
                    var proxy = (ProcessProxy) _processHost.GetProxy();

                    return proxy?.GetProcessInstanceInfoModels(processName);
                }
            }
            catch (Exception e)
            {
                Log.Error($"获取ProcessInstance的详情失败，异常为：[{e.Message}]");
                return null;
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
        private void Dispose(bool disposing)
        {
            if (!_isDisposed) //_isDisposed为false表示没有进行手动dispose
                if (disposing)
                    //清理托管资源
                    if (_processHost != null)
                    {
                        HostConnectionManager.ReleaseConnection(HostConnectionType.Process);
                        _processHost.Dispose();
                        _processHost = null;
                    }
            //清理非托管资源

            _isDisposed = true;
        }

        private bool _isDisposed;

        ~ProcessUtil()
        {
            Dispose(false); //释放非托管资源，托管资源由终极器自己完成了
        }

        #endregion
    }
}