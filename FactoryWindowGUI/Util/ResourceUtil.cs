// ==================================================
// FactoryWindowGUI
// 文件名：ResourceUtil.cs
// 创建时间：2020/04/12 18:20
// ==================================================
// 最后修改于：2020/08/21 18:20
// 修改人：jians
// ==================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FactoryWindowGUI.Model;
using log4net;
using ProcessControlService.Contracts.ProcessData;
using ProcessControlService.WCFClients;

namespace FactoryWindowGUI.Util
{
    public static class ResourceUtil
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ResourceUtil));

        private static IHostConnection _resourceHost;
        private static ResourceProxy _proxy;

        public static ResourceProxy ResourceProxy => _proxy ?? (_proxy = (ResourceProxy) ResourceHost?.GetProxy());

        public static IHostConnection ResourceHost => _resourceHost ??
                                                      (_resourceHost =
                                                          HostConnectionManager.CreateConnection(HostConnectionType
                                                              .Resource));

        public static bool? Connected =>  ResourceProxy?.Connected ;

        /*
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
        */

        /// <summary>
        ///     get resource list from service
        /// </summary>
        /// <returns></returns>
        public static List<string> GetResourceList()
        {
            try
            {
                CheckConnection();

                return ResourceProxy?.GetAllResources();
            }
            catch (Exception ex)
            {
                Log.Error("获取资源列表失败：" + ex.Message);
                return null;
            }
        }

        public static List<MemoryAndCpuData> GetMemoryAndCpuData(DateTime dateTime)
        {
            try
            {
                CheckConnection();

                return ResourceProxy?.GetMemoryAndCpuData(dateTime);
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
        public static List<ResourceServiceGuiModel> GetServicesList(string resourceName)
        {
            try
            {
                CheckConnection();

                var i = 0;
                var resourceServiceModels = ResourceProxy?.GetResourceService(resourceName);
                return resourceServiceModels?.Select(item => new ResourceServiceGuiModel
                    {Id = i++, Name = item.Name, ServiceModel = item}).ToList();
            }
            catch (Exception ex)
            {
                Log.Error($"获取资源{resourceName}服务列表失败：{ex.Message}");
                return null;
            }
        }

        private static readonly object ConnectionLocker=new object();

        private static void CheckConnection()
        {
            lock (ConnectionLocker)
            {
                if (Connected!=true)
                {
                    ResourceHost.StartConnect();

                    Thread.Sleep(1000);

                    _proxy = (ResourceProxy)ResourceHost?.GetProxy();
                }
            }
        }

        /// <summary>
        ///     call resource service from service
        /// </summary>
        /// <returns></returns>
        public static string CallResourceService(string resourceName, string serviceName, string parameter = null)
        {
            try
            {
                CheckConnection();

                return ResourceProxy?.CallResourceService(resourceName, serviceName, parameter);
            }
            catch (Exception ex)
            {
                Log.Error($"调用资源:{resourceName}服务:{serviceName}失败：" + ex.Message);
                return null;
            }
        }

        public static async Task<string> CallResourceServiceAsync(string resourceName, string serviceName,
            string parameter = null)
        {
            try
            {
                CheckConnection();

                return await ResourceProxy?.CallResourceServiceAsc(resourceName, serviceName, parameter);
            }
            catch (Exception ex)
            {
                Log.Error($"调用资源:{resourceName}服务:{serviceName}失败：" + ex.Message);
                return null;
            }
        }

        public static string CallResourceServiceParams(string resourceName, string serviceName,
           params object[] parameter)
        {
            try
            {
                CheckConnection();

                return ResourceProxy?.CallResourceServiceParams(resourceName, serviceName, parameter);
            }
            catch (Exception ex)
            {
                Log.Error($"调用资源:{resourceName}服务:{serviceName}失败：" + ex.Message);
                return null;
            }
        }

        #region Dispose

        /*public static void Dispose()
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
        }*/

        #endregion
    }
}