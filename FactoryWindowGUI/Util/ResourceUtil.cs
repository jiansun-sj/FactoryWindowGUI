// ==================================================
// 文件名：ResourceUtil.cs
// 创建时间：2020/04/12 16:25
// 上海芸浦信息技术有限公司
// copyright@yumpoo
// ==================================================
// 最后修改于：2020/05/11 16:25
// 修改人：jians
// ==================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FactoryWindowGUI.Model;
using log4net;
using ProcessControlService.Contracts.ProcessData;
using ProcessControlService.WCFClients;

namespace FactoryWindowGUI.Util
{
    public class ResourceUtil
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ResourceUtil));

        private IHostConnection _resourceHost;

        public ResourceUtil()
        {
            Task.Run(ConnectToServer);
        }

        public bool Connected => _resourceHost != null && _resourceHost.Connected;

        public void ConnectToServer()
        {
            try
            {
                _resourceHost = HostConnectionManager.CreateConnection(HostConnectionType.Resource);
            }
            catch (Exception ex)
            {
                Log.Error("连接Resource服务端失败：" + ex.Message);
            }
        }

        /// <summary>
        ///     get resource list from service
        /// </summary>
        /// <returns></returns>
        public List<string> GetResourceList()
        {
            try
            {
                var proxy = (ResourceProxy) _resourceHost.GetProxy();

                return proxy?.GetAllResources();
            }
            catch (Exception ex)
            {
                Log.Error("获取资源列表失败：" + ex.Message);
                return null;
            }
        }

        public List<MemoryAndCpuData> GetMemoryAndCpuData(DateTime dateTime)
        {
            try
            {
                var proxy = (ResourceProxy)_resourceHost.GetProxy();

                return proxy?.GetMemoryAndCpuData(dateTime);
            }
            catch (Exception ex)
            {
                Log.Error("获取服务端内存使用和CPU占用率失败：" + ex.Message);
                return null;
            }

        }

        /// <summary>
        ///     get service list from a resource
        /// </summary>
        /// <returns></returns>
        public List<ResourceServiceGuiModel> GetServicesList(string resourceName)
        {
            try
            {
                var proxy = (ResourceProxy) _resourceHost.GetProxy();

                if (proxy == null) return null;
                var i = 0;
                return proxy.GetResourceService(resourceName).Select(item => new ResourceServiceGuiModel
                    {Id = i++, Name = item.Name, ServiceModel = item}).ToList();
            }
            catch (Exception ex)
            {
                Log.Error($"获取资源{resourceName}服务列表失败：{ex.Message}");
                return null;
            }
        }

        /// <summary>
        ///     call resource service from service
        /// </summary>
        /// <returns></returns>
        public string CallResourceService(string resourceName, string serviceName, string parameter = null)
        {
            try
            {
                var proxy = (ResourceProxy) _resourceHost.GetProxy();

                return proxy?.CallResourceService(resourceName, serviceName, parameter);
            }
            catch (Exception ex)
            {
                Log.Error($"调用资源:{resourceName}服务:{serviceName}失败：" + ex.Message);
                return null;
            }
        }

        #region Dispose

        public void Dispose()
        {
            Dispose(true); ////释放托管资源
            GC.SuppressFinalize(this); //请求系统不要调用指定对象的终结器.
            //该方法在对象头中设置一个位，系统在调用终结器时将检查这个位
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
                    if (_resourceHost != null)
                    {
                        HostConnectionManager.ReleaseConnection(HostConnectionType.Resource);
                        _resourceHost = null;
                    }
            //清理非托管资源

            _isDisposed = true;
        }

        private bool _isDisposed;

        ~ResourceUtil()
        {
            Dispose(false); //释放非托管资源，托管资源由终极器自己完成了
        }

        #endregion
    }
}