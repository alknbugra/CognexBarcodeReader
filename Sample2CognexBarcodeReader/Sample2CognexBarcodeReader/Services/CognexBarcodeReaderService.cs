using Cognex.DataMan.SDK;
using Cognex.DataMan.SDK.Discovery;
using Cognex.DataMan.SDK.Utils;
using Sample2CognexBarcodeReader.Interfaces;
using Sample2CognexBarcodeReader.Models;
using Sample2CognexBarcodeReader.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using ConnectionState = Cognex.DataMan.SDK.ConnectionState;

namespace Sample2CognexBarcodeReader.Services
{
    /// <summary>
    /// Cognex barkod okuyucu servisi implementasyonu
    /// </summary>
    public class CognexBarcodeReaderService : IBarcodeReaderService, IDisposable
    {
        #region Private Fields

        private readonly BarcodeReaderConfig _config;
        private readonly SynchronizationContext _syncContext;
        private readonly object _lockObject = new object();
        private readonly CancellationTokenSource _cancellationTokenSource;

        private ResultCollector _results;
        private SerSystemDiscoverer _serSystemDiscoverer;
        private ISystemConnector _connector;
        private DataManSystem _system;
        private bool _isRunning;
        private bool _isConnected;

        #endregion

        #region Events

        /// <summary>
        /// Cihaz bağlantısı kurulduğunda tetiklenen event
        /// </summary>
        public event EventHandler<EventArgs> DeviceConnected;

        /// <summary>
        /// Cihaz bağlantısı kesildiğinde tetiklenen event
        /// </summary>
        public event EventHandler<EventArgs> DeviceDisconnected;

        /// <summary>
        /// Barkod okuma tamamlandığında tetiklenen event
        /// </summary>
        public event EventHandler<BarcodeReadEventArgs> BarcodeRead;

        #endregion

        #region Properties

        /// <summary>
        /// Servisin çalışıp çalışmadığını kontrol eder
        /// </summary>
        public bool IsRunning
        {
            get
            {
                lock (_lockObject)
                {
                    return _isRunning;
                }
            }
        }

