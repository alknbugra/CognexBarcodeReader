using System;
using System.Threading.Tasks;

namespace Sample2CognexBarcodeReader.Interfaces
{
    /// <summary>
    /// Barkod okuyucu servisi için temel interface
    /// </summary>
    public interface IBarcodeReaderService
    {
        /// <summary>
        /// Cihaz bağlantısı kurulduğunda tetiklenen event
        /// </summary>
        event EventHandler<EventArgs> DeviceConnected;

        /// <summary>
        /// Cihaz bağlantısı kesildiğinde tetiklenen event
        /// </summary>
        event EventHandler<EventArgs> DeviceDisconnected;

        /// <summary>
        /// Barkod okuma tamamlandığında tetiklenen event
        /// </summary>
        event EventHandler<BarcodeReadEventArgs> BarcodeRead;

        /// <summary>
        /// Servisi başlatır ve cihaz keşfini başlatır
        /// </summary>
        /// <returns>Başlatma işleminin başarı durumu</returns>
        Task<bool> StartAsync();

        /// <summary>
        /// Servisi durdurur ve kaynakları temizler
        /// </summary>
        /// <returns>Durdurma işleminin başarı durumu</returns>
        Task<bool> StopAsync();

        /// <summary>
        /// Servisin çalışıp çalışmadığını kontrol eder
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Cihazın bağlı olup olmadığını kontrol eder
        /// </summary>
        bool IsConnected { get; }
    }
}
