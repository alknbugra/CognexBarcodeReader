using System;

namespace Sample2CognexBarcodeReader.Models
{
    /// <summary>
    /// Barkod okuyucu konfigürasyon sınıfı
    /// </summary>
    public class BarcodeReaderConfig
    {
        /// <summary>
        /// Varsayılan timeout değeri (milisaniye)
        /// </summary>
        public const int DefaultTimeout = 5000;

        /// <summary>
        /// Varsayılan baudrate değeri
        /// </summary>
        public const int DefaultBaudrate = 115200;

        /// <summary>
        /// Varsayılan thread sleep süresi (milisaniye)
        /// </summary>
        public const int DefaultThreadSleep = 20;

        /// <summary>
        /// Sistem timeout değeri
        /// </summary>
        public int Timeout { get; set; } = DefaultTimeout;

        /// <summary>
        /// Seri port baudrate değeri
        /// </summary>
        public int Baudrate { get; set; } = DefaultBaudrate;

        /// <summary>
        /// Thread sleep süresi
        /// </summary>
        public int ThreadSleep { get; set; } = DefaultThreadSleep;

        /// <summary>
        /// Otomatik keşif yapılıp yapılmayacağı
        /// </summary>
        public bool AutoDiscovery { get; set; } = true;

        /// <summary>
        /// Debug modu aktif mi
        /// </summary>
        public bool DebugMode { get; set; } = false;

        /// <summary>
        /// Varsayılan konfigürasyon oluşturur
        /// </summary>
        /// <returns>Varsayılan konfigürasyon</returns>
        public static BarcodeReaderConfig CreateDefault()
        {
            return new BarcodeReaderConfig();
        }
    }
}