        /// <summary>
        /// Cihazın bağlı olup olmadığını kontrol eder
        /// </summary>
        public bool IsConnected
        {
            get
            {
                lock (_lockObject)
                {
                    return _isConnected && _system?.State == ConnectionState.Connected;
                }
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Yeni CognexBarcodeReaderService örneği oluşturur
        /// </summary>
        /// <param name="config">Konfigürasyon ayarları</param>
        public CognexBarcodeReaderService(BarcodeReaderConfig config = null)
        {
            _config = config ?? BarcodeReaderConfig.CreateDefault();
            _syncContext = WindowsFormsSynchronizationContext.Current ?? new WindowsFormsSynchronizationContext();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Servisi başlatır ve cihaz keşfini başlatır
        /// </summary>
        /// <returns>Başlatma işleminin başarı durumu</returns>
        public async Task<bool> StartAsync()
        {
            try
            {
                lock (_lockObject)
                {
                    if (_isRunning)
                    {
                        return true; // Zaten çalışıyor
                    }
                    _isRunning = true;
                }

                if (_config.AutoDiscovery)
                {
                    await StartDeviceDiscoveryAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                LogError("Servis başlatılırken hata oluştu", ex);
                lock (_lockObject)
                {
                    _isRunning = false;
                }
                return false;
            }
        }

        /// <summary>
        /// Servisi durdurur ve kaynakları temizler
        /// </summary>
        /// <returns>Durdurma işleminin başarı durumu</returns>
        public async Task<bool> StopAsync()
        {
            try
            {
                _cancellationTokenSource.Cancel();

                lock (_lockObject)
                {
                    _isRunning = false;
                }

                await CleanupConnectionAsync();
                return true;
            }
            catch (Exception ex)
            {
                LogError("Servis durdurulurken hata oluştu", ex);
                return false;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Cihaz keşfini başlatır
        /// </summary>
        private async Task StartDeviceDiscoveryAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    _serSystemDiscoverer = new SerSystemDiscoverer();
                    _serSystemDiscoverer.SystemDiscovered += OnSerSystemDiscovered;
                    _serSystemDiscoverer.Discover();
                }
                catch (Exception ex)
                {
                    LogError("Cihaz keşfi başlatılırken hata oluştu", ex);
                }
            });
        }

        /// <summary>
        /// Seri port cihazı keşfedildiğinde çağrılır
        /// </summary>
        private void OnSerSystemDiscovered(SerSystemDiscoverer.SystemInfo systemInfo)
        {
            try
            {
                LogInfo($"Cihaz bulundu: {systemInfo.Name}, Port: {systemInfo.PortName}");

                var connector = new SerSystemConnector(systemInfo.PortName, systemInfo.Baudrate);
                _connector = connector;

                _system = new DataManSystem(_connector);
                _system.DefaultTimeout = _config.Timeout;

                // Event'leri bağla
                _system.SystemConnected += OnSystemConnected;
                _system.SystemDisconnected += OnSystemDisconnected;

                // Sonuç toplayıcıyı ayarla
                var requestedResultTypes = ResultTypes.ReadXml | ResultTypes.Image | ResultTypes.ImageGraphics;
                _results = new ResultCollector(_system, requestedResultTypes);
                _results.ComplexResultCompleted += OnComplexResultCompleted;
                _results.SimpleResultDropped += OnSimpleResultDropped;

                // Bağlantıyı kur
                _system.Connect();

                try
                {
                    _system.SetResultTypes(requestedResultTypes);
                }
                catch (Exception ex)
                {
                    LogError("Sonuç tipleri ayarlanırken hata oluştu", ex);
                }

                // Ana döngüyü başlat
                StartMainLoop();
            }
            catch (Exception ex)
            {
                LogError("Cihaz bağlantısı kurulurken hata oluştu", ex);
            }
        }

        /// <summary>
        /// Ana döngüyü başlatır
        /// </summary>
        private void StartMainLoop()
        {
            Task.Run(async () =>
            {
                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    try
                    {
                        await Task.Delay(_config.ThreadSleep, _cancellationTokenSource.Token);
                    }
                    catch (System.OperationCanceledException)
                    {
                        break;
                    }
                }
            });
        }

        /// <summary>
        /// Sistem bağlandığında çağrılır
        /// </summary>
        private void OnSystemConnected(object sender, EventArgs args)
        {
            _syncContext.Post(_ =>
            {
                lock (_lockObject)
                {
                    _isConnected = true;
                }
                LogInfo("Sistem bağlandı");
                DeviceConnected?.Invoke(this, EventArgs.Empty);
            }, null);
        }

        /// <summary>
        /// Sistem bağlantısı kesildiğinde çağrılır
        /// </summary>
        private void OnSystemDisconnected(object sender, EventArgs args)
        {
            _syncContext.Post(_ =>
            {
                lock (_lockObject)
                {
                    _isConnected = false;
                }
                LogInfo("Sistem bağlantısı kesildi");
                DeviceDisconnected?.Invoke(this, EventArgs.Empty);
            }, null);
        }

        /// <summary>
        /// Karmaşık sonuç tamamlandığında çağrılır
        /// </summary>
        private void OnComplexResultCompleted(object sender, ComplexResult e)
        {
            _syncContext.Post(_ =>
            {
                ProcessComplexResult(e);
            }, null);
        }

        /// <summary>
        /// Basit sonuç düştüğünde çağrılır
        /// </summary>
        private void OnSimpleResultDropped(object sender, SimpleResult e)
        {
            _syncContext.Post(_ =>
            {
                LogWarning($"Kısmi sonuç düştü: {e.Id.Type}, id={e.Id.Id}");
            }, null);
        }

        /// <summary>
        /// Karmaşık sonucu işler
        /// </summary>
        private void ProcessComplexResult(ComplexResult complexResult)
        {
            try
            {
                var images = new List<Image>();
                var imageGraphics = new List<string>();
                string readResult = null;
                int resultId = -1;
                string barcodeType = "Unknown";

                foreach (var simpleResult in complexResult.SimpleResults)
                {
                    switch (simpleResult.Id.Type)
                    {
                        case ResultTypes.Image:
                            var image = ImageArrivedEventArgs.GetImageFromImageBytes(simpleResult.Data);
                            if (image != null)
                                images.Add(image);
                            break;

                        case ResultTypes.ImageGraphics:
                            imageGraphics.Add(simpleResult.GetDataAsString());
                            break;

                        case ResultTypes.ReadXml:
                            readResult = ParseReadStringFromXml(simpleResult.GetDataAsString());
                            resultId = simpleResult.Id.Id;
                            barcodeType = ExtractBarcodeTypeFromXml(simpleResult.GetDataAsString());
                            break;

                        case ResultTypes.ReadString:
                            readResult = simpleResult.GetDataAsString();
                            resultId = simpleResult.Id.Id;
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(readResult))
                {
                    var barcodeImage = images.FirstOrDefault();
                    var eventArgs = new BarcodeReadEventArgs(readResult, barcodeImage, resultId, barcodeType);
                    
                    LogInfo($"Barkod okundu: {readResult} (ID: {resultId})");
                    BarcodeRead?.Invoke(this, eventArgs);
                }
            }
            catch (Exception ex)
            {
                LogError("Sonuç işlenirken hata oluştu", ex);
            }
        }

        /// <summary>
        /// XML'den okuma string'ini parse eder
        /// </summary>
        private string ParseReadStringFromXml(string resultXml)
        {
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(resultXml);

                var fullStringNode = doc.SelectSingleNode("result/general/full_string");
                if (fullStringNode != null && _system?.State == ConnectionState.Connected)
                {
                    var encoding = fullStringNode.Attributes["encoding"];
                    if (encoding?.InnerText == "base64" && !string.IsNullOrEmpty(fullStringNode.InnerText))
                    {
                        var code = Convert.FromBase64String(fullStringNode.InnerText);
                        return _system.Encoding.GetString(code, 0, code.Length);
                    }
                    return fullStringNode.InnerText;
                }
            }
            catch (Exception ex)
            {
                LogError("XML parse edilirken hata oluştu", ex);
            }

            return string.Empty;
        }

        /// <summary>
        /// XML'den barkod tipini çıkarır
        /// </summary>
        private string ExtractBarcodeTypeFromXml(string resultXml)
        {
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(resultXml);
                var barcodeTypeNode = doc.SelectSingleNode("result/general/barcode_type");
                return barcodeTypeNode?.InnerText ?? "Unknown";
            }
            catch
            {
                return "Unknown";
            }
        }

