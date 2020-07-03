// ==================================================
// 文件名：MachineUtil.cs
// 创建时间：2020/04/12 16:25
// 上海芸浦信息技术有限公司
// copyright@yumpoo
// ==================================================
// 最后修改于：2020/05/11 16:25
// 修改人：jians
// ==================================================

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using log4net;
using ProcessControlService.Contracts;
using ProcessControlService.WCFClients;

namespace FactoryWindowGUI.Util
{
    public class MachineUtil
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MachineUtil));

        private IHostConnection _machineHost;

        public MachineUtil()
        {
            ConnectToServerTask = Task.Run(ConnectToServer);
        }

        public bool Connected => _machineHost != null && _machineHost.Connected;

        public void ConnectToServer()
        {
            try
            {
                _machineHost = HostConnectionManager.CreateConnection(HostConnectionType.Machine);
            }
            catch (Exception ex)
            {
                Log.Error("连接Machine服务端失败：" + ex.Message);
            }
        }

        /// <summary>
        ///     get machine list from service
        /// </summary>
        /// <returns></returns>
        public List<string> GetMachineList()
        {
            try
            {
                var proxy = (MachineProxy) _machineHost.GetProxy();

                return proxy?.ListMachineNames();
            }
            catch (Exception ex)
            {
                Log.Error("获取MachineList失败：" + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///     get all tag name according to machine name
        /// </summary>
        /// <param name="machineName"></param>
        /// <returns></returns>
        public List<string> GetTagList(string machineName)
        {
            try
            {
                var proxy = (MachineProxy) _machineHost.GetProxy();

                return proxy?.ListMachineTags(machineName);
            }
            catch (Exception ex)
            {
                Log.Error("获取TagList失败：" + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///     search all tag value using Dictionary(K:machine name ,V:tag name)
        /// </summary>
        /// <param name="machineTagList"></param>
        /// <returns></returns>
        public List<string[]> SearchTagValue(List<string[]> machineTagList)
        {
            try
            {
                var proxy = (MachineProxy) _machineHost.GetProxy();

                return proxy?.GetTagsValue(machineTagList);
            }
            catch (Exception ex)
            {
                Log.Error("查询所有Tag值时失败：" + ex.Message);
                return null;
            }
        }

        public List<string[]> WriteTagValue(List<string[]> machineTagList)
        {
            try
            {
                var proxy = (MachineProxy) _machineHost.GetProxy();

                return proxy?.SetTagsValue(machineTagList);
            }
            catch (Exception ex)
            {
                Log.Error("写入所有Tag值时失败：" + ex.Message);
                return null;
            }
        }

        public List<DataSourceConnectionModel> GetMachineDataSourceConn()
        {
            try
            {
                var proxy = (MachineProxy) _machineHost.GetProxy();

                return proxy?.GetAllDataSourceConn();
            }
            catch (Exception ex)
            {
                Log.Error("获取机器数据源连接状态时失败：" + ex.Message);
                return null;
            }
        }

        public List<string> GetMachineDataSourceName(string machine)
        {
            try
            {
                var proxy = (MachineProxy) _machineHost.GetProxy();

                return proxy?.GetMachineDataSourceName(machine);
            }
            catch (Exception ex)
            {
                Log.Error("获取机器数据源列表时失败：" + ex.Message);
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
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed) //_isDisposed为false表示没有进行手动dispose
                if (disposing)
                    //清理托管资源
                    if (_machineHost != null)
                    {
                        HostConnectionManager.ReleaseConnection(HostConnectionType.Machine);
                        _machineHost = null;
                    }
            //清理非托管资源

            _isDisposed = true;
        }

        private bool _isDisposed;
        public Task ConnectToServerTask;

        ~MachineUtil()
        {
            Dispose(false); //释放非托管资源，托管资源由终极器自己完成了
        }

        #endregion
    }
}