        /// <summary>
        /// Bağlantıyı temizler
        /// </summary>
        private async Task CleanupConnectionAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    if (_system != null)
                    {
                        _system.SystemConnected -= OnSystemConnected;
                        _system.SystemDisconnected -= OnSystemDisconnected;
                        _system.Dispose();
                    }

                    _results?.Dispose();
                    _connector = null;
                    _system = null;
                    _serSystemDiscoverer?.Dispose();
                }
                catch (Exception ex)
                {
                    LogError("Bağlantı temizlenirken hata oluştu", ex);
                }
            });
        }

        #endregion

        #region Logging

        private void LogInfo(string message)
        {
            if (_config.DebugMode)
            {
                Console.WriteLine($"[INFO] {DateTime.Now:HH:mm:ss} - {message}");
            }
        }

        private void LogWarning(string message)
        {
            Console.WriteLine($"[WARNING] {DateTime.Now:HH:mm:ss} - {message}");
        }

        private void LogError(string message, Exception ex = null)
        {
            var errorMessage = $"[ERROR] {DateTime.Now:HH:mm:ss} - {message}";
            if (ex != null)
            {
                errorMessage += $" - Exception: {ex.Message}";
            }
            Console.WriteLine(errorMessage);
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            StopAsync().Wait();
            _cancellationTokenSource?.Dispose();
        }

        #endregion
    }
}